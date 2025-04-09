using System.Text.Json.Serialization;

namespace TFG.OpenProjectClient.Models.WorkPackages
{
	public class GitlabUser:HalResource<OpenProjectSelfLink>
	{
		[JsonPropertyName("login")]
		public string Login { get; set; }
		[JsonPropertyName("email")]
		public string Email { get; set; }
		[JsonPropertyName("avatarUrl")]
		public string AvatarUrl { get; set; }
	}
}
