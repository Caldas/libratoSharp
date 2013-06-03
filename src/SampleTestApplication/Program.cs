using LibratoSharp.Client;
using LibratoSharp.Client.Measurement;
using LibratoSharp.Client.Metric;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SampleTestApplication
{
	class Program
	{
		private const string HOST = "Vtex-Note-FabioCaldas";

		static void Main(string[] args)
		{
			MetricsManager metricsManager = new MetricsManager("logger-system@vtex.com.br", "109120cb5d2dbf2723b6a07f0fdee85cc4cb5e525a0d45360dcff65d21a197d5");

			metricsManager.CreateMetric(new CounterMetric("logger.client.counter") { Description = "Counter Test For Logger Client", DisplayName = "Logs Created" });
			/*
PUT https://metrics-api.librato.com/v1/metrics/logger.client.counter HTTP/1.1
Content-Type: application/json
Host: metrics-api.librato.com
Authorization: Basic bG9nZ2VyLXN5c3RlbUB2dGV4LmNvbS5icjoxMDkxMjBjYjVkMmRiZjI3MjNiNmEwN2YwZmRlZTg1Y2M0Y2I1ZTUyNWEwZDQ1MzYwZGNmZjY1ZDIxYTE5N2Q1
User-Agent: .NET API Client
Content-Length: 112
Expect: 100-continue
Connection: Keep-Alive

{
  "type": "counter",
  "display_name": "Logs Created",
  "description": "Counter Test For Logger Client"
}
			*/

			metricsManager.CreateMetric(new GaugeMetric("logger.client.gauge") { Description = "Gauge Test For Logger Client", DisplayName = "Logs Stats" });
			/*
PUT https://metrics-api.librato.com/v1/metrics/logger.client.gauge HTTP/1.1
Content-Type: application/json
Host: metrics-api.librato.com
Authorization: Basic bG9nZ2VyLXN5c3RlbUB2dGV4LmNvbS5icjoxMDkxMjBjYjVkMmRiZjI3MjNiNmEwN2YwZmRlZTg1Y2M0Y2I1ZTUyNWEwZDQ1MzYwZGNmZjY1ZDIxYTE5N2Q1
User-Agent: .NET API Client
Content-Length: 106
Expect: 100-continue

{
  "type": "gauge",
  "display_name": "Logs Stats",
  "description": "Gauge Test For Logger Client"
}
			*/


			metricsManager.PostMeasurement(new GaugeMeasurement("logger.client.gauge", new Random(DateTime.Now.Millisecond).Next(0, 1000), HOST) { });
			/*
POST https://metrics-api.librato.com/v1/metrics HTTP/1.1
Content-Type: application/json
Host: metrics-api.librato.com
Authorization: Basic bG9nZ2VyLXN5c3RlbUB2dGV4LmNvbS5icjoxMDkxMjBjYjVkMmRiZjI3MjNiNmEwN2YwZmRlZTg1Y2M0Y2I1ZTUyNWEwZDQ1MzYwZGNmZjY1ZDIxYTE5N2Q1
User-Agent: .NET API Client
Content-Length: 98
Expect: 100-continue

{
  "gauges": [
    {
      "name": "logger.client.gauge",
      "value": "263",
	  "source": "Vtex-Note-FabioCaldas"
    }
  ]
}			
			*/
			
			Parallel.For(0, 100, index =>
				{
					Interlocked.Increment(ref itemsDone);

					int sleepInterval = new Random(index).Next(0, 1000);
					Console.WriteLine("Index {0} going to sleep for {1} ms", index, sleepInterval);
					Thread.Sleep(sleepInterval);
					
					List<IMeasurement> measurements = new List<IMeasurement>();
					measurements.Add(new GaugeMeasurement("logger.client.gauge", sleepInterval, HOST));
					if(index % 10 == 0)
					{
						int currentItemsDone = Interlocked.Exchange(ref itemsDone, 0);
						Interlocked.Add(ref itemsSent, currentItemsDone);
						measurements.Add(new CounterMeasurement("logger.client.counter", currentItemsDone, HOST));
					}

					metricsManager.PostMeasurement(measurements);
				}
			);

			int currentItemsDoneRemaining = Interlocked.Exchange(ref itemsDone, 0);
			Interlocked.Add(ref itemsSent, currentItemsDoneRemaining);
			metricsManager.PostMeasurement(new CounterMeasurement("logger.client.counter", currentItemsDoneRemaining, HOST));

			Console.WriteLine("Items sent: {0}", itemsSent);
			Console.ReadLine();
		}

		private static int itemsDone = 0;
		private static int itemsSent = 0;
	}
}
