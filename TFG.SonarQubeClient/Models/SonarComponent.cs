using System.Text.Json.Serialization;

namespace TFG.SonarQubeClient.Models
{
	public class SonarComponent
    {
        [JsonPropertyName("key")]
        public string Key { get; set; }
		[JsonPropertyName("description")]
		public string Description { get; set; }
        [JsonPropertyName("qualifier")]
        public string Qualifier { get; set; }
        [JsonPropertyName("path")]
        public string Path { get; set; }
        [JsonPropertyName("language")]
        public string Language { get; set; }
    }
}
