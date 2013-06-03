using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibratoSharp.Client.Metric
{
	public class CounterMetric : BaseMetric
	{
		public override string Type
		{
			get { return "counter"; }
		}

		public CounterMetric(string name)
			: base(name)
		{
		}
	}
}