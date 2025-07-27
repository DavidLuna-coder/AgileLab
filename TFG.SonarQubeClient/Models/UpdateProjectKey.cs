using System.Text.Json.Serialization;

namespace TFG.SonarQubeClient.Models
{
	public class UpdateProjectKey
	{
		[JsonPropertyName("from")]
		public required string From { get; set; }
		[JsonPropertyName("to")]
		public required string To { get; set; }
	}
}
