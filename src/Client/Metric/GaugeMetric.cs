
namespace LibratoSharp.Client.Metric
{
	public class GaugeMetric : BaseMetric
	{
		public override string Type
		{
			get
			{
				return "gauge";
			}
		}

		public GaugeMetric(string name)
			: base(name)
		{
		}
	}
}