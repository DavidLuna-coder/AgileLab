using System.Text.Json.Serialization;

namespace TFG.OpenProjectClient.Models.BasicObjects
{
	public class Digest
	{
		[JsonPropertyName("algorithm")]
		public string Algorithm { get; set; }
		[JsonPropertyName("hash")]
		public string Hash { get; set; }
	}
}
