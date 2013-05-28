using System;

namespace Client.Metric
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