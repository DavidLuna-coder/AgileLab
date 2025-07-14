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

	public record GoRaceQualityRequest : GoRaceDataRequestBase
	{
		[JsonPropertyName("Q")]
		public int Quality { get; set; }
	}
	public record GoRaceImprovementRequest : GoRaceDataRequestBase
	{
		[JsonPropertyName("IMP")]
		public int Improvement { get; set; }
	}
	public record GoRaceClosedTasksRequest : GoRaceDataRequestBase
	{
		[JsonPropertyName("CT")]
		public int ClosedTasks { get; set; }
	}

	public record GoRaceOnTimeTasksRequest : GoRaceDataRequestBase
	{
		[JsonPropertyName("OTT")]
		public int OnTimeTask { get; set; }
	}

	public record GoRaceMergedMergeRequest : GoRaceDataRequestBase
	{
		[JsonPropertyName("MMR")]
		public int MergedMergeRequest { get; set; }
	}
}
