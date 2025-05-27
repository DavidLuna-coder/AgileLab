using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.DTOs.Projects.Metrics;
using TFG.Api.Exeptions;
using TFG.Api.Mappers;
using TFG.Domain.Entities;
using TFG.Infrastructure.Data;
using TFG.SonarQubeClient;
using TFG.SonarQubeClient.Models.Metrics;

namespace TFG.Application.Services.Projects.Queries.GetProjectsKpi
{
	public class GetProjectKpisQueryHandler(ISonarQubeClient sonarQubeClient, ApplicationDbContext context) : IRequestHandler<GetProjectKpisQuery, ProjectMetricsDto>
	{
		public async Task<ProjectMetricsDto> Handle(GetProjectKpisQuery request, CancellationToken cancellationToken)
		{
			Project project = await context.Projects.FirstOrDefaultAsync(p => p.Id == request.ProjectId)
				?? throw new NotFoundException("Project not found");

			if (project.SonarQubeProjectKey == null)
				throw new NotFoundException("SonarQube project key not found");

			SonarMetricsRequest metricRequest = new()
			{
				ProjectKey = project.SonarQubeProjectKey,
				MetricKeys =
				[
					SonarMetricKeys.CodeSmells,
					SonarMetricKeys.Bugs,
					SonarMetricKeys.Vulnerabilities,
					SonarMetricKeys.CognitiveComplexity,
					SonarMetricKeys.SqaleRating,
					SonarMetricKeys.Ncloc,
					SonarMetricKeys.Coverage,
					SonarMetricKeys.DuplicatedLinesDensity,
				]
			};

			var response = await sonarQubeClient.Projects.GetMetrics(metricRequest);

			return response.ToProjectMetricsDto();
		}
	}
}
