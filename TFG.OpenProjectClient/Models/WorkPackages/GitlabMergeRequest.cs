using System.Text.Json.Serialization;
using TFG.OpenProjectClient.Models.BasicObjects;

namespace TFG.OpenProjectClient.Models.WorkPackages
{
	public class GitlabMergeRequest : HalResource<GitlabMergeRequestLinks>
	{
		[JsonPropertyName("id")]
		public int Id { get; set; }
		[JsonPropertyName("number")]
		public int Number { get; set; }
		[JsonPropertyName("htmlUrl")]
		public string HtmlUrl { get; set; }
		[JsonPropertyName("state")]
		public string State { get; set; }
		[JsonPropertyName("repository")]
		public string Repository { get; set; }
		[JsonPropertyName("gitlabUppdatedAt")]
		public DateTime GitlabUpdatedAt { get; set; }
		[JsonPropertyName("title")]
		public string Title { get; set; }
		[JsonPropertyName("body")]
		public FormattableText Body { get; set; }
		[JsonPropertyName("draft")]
		public bool Draft { get; set; }
		[JsonPropertyName("merged")]
		public bool Merged { get; set; }
		[JsonPropertyName("labels")]
		public string[] Labels { get; set; }
		[JsonPropertyName("createdAt")]
		public DateTime CreatedAt { get; set; }
		[JsonPropertyName("updatedAt")]
		public DateTime UpdatedAt { get; set; }
	}
}
