using System.Text.Json.Serialization;

namespace TFG.OpenProjectClient.Models.WorkPackages
{
	public class GitlabMergeRequestLinks
	{
		[JsonPropertyName("gitlabUser")]
		public Link GitlabUser { get; set; }
		[JsonPropertyName("mergedBy")]
		public Link MergedBy { get; set; }
	}
}
