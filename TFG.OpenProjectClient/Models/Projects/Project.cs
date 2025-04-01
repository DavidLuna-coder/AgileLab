using System.Text.Json.Serialization;

namespace TFG.OpenProjectClient.Models.Projects
{
	public class Project : HalResource<ProjectLinksProperties>
	{
		[JsonPropertyName("id")]
		public int Id { get; set; }
		[JsonPropertyName("identifier")]
		public string Identifier { get; set; }
		[JsonPropertyName("name")]
		public string Name { get; set; }
		[JsonPropertyName("active")]
		public bool Active { get; set; }
		[JsonPropertyName("statusExplanation")]
		public FormattableString StatusExplanation { get; set; }
		[JsonPropertyName("public")]
		public bool Public { get; set; }

		[JsonPropertyName("description")]
		public string Description { get; set; }
		[JsonPropertyName("createdAt")]
		public string CreatedAt { get; set; }
		[JsonPropertyName("updatedAt")]
		public string UpdatedAt { get; set; }
	}

	public class ProjectLinksProperties
	{
		[JsonPropertyName("self")]
		public Link Self { get; set; }
	}
}
