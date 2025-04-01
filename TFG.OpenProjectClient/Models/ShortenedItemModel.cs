using System.Text.Json.Serialization;

namespace TFG.OpenProjectClient.Models
{
	public class ShortenedItemModel
	{
		[JsonPropertyName("_type")]
		public string Type { get; set; }
		[JsonPropertyName("id")]
		public string Id { get; set; }
		[JsonPropertyName("_hint")]
		public string Hint { get; set; }
	}
}
