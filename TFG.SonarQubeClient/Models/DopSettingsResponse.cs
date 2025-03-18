using System.Text.Json.Serialization;

namespace TFG.SonarQubeClient.Models
{
	public class DopSettingsResponse
	{
		[JsonPropertyName("dopSettings")]
		public List<DopSetting> DopSettings { get; set; } = new();

		[JsonPropertyName("page")]
		public PageInfo Page { get; set; } = new();
	}
}
