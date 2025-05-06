using System.Text.Json.Serialization;

namespace TFG.SonarQubeClient.Models.Metrics
{
	public class SonarMetricsResponse
	{
		[JsonPropertyName("component")]
		public SonarComponent Component { get; set; } = new();
	}
}
