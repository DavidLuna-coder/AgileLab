using System.Text.Json.Serialization;

namespace TFG.SonarQubeClient.Models.Metrics
{
	public class SonarMeasure
	{
		[JsonPropertyName("metric")]
		public string Metric { get; set; } = string.Empty;
		[JsonPropertyName("value")]
		public string Value { get; set; } = string.Empty;
	}
}
