using System.Text.Json.Serialization;

namespace TFG.OpenProjectClient.Models.WorkPackages
{
	public class GitlabMergeRequestEmbedded
	{
		[JsonPropertyName("gitlabUser")]
		public GitlabUser GitlabUser { get; set; }
		[JsonPropertyName("mergedBy")]
		public GitlabUser MergedBy { get; set; }
	}
}
