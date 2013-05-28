
namespace Client.Metric
{
	public class Gauge : BaseMetric
	{
		public override string Type
		{
			get
			{
				return "gauge";
			}
		}

		public Gauge(string name)
			: base(name)
		{
		}
	}
}