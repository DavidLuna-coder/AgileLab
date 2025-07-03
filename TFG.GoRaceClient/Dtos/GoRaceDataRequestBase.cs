using System.Text.Json.Serialization;

namespace TFG.GoRaceClient.Dtos
{
	public record GoRaceDataRequestBase
	{
		[JsonPropertyName("assignment")]
		public string Assignment { get; set; }
		[JsonPropertyName("email")]
		public string Email { get; set; }
		[JsonPropertyName("time")]
		public DateTime Time { get; set; }
	}
}
