using System.Text.Json.Serialization;

namespace TFG.OpenProjectClient.Models.Memberships
{
	public class MembershipCreationLinks
	{
		[JsonPropertyName("principal")]
		Link Principal { get; set; }
		[JsonPropertyName("roles")]
		Link[] Roles { get; set; }
		[JsonPropertyName("project")]
		Link Project { get; set; }
	}
}
