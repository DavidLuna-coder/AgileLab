using System.Text.Json.Serialization;

namespace TFG.OpenProjectClient.Models.Statuses
{
	public class Status : HalResource<StatusLinkedProperties>
	{
		[JsonPropertyName("id")]
		public int Id { get; set; }
		[JsonPropertyName("name")]
		public string Name { get; set; }
		[JsonPropertyName("isClosed")]
		public bool IsClosed { get; set; }
		[JsonPropertyName("color")]
		public string Color { get; set; }
		[JsonPropertyName("isDefault")]
		public bool IsDefault { get; set; }
		[JsonPropertyName("isReadonly")]
		public bool IsReadonly { get; set; }
		[JsonPropertyName("excludedFromTotals")]
		public bool ExcludedFromTotals { get; set; }
		[JsonPropertyName("defaultDoneRatio")]
		public int? DefaultDoneRatio { get; set; }
		[JsonPropertyName("position")]
		public int Position { get; set; }
	}
}
