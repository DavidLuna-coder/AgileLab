using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.DTOs.Projects.Metrics;
using TFG.Application.Services.Projects.Queries.GetMosAffectedFiles;

namespace TFG.Application.Services.Projects.Queries.GetGitlabMetrics;

public class GetGitlabMetricsQuery : IRequest<GitlabMetricsDto>
{
	public string? UserId { get; set; }
	public Guid ProjectId { get; set; }
}
