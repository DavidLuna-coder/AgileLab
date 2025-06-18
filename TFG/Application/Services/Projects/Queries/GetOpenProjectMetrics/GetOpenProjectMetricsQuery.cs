using MediatR;
using Shared.DTOs.Projects.Metrics;

namespace TFG.Application.Services.Projects.Queries.GetOpenProjectMetrics;

public class GetOpenProjectMetricsQuery : IRequest<OpenProjectMetricsDto>
{
	public Guid ProjectId { get; set; }
	public string? UserId { get; set; }
}
