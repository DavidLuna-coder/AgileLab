using System.Text.Json.Serialization;
using TFG.OpenProjectClient.Models.Projects;
using TFG.OpenProjectClient.Models.Roles;
using TFG.OpenProjectClient.Models.Users;

namespace TFG.OpenProjectClient.Models.Memberships
{
	public class MembershipEmbeddedItems
	{
		[JsonPropertyName("project")]
		Project Project { get; set; }
		[JsonPropertyName("principal")]
		Principal Principal { get; set; }
		[JsonPropertyName("roles")]
		Role[] Roles { get; set; }
	}
}
