using MediatR;
using Shared.DTOs.Projects.Metrics;
using TFG.Api.Mappers;
using TFG.SonarQubeClient;
using TFG.SonarQubeClient.Models;
using TFG.SonarQubeClient.Models.Metrics;

namespace TFG.Application.Services.Projects.Queries.GetMosAffectedFiles
{
	public class GetMostAffectedFilesQueryHandler(ISonarQubeClient sonarQubeClient) : IRequestHandler<GetMostAffectedFilesQuery, List<AffectedFileDto>>
	{
		const double wSmells = 0.2;
		const double wBugs = 0.3;
		const double wVulns = 0.5;
		public async Task<List<AffectedFileDto>> Handle(GetMostAffectedFilesQuery request, CancellationToken cancellationToken)
		{
			int page = 1;
			int pageSize = 100;
			List<string> componentsKeys = [];
			GetComponentsRequest componentsRequest = new() { ComponentKey = request.ProjectId.ToString(), Page = page, PageSize = pageSize, Qualifiers = ["FIL"] };
			int totalPages = 0;
			int totalItems = 0;
			do
			{
				// Get the components tree from SonarQube
				SonarComponentsTree componentsTree = await sonarQubeClient.Projects.GetComponentsTree(componentsRequest);
				totalItems = totalItems == 0 ? componentsTree.Paging.Total : totalItems;
				totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
				componentsKeys.AddRange(componentsTree.Components.Select(c => c.Key));
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
		IEnumerable<string> componentKeys,
		int maxDegree = 8,
		CancellationToken ct = default)
		{
			using var throttler = new SemaphoreSlim(maxDegree);

			var tasks = componentKeys
				.Select(async (key, idx) =>
				{
					await throttler.WaitAsync(ct);
					try
					{
						var response = await sonarQubeClient.Projects.GetMetrics(new SonarMetricsRequest
						{
							ProjectKey = key,
							MetricKeys = [
								SonarMetricKeys.CodeSmells,
							SonarMetricKeys.Bugs,
							SonarMetricKeys.Vulnerabilities, SonarMetricKeys.Ncloc ]
						});

						return (idx, new AffectedFileDto
						{
							Name = key,
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
