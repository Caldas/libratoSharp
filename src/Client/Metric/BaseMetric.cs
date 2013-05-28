using System.Collections;

namespace Client.Metric
{
	public abstract class BaseMetric : IMetric
	{
		protected string _name;
		protected int _period;
		protected string _description;
		protected string _displayName;
		protected ArrayList _attributes;

		public string Name
		{
			get
			{
				return this._name;
			}
		}

		public int Period
		{
			get
			{
				return this._period;
			}
			set
			{
				this._period = value;
			}
		}

		public string Description
		{
			get
			{
				return this._description;
			}
			set
			{
				this._description = value;
			}
		}

		public string DisplayName
		{
			get
			{
				return this._displayName;
			}
			set
			{
				this._displayName = value;
			}
		}

		public MetricAttribute[] Attributes
		{
			get
			{
				return (MetricAttribute[])this._attributes.ToArray(typeof(MetricAttribute));
			}
		}

		public abstract string Type
		{
			get;
		}

		public BaseMetric(string name)
		{
			this._name = name;
			this._period = -1;
			this._description = null;
			this._displayName = null;
			this._attributes = new ArrayList();
		}
	}
}