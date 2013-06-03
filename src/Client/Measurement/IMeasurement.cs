using LibratoSharp.Client.Metric;
using System;

namespace LibratoSharp.Client.Measurement
{
	public interface IMeasurement : IMetric
	{
		object Value
		{
			get;
		}

		DateTime MeasurementTime
		{
			get;
		}

		string Source
		{
			get;
		}
	}
}