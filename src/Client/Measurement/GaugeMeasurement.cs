using LibratoSharp.Client.Metric;
using System;

namespace LibratoSharp.Client.Measurement
{
	public class GaugeMeasurement : GaugeMetric, IGaugeMeasurement, IMeasurement, IMetric
	{
		public object Value { get; private set; }
		public DateTime MeasurementTime { get; private set; }
		public string Source { get; private set; }

		public object Count { get; private set; }
		public object Sum { get; private set; }
		public object Max { get; private set; }
		public object Min { get; private set; }
		public object SumSquares { get; private set; }

		public GaugeMeasurement(string name, object value, string source = "", DateTime measurementTime = new DateTime())
			: base(name)
		{
			this.Value = value;
			this.MeasurementTime = measurementTime;
			this.Source = source;
		}

		public GaugeMeasurement SetupGaugeDetails(object count, object sum, object max = null, object min = null, object sumSquares = null)
		{
			this.Count = count;
			this.Sum = sum;
			this.Max = max;
			this.Min = min;
			this.SumSquares = sumSquares;
			return this;
		}
	}
}