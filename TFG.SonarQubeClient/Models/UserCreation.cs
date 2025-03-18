using System.Text.Json.Serialization;

namespace TFG.SonarQubeClient.Models
{
    public class UserCreation
    {
        [JsonPropertyName("email")]
        public string? Email { get; set; }
		[JsonPropertyName("local")]
		public bool? Local { get; set; }
        [JsonPropertyName("login")]
		public required string Login { get; set; }
		[JsonPropertyName("name")]
		public required string Name { get; set; }
		[JsonPropertyName("password")]
		public string? Password { get; set; }

		[JsonPropertyName("scmAccounts")]
		public string[]? ScmAccounts { get; set; }
    }
}
