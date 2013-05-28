using System;

namespace LibratoSharp.Client.Metric
{
	public class GaugeMeasurement : Gauge, IMeasurement, IMetric
	{
		protected object _value;
		protected DateTime _measurementTime;
		protected string _source;

		public static DateTime NO_TIME_SET = DateTime.MinValue;

		public virtual object Value
		{
			get
			{
				return this._value;
			}
		}

		public virtual DateTime MeasurementTime
		{
			get
			{
				return this._measurementTime;
			}
		}

		public virtual string Source
		{
			get
			{
				return this._source;
			}
		}

		protected GaugeMeasurement(string name, object value, DateTime measurementTime, string source)
			: base(name)
		{
			this._value = value;
			this._measurementTime = measurementTime;
			this._source = source;
		}

		public GaugeMeasurement(string name, int value)
			: this(name, value, GaugeMeasurement.NO_TIME_SET, null)
		{
		}

		public GaugeMeasurement(string name, double value)
			: this(name, value, GaugeMeasurement.NO_TIME_SET, null)
		{
		}
	}
}