using System.Text.Json.Serialization;

namespace TFG.OpenProjectClient.Models.BasicObjects.Errors
{
	public class ErrorDetails : HalResource<object>
	{
		[JsonPropertyName("erroneousField")]
		public string ErroneousField { get; set; }
	}
}
