using System.Text.Json.Serialization;

namespace TFG.OpenProjectClient.Models.BasicObjects.Errors
{
	public class Error : HalResource<EmbeddedError>
	{
		[JsonPropertyName("message")]
		public string Message { get; set; }
		[JsonPropertyName("errorIdentifier")]
		public string ErrorIdentifier { get; set; }
	}
}
