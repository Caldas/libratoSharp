using LibratoSharp.Client.Measurement;
using LibratoSharp.Client.Metric;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Linq;

namespace LibratoSharp.Client
{
	public class MetricsManager
	{
		private const string METRICS_API_SERVER = "metrics-api.librato.com";
		private const string METRICS_URL = "https://metrics-api.librato.com/v1/metrics";
		private const string METRICS_PUT_POST = "/{0}";
		private const string REQUEST_CONTENT_TYPE = "application/json";
		private const string JSON_METRIC_NAME = "name";
		private const string JSON_METRIC_PERIOD = "period";
		private const string JSON_METRIC_DESCRIPTION = "description";
		private const string JSON_METRIC_DISPLAY_NAME = "display_name";
		private const string JSON_METRIC_ATRIBUTES = "attributes";
		private const string JSON_MEASUREMENT_VALUE = "value";
		private const string JSON_MEASUREMENT_TIME = "measurement_time";
		private const string JSON_MEASUREMENT_SOURCE = "source";

		private string _user;
		private string _apiToken;

		public MetricsManager(string user, string apiToken)
		{
			if (string.IsNullOrEmpty(user) || string.IsNullOrEmpty(apiToken))
			{
				throw new ArgumentException("User and API Token are required");
			}
			this._user = user;
			this._apiToken = apiToken;
		}

		public void PostMeasurement(IMeasurement measurement)
		{
			if (measurement == null)
			{
				return;
			}
			this.PostMeasurement(new IMeasurement[]
			{
				measurement
			});
		}

		public void PostMeasurement(params IMeasurement[] measurements)
		{
			if (measurements == null || measurements.Length == 0)
			{
				return;
			}
			this.PostMeasurement(measurements.ToList());
		}

		public void PostMeasurement(List<IMeasurement> measurements)
		{
			if (measurements == null || measurements.Count == 0)
			{
				return;
			}
			string json = this.CreateJsonObject(measurements);
			this.MakeJsonPost(json, null, null);
		}

		private string CreateJsonObject(IEnumerable<IMeasurement> measurements)
		{
			StringBuilder sb = new StringBuilder();
			StringWriter sw = new StringWriter(sb);
			JsonWriter writer = new JsonTextWriter(sw);
			using (writer)
			{
				writer.Formatting = Formatting.Indented;
				writer.WriteStartObject();
				foreach (IGrouping<string, IMeasurement> measurementByType in measurements.GroupBy(measurement => measurement.Type))
				{
					writer.WritePropertyName(measurementByType.Key + "s");
					writer.WriteStartArray();
					foreach (IMeasurement measurement in measurements.Where(m => m.Type == measurementByType.Key))
					{
						this.JsonWriteMeasurement(writer, measurement);
					}
					writer.WriteEnd();
				}
				writer.WriteEndObject();
			}
			return sb.ToString();
		}

		private string CreateJsonObject(IMetric metric)
		{
			StringBuilder sb = new StringBuilder();
			StringWriter sw = new StringWriter(sb);
			JsonWriter writer = new JsonTextWriter(sw);
			using (writer)
			{
				writer.Formatting = Formatting.Indented;
				writer.WriteStartObject();
				writer.WritePropertyName("type");
				writer.WriteValue(metric.Type);
				this.JsonAddMetricInfo(writer, metric, false);
				writer.WriteEndObject();
			}
			return sb.ToString();
		}

		private void JsonAddMetricInfo(JsonWriter writer, IMetric metric, bool includeName)
		{
			if (includeName)
			{
				writer.WritePropertyName("name");
				writer.WriteValue(metric.Name);
			}
			if (metric.DisplayName != null)
			{
				writer.WritePropertyName("display_name");
				writer.WriteValue(metric.DisplayName);
			}
			if (metric.Description != null)
			{
				writer.WritePropertyName("description");
				writer.WriteValue(metric.Description);
			}
			if (metric.Period != -1)
			{
				writer.WritePropertyName("period");
				writer.WriteValue(metric.Period);
			}
		}

		private void JsonWriteMeasurement(JsonWriter writer, IMeasurement measurement)
		{
			writer.WriteStartObject();
			this.JsonAddMetricInfo(writer, measurement, true);
			if (measurement.MeasurementTime != DateTime.MinValue && measurement.MeasurementTime != default(DateTime))
			{
				long unixTimestamp = this.GetUnixTimestamp(measurement.MeasurementTime);
				writer.WritePropertyName("measurement_time");
				writer.WriteValue(unixTimestamp.ToString());
			}
			if (measurement.Source != null)
			{
				writer.WritePropertyName("source");
				writer.WriteValue(measurement.Source);
			}
			if (measurement.Type == "gauge")
			{
				IGaugeMeasurement gaugeMeasurement = (IGaugeMeasurement)measurement;

				writer.WritePropertyName("count");
				writer.WriteValue(gaugeMeasurement.Count);

				writer.WritePropertyName("sum");
				writer.WriteValue(gaugeMeasurement.Sum);

				if (gaugeMeasurement.Max != default(object)) 
				{
					writer.WritePropertyName("max");
					writer.WriteValue(gaugeMeasurement.Max);
				}

				if (gaugeMeasurement.Min != default(object))
				{
					writer.WritePropertyName("min");
					writer.WriteValue(gaugeMeasurement.Min);
				}

				if (gaugeMeasurement.SumSquares != default(object))
				{
					writer.WritePropertyName("sum_squares");
					writer.WriteValue(gaugeMeasurement.SumSquares);
				}
			}
			else
			{
				writer.WritePropertyName("value");
				writer.WriteValue(measurement.Value.ToString());
			}
			writer.WriteEndObject();
		}

		public void CreateMetric(IMetric metric)
		{
			if (metric == null)
			{
				throw new ArgumentException("metric is null");
			}
			string json = this.CreateJsonObject(metric);
			string url = string.Format("/{0}", metric.Name);
			this.MakeJsonPost(json, url, "PUT");
		}

		public void DeleteMetric(IMetric metric)
		{
			if (metric == null)
			{
				throw new ArgumentException("metric is null");
			}
			string url = string.Format("/{0}", metric.Name);
			this.MakeJsonPost(string.Empty, url, "DELETE");
		}

		private void MakeJsonPost(string json, string urlPostfix, string verb)
		{
			verb = ((verb == null) ? "POST" : verb);
			string url = (urlPostfix == null) ? "https://metrics-api.librato.com/v1/metrics" : ("https://metrics-api.librato.com/v1/metrics" + urlPostfix);
			WebRequest request = WebRequest.Create(url);
			request.Method = verb;
			request.ContentType = "application/json";
			Stream stream = request.GetRequestStream();
			this.SetBasicAuthHeader(request, this._user, this._apiToken);
			((HttpWebRequest)request).UserAgent = ".NET API Client";
			this.WriteJsonToStream(stream, json);
			WebResponse response = request.GetResponse();
			HttpStatusCode statusCode = ((HttpWebResponse)response).StatusCode;
			response.Close();
			if (statusCode != HttpStatusCode.OK && statusCode != HttpStatusCode.Created && statusCode != HttpStatusCode.NoContent)
			{
				throw new Exception("Error Posting Json");
			}
		}

		private void WriteJsonToStream(Stream stream, string json)
		{
			Encoding utf8Encoding = Encoding.UTF8;
			byte[] bytes = utf8Encoding.GetBytes(json);
			stream.Write(bytes, 0, bytes.Length);
		}

		private void SetBasicAuthHeader(WebRequest request, string userName, string userPassword)
		{
			string authInfo = userName + ":" + userPassword;
			authInfo = Convert.ToBase64String(Encoding.Default.GetBytes(authInfo));
			request.Headers["Authorization"] = "Basic " + authInfo;
		}

		private long GetUnixTimestamp(DateTime dt)
		{
			DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);
			return (long)Math.Floor((dt - origin).TotalSeconds);
		}
	}
}
