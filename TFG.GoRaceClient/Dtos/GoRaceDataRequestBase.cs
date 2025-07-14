using System.Text.Json;
using System.Text.Json.Serialization;

namespace TFG.GoRaceClient.Dtos
{
	public class GoRaceDataRequest
	{
		[JsonPropertyName("assignment")]
		public string Assignment { get; set; }
		[JsonPropertyName("email")]
		public string Email { get; set; }
		[JsonPropertyName("time")]
		public string Time { get; set; }

		[JsonExtensionData]
		public Dictionary<string, object>? Extra { get; set; }
	}
}
