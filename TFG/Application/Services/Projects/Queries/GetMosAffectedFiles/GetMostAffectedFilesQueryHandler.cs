using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.DTOs.Projects.Metrics;
using TFG.Api.Exeptions;
using TFG.Api.Mappers;
using TFG.Infrastructure.Data;
using TFG.Model.Entities;
using TFG.SonarQubeClient;
using TFG.SonarQubeClient.Models;
using TFG.SonarQubeClient.Models.Metrics;

namespace TFG.Application.Services.Projects.Queries.GetMosAffectedFiles
{
	public class GetMostAffectedFilesQueryHandler(ISonarQubeClient sonarQubeClient, ApplicationDbContext dbContext) : IRequestHandler<GetMostAffectedFilesQuery, List<AffectedFileDto>>
	{
		const double wSmells = 0.2;
		const double wBugs = 0.3;
		const double wVulns = 0.5;
		public async Task<List<AffectedFileDto>> Handle(GetMostAffectedFilesQuery request, CancellationToken cancellationToken)
		{
			int page = 1;
			int pageSize = 100;
			List<SonarQubeClient.Models.SonarComponent> componentsKeys = [];
			Project project = await dbContext.Projects.FirstOrDefaultAsync(p => p.Id == request.ProjectId) ?? throw new NotFoundException("Project not found");
			GetComponentsRequest componentsRequest = new() { ComponentKey = project.SonarQubeProjectKey, Page = page, PageSize = pageSize, Qualifiers = ["FIL"] };
			int totalPages = 0;
			int totalItems = 0;
			do
			{
				// Get the components tree from SonarQube
				SonarComponentsTree componentsTree = await sonarQubeClient.Projects.GetComponentsTree(componentsRequest);
				totalItems = totalItems == 0 ? componentsTree.Paging.Total : totalItems;
				totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
				componentsKeys.AddRange(componentsTree.Components.ToList());
				page++;
				componentsRequest.Page = page;
			} while (componentsRequest.Page <= totalPages);

			List<AffectedFileDto> affectedFiles = await GetFileMetricsAsync(componentsKeys, 8, cancellationToken);
			List<AffectedFileDto> mostAffectedFiles = affectedFiles
			.Select(f =>
			{
				int smells = ParseValue(f.Measures.FirstOrDefault(m => m.Metric == SonarMetricKeys.CodeSmells)?.Value);
				int bugs = ParseValue(f.Measures.FirstOrDefault(m => m.Metric == SonarMetricKeys.Bugs)?.Value);
				int vulns = ParseValue(f.Measures.FirstOrDefault(m => m.Metric == SonarMetricKeys.Vulnerabilities)?.Value);

				double score = wSmells * smells + wBugs * bugs + wVulns * vulns;
				return (File: f, Score: score);
			})
			.OrderByDescending(x => x.Score)
			.Take(5)
			.Select(x => x.File)
			.ToList();

			return mostAffectedFiles;
		}
		static int ParseValue(string? val) => int.TryParse(val, out var n) ? n : 0;

		public async Task<List<AffectedFileDto>> GetFileMetricsAsync(
		IEnumerable<SonarQubeClient.Models.SonarComponent> componentKeys,
		int maxDegree = 8,
		CancellationToken ct = default)
		{
			using var throttler = new SemaphoreSlim(maxDegree);

			var tasks = componentKeys
				.Select(async (component, idx) =>
				{
					await throttler.WaitAsync(ct);
					try
					{
						var response = await sonarQubeClient.Projects.GetMetrics(new SonarMetricsRequest
						{
							ProjectKey = component.Key,
							MetricKeys = [
								SonarMetricKeys.CodeSmells,
							SonarMetricKeys.Bugs,
							SonarMetricKeys.Vulnerabilities, SonarMetricKeys.Ncloc ]
						});

						return (idx, new AffectedFileDto
						{
							Name = component.Name,
							Key = component.Key,
							Path = component.Path,
							Measures = response.ToProjectMetricsDto().Measures
						});
					}
					finally
					{
						throttler.Release();
					}
				});

			var ordered = await Task.WhenAll(tasks);
			return ordered.OrderBy(t => t.idx).Select(t => t.Item2).ToList();
		}
	}
}
