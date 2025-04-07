using System.Text.Json.Serialization;

namespace TFG.OpenProjectClient.Models.Memberships
{
	public class MembershipCreation
	{
		[JsonPropertyName("_links")]
		public MembershipCreationLinks Links { get; set; }
	}
}
