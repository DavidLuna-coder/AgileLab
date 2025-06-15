using MediatR;
using Shared.DTOs.Projects.Metrics;

namespace TFG.Application.Services.Projects.Queries.GetGitlabMetrics;

public class GetGitlabMetricsQuery : IRequest<GitlabMetricsDto>
{
	public string? UserId { get; set; }
	public Guid ProjectId { get; set; }
}
