using System.Text.Json.Serialization;

namespace TFG.SonarQubeClient.Models
{
	public class ProjectBinding
	{
		[JsonPropertyName("projectKey")]
		public string ProjectKey { get; set; }
		[JsonPropertyName("projectName")]
		public string ProjectName { get; set; }

		[JsonPropertyName("devOpsPlatformSettingId")]
		public string DevOpsPlatformSettingId { get; set; }

		[JsonPropertyName("repositoryIdentifier")]
		public string RepositoryIdentifier { get; set; }

		[JsonPropertyName("projectIdentifier")]
		public string ProjectIdentifier { get; set; }

		[JsonPropertyName("newCodeDefinitionType")]
		public string NewCodeDefinitionType { get; set; }

		[JsonPropertyName("newCodeDefinitionValue")]
		public string NewCodeDefinitionValue { get; set; }

		[JsonPropertyName("monorepo")]
		public bool Monorepo { get; set; }
	}
}
