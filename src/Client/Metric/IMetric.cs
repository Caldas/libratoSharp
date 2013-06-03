
namespace LibratoSharp.Client.Metric
{
	public interface IMetric : ILibratoEntity
	{
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