namespace TFG.SonarQubeClient.Models.Metrics
{
	public class SonarMetricsRequest
	{
		public string ProjectKey { get; set; }
		public List<string> MetricKeys { get; set; } = [];

		public string BuildRequestUri()
		{
			var metrics = string.Join(",", MetricKeys);
			return $"measures/component?component={ProjectKey}&metricKeys={metrics}";
		}
	}
}
