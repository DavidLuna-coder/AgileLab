using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using TFG.OpenProjectClient.Models.BasicObjects;

namespace TFG.OpenProjectClient.Models.Projects
{
	public class ProjectCreated : HalResource<CreatedProjectLinks>
	{
		/// <summary>
		/// Unique identifier for the project.
		/// </summary>
		[JsonPropertyName("id")]
		[Required]
		[Range(1, int.MaxValue)]
		public int Id { get; set; }

		/// <summary>
		/// Project identifier (short name).
		/// </summary>
		[JsonPropertyName("identifier")]
		public string Identifier { get; set; } = string.Empty;

		/// <summary>
		/// Project name.
		/// </summary>
		[JsonPropertyName("name")]
		public string Name { get; set; } = string.Empty;

		/// <summary>
		/// Indicates whether the project is currently active.
		/// </summary>
		[JsonPropertyName("active")]
		public bool Active { get; set; }

		/// <summary>
		/// Indicates whether the project is publicly accessible.
		/// </summary>
		[JsonPropertyName("public")]
		public bool Public { get; set; }

		/// <summary>
		/// Explanation for the project status.
		/// </summary>
		[JsonPropertyName("statusExplanation")]
		public FormattableText? StatusExplanation { get; set; }

		/// <summary>
		/// Project description (supports formatting).
		/// </summary>
		[JsonPropertyName("description")]
		public FormattableText? Description { get; set; }

		/// <summary>
		/// Project creation timestamp.
		/// </summary>
		[JsonPropertyName("createdAt")]
		public DateTime CreatedAt { get; set; }

		/// <summary>
		/// Timestamp of the last update to the project.
		/// </summary>
		[JsonPropertyName("updatedAt")]
		public DateTime UpdatedAt { get; set; }
	}

	public class CreatedProjectLinks
	{
		[JsonPropertyName("self")] 
		public Link Self { get; set; }
	}
}
