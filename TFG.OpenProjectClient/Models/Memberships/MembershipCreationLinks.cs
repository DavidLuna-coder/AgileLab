using System.Text.Json.Serialization;

namespace TFG.OpenProjectClient.Models.Memberships
{
	public class MembershipCreationLinks
	{
		[JsonPropertyName("principal")]
		public Link Principal { get; set; }
		[JsonPropertyName("roles")]
		public Link[] Roles { get; set; }
		[JsonPropertyName("project")]
		public Link Project { get; set; }
	}
}
