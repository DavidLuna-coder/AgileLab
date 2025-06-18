namespace TFG.Application.Services.Projects.Queries.GetOpenProjectMetrics;

public class GetOpenProjectMetricsDto
{
	public Guid ProjectId { get; set; }
	public string? UserId { get; set; }
}
