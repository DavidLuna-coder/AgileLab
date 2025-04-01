using System.Text.Json.Serialization;

namespace TFG.OpenProjectClient.Models.Roles
{
	public class Role : HalResource<RoleLinks>
	{
		[JsonPropertyName("id")]
		public int Id { get; set; }
		[JsonPropertyName("name")]
		public string Name { get; set; }
	}

	public class RoleLinks
	{
		public Link Self { get; set; }
	}
}
