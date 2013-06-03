using LibratoSharp.Client.Metric;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibratoSharp.Client.Measurement
{
	public class CounterMeasurement : CounterMetric, IMeasurement, IMetric
	{
		public object Value { get; private set; }
		public DateTime MeasurementTime { get; private set; }
		public string Source { get; private set; }

		public CounterMeasurement(string name, object value, string source = "", DateTime measurementTime = new DateTime())
			: base(name)
		{
			this.Value = value;
			this.MeasurementTime = measurementTime;
			this.Source = source;
		}
	}
}
