using System.Text.Json.Serialization;

namespace TFG.OpenProjectClient.Models.BasicObjects.Errors
{
	public class EmbeddedError
	{
		[JsonPropertyName("details")]
		public ErrorDetails Details { get; set; }
		[JsonPropertyName("errors")]
		public Error[] Errors { get; set; }

	}
}
