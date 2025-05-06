using System.Text.Json.Serialization;

namespace TFG.SonarQubeClient.Models.Metrics
{
	public class SonarComponent
	{
		[JsonPropertyName("key")]
		public string Key { get; set; } = string.Empty;
		[JsonPropertyName("measures")]
		public List<SonarMeasure> Measures { get; set; } = new();
	}
}
