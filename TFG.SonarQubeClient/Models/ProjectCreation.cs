using System.Text.Json.Serialization;

namespace TFG.SonarQubeClient.Models
{
    public class ProjectCreation
    {
		[JsonPropertyName("mainBranch")]
		public string? MainBranch { get; set; }

		[JsonPropertyName("name")]
		public required string Name { get; set; }

		[JsonPropertyName("newCodeDefinitionType")]
		public string? NewCodeDefinitionType { get; set; }

		[JsonPropertyName("newCodeDefinitionValue")]
		public int? NewCodeDefinitionValue { get; set; }

		[JsonPropertyName("project")]
		public required string ProjectKey { get; set; }

		[JsonPropertyName("visibility")]
		public ProjectVisibility Visibility { get; set; }
	}
}
