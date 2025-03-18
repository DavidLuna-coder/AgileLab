using System.Text.Json.Serialization;

namespace TFG.SonarQubeClient.Models
{
	public class DopSetting
	{
		[JsonPropertyName("id")]
		public string Id { get; set; } = string.Empty;

		[JsonPropertyName("type")]
		public string Type { get; set; } = string.Empty;

		[JsonPropertyName("key")]
		public string Key { get; set; } = string.Empty;

		[JsonPropertyName("url")]
		public string Url { get; set; } = string.Empty;

		[JsonPropertyName("appId")]
		public string AppId { get; set; } = string.Empty;
	}

}
