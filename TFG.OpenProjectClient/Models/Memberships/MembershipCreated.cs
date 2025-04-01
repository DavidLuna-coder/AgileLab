using System.Text.Json.Serialization;

namespace TFG.OpenProjectClient.Models.Memberships
{
	public class MembershipCreated : HalResource<MembershipCreatedLinks, MembershipEmbeddedItems>
	{
		[JsonPropertyName("id")]
		public int Id { get; set; }
		[JsonPropertyName("createdAt")]
		public DateTime CreatedAt { get; set; }
		[JsonPropertyName("updatedAt")]
		public DateTime UpdatedAt { get; set; }
	}
}
