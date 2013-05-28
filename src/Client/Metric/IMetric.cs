
namespace LibratoSharp.Client.Metric
{
	public interface IMetric
	{
		string Name
		{
			get;
		}

		int Period
		{
			get;
		}

		string Description
		{
			get;
		}

		string DisplayName
		{
			get;
		}

		MetricAttribute[] Attributes
		{
			get;
		}

		string Type
		{
			get;
		}
	}
}