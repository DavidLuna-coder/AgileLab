using System.Text.Json.Serialization;

namespace TFG.OpenProjectClient.Models.Memberships
{
	public class MembershipCreatedLinks
	{
		[JsonPropertyName("self")]
		public Link Self { get; set; }
		[JsonPropertyName("project")]
		public Link Project { get; set; }
		[JsonPropertyName("principal")]
		public Link Principal { get; set; }
		[JsonPropertyName("roles")]
		public Link[] Roles { get; set; }
	}
}
