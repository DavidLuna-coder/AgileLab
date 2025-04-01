using System.Text.Json.Serialization;

namespace TFG.OpenProjectClient.Models.Memberships
{
	public class MembershipCreation
	{
		[JsonPropertyName("_links")]
		MembershipCreationLinks Links { get; set; }
	}
}
