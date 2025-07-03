using System.Text.Json.Serialization;

namespace TFG.GoRaceClient.Dtos
{
	public record GoRaceActivityResultDto
	{
		[JsonPropertyName("result")]
		public bool Result { get; set; }
		[JsonPropertyName("gCode")]
		public int GCode { get; set; }
	}
}
