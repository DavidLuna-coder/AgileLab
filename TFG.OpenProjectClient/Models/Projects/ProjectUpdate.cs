using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using TFG.OpenProjectClient.Models.BasicObjects;

namespace TFG.OpenProjectClient.Models.Projects
{
	public class ProjectUpdate
	{
		[JsonPropertyName("id")]
		public int Id { get; set; }
		/// <summary>
		/// Identificador único del proyecto (slug).
		/// </summary>
		[JsonPropertyName("identifier")]
		[Required]
		public string Identifier { get; set; } = string.Empty;

		/// <summary>
		/// Nombre del proyecto.
		/// </summary>
		[JsonPropertyName("name")]
		[Required]
		public string Name { get; set; } = string.Empty;

		/// <summary>
		/// Indica si el proyecto está activo o archivado.
		/// </summary>
		[JsonPropertyName("active")]
		public bool Active { get; set; } = true;

		/// <summary>
		/// Indica si el proyecto es público.
		/// </summary>
		[JsonPropertyName("public")]
		public bool Public { get; set; } = false;

		/// <summary>
		/// Explicación del estado del proyecto.
		/// </summary>
		[JsonPropertyName("statusExplanation")]
		public FormattableText StatusExplanation { get; set; } = new();

		/// <summary>
		/// Descripción del proyecto.
		/// </summary>
		[JsonPropertyName("description")]
		public FormattableText? Description { get; set; }
	}
}
