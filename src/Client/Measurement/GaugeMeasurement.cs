using LibratoSharp.Client.Metric;
using System;

namespace LibratoSharp.Client.Measurement
{
	public class GaugeMeasurement : GaugeMetric, IMeasurement, IMetric
	{
		public object Value { get; private set; }
		public DateTime MeasurementTime { get; private set; }
		public string Source { get; private set; }
		
		public GaugeMeasurement(string name, object value, string source = "", DateTime measurementTime = new DateTime())
			: base(name)
		{
			this.Value = value;
			this.MeasurementTime = measurementTime;
			this.Source = source;
		}
	}
}