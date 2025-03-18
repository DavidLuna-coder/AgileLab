using System.Text.Json.Serialization;

namespace TFG.SonarQubeClient.Models
{
    public class ProjectCreatedDetails
    {
		[JsonPropertyName("key")]
		public string Key { get; set; } = string.Empty;

		[JsonPropertyName("name")]
		public string Name { get; set; } = string.Empty;

		[JsonPropertyName("qualifier")]
		public string Qualifier { get; set; } = string.Empty;
	}
}
