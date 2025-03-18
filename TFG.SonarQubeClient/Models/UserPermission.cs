using System.Text.Json.Serialization;

namespace TFG.SonarQubeClient.Models
{
    public class UserPermission
	{
		[JsonPropertyName("login")]
		public required string Login { get; set; }

		[JsonPropertyName("permission")]
		public required PermissionType Permission { get; set; }

		[JsonPropertyName("projectId")]
		public string? ProjectId { get; set; }

		[JsonPropertyName("projectKey")]
		public string? ProjectKey { get; set; }
	}
}
