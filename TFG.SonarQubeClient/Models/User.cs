using System.Text.Json.Serialization;

namespace TFG.SonarQubeClient.Models
{
	public class User
	{
		[JsonPropertyName("id")]
		public string Id { get; set; }

		[JsonPropertyName("login")]
		public string Login { get; set; }

		[JsonPropertyName("name")]
		public string Name { get; set; }

		[JsonPropertyName("email")]
		public string Email { get; set; }

		[JsonPropertyName("active")]
		public bool Active { get; set; }

		[JsonPropertyName("local")]
		public bool Local { get; set; }

		[JsonPropertyName("managed")]
		public bool Managed { get; set; }

		[JsonPropertyName("externalLogin")]
		public string ExternalLogin { get; set; }

		[JsonPropertyName("externalProvider")]
		public string ExternalProvider { get; set; }

		[JsonPropertyName("externalId")]
		public string ExternalId { get; set; }

		[JsonPropertyName("avatar")]
		public string Avatar { get; set; }

		[JsonPropertyName("sonarQubeLastConnectionDate")]
		public string SonarQubeLastConnectionDate { get; set; }

		[JsonPropertyName("sonarLintLastConnectionDate")]
		public string SonarLintLastConnectionDate { get; set; }

		[JsonPropertyName("scmAccounts")]
		public string[] ScmAccounts { get; set; } = [];
	}
}
