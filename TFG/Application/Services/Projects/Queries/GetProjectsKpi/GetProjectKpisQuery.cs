using MediatR;
using Shared.DTOs.Projects.Metrics;

namespace TFG.Application.Services.Projects.Queries.GetProjectsKpi
{
	public class GetProjectKpisQuery:IRequest<ProjectMetricsDto>
	{
		public Guid ProjectId { get; set; }
		public string? UserId { get; set; }
	}
}
