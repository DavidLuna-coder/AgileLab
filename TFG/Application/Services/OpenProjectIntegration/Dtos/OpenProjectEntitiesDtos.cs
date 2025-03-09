using System.Text.Json.Serialization;

namespace TFG.Application.Services.OpenProjectIntegration.Dtos
{
	public record OpenProjectRoleDto
	{
		public int Id { get; set; } = 4;
		public string Name { get; set; } = "Member";
	};

	public record OpenProjectLink
	{
		public string Href { get; set; }
	}
	public record OpenProjectProjectDto
	{
		public int Id { get; set; }
		public string Identifier { get; set; }
		public string Name { get; set; }
		public bool Active { get; set; }
		public bool Public { get; set; }
	}
	public record OpenProjectPrincipalDto
	{
		public int Id { get; set; }
		public string Identifier { get; set; }
		public string Name { get; set; }
		public bool Active { get; set; }
		public bool Public { get; set; }
	}
	public record OpenProjectFormattableDto
	{
		public string Format { get; set; } = "plain";
		public string Raw { get; set; }
	}

	public class OpenProjectWorkPackage
	{
		public int Id { get; set; }
		[JsonPropertyName("subject")]
		public string Name { get; set; }
		public int PercentageDone { get; set; }
	}
	public record EmbeddedItems<T>
	{
		[JsonPropertyName("elements")]
		public T[] Elements { get; set; } = [];
	}
	public record GetWorkPackageResponse
	{
		[JsonPropertyName("_embedded")]
		public EmbeddedItems<OpenProjectWorkPackage> Embedded { get; set; } = new();
	}
}
