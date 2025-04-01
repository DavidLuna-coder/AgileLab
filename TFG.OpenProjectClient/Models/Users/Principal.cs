using System.Text.Json.Serialization;

namespace TFG.OpenProjectClient.Models.Users
{
	public class Principal : HalResource<Link>
	{
		[JsonPropertyName("id")]
		public int Id { get; set; }
		[JsonPropertyName("name")]
		public string Name { get; set; }
		public bool IsUser => Type == "User";
		public bool IsGroup => Type == "Group";
	}
	public class PrincipalLinks
	{
		[JsonPropertyName("self")]
		public Link Self { get; set; }
	}
}
