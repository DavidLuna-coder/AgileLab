using System.Text.Json.Serialization;

namespace TFG.GoRaceClient.Dtos
{
	public record GoRaceResponseDto<T>
	{
		[JsonPropertyName("success")]
		public bool Success { get; set; }
		[JsonPropertyName("data")]
		public T Data { get; set; }
	}

	public record GoRaceDataResponseDto : GoRaceResponseDto<GoRaceActivityResultDto>
	{

	}
}
