namespace TFG.Application.Services.GitlabIntegration.Dtos
{
	public record GitlabCreateProjectDto
	{
		public required string Name { get; set; }
		public required int UserId { get; set; }
	}
}
