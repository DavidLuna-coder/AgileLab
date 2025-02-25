namespace TFG.Application.Services.GitlabIntegration.Dtos
{
	public record GitlabCreateProjectDto
	{
		public required string Name { get; set; }
		public required int UserId { get; set; }
		public string? Visibility { get; set; }
		public string? TemplateName { get; set; }
	}
}
