using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NGitLab;
using Shared.Constants;
using Shared.DTOs.Projects.Metrics;
using TFG.Application.Services.Projects.Queries.GetGitlabMetrics;
using TFG.Application.Services.Projects.Queries.GetOpenProjectMetrics;
using TFG.Application.Services.Projects.Queries.GetProjectsKpi;
using TFG.Application.Services.Projects.Queries.GetTasksSummary;
using TFG.Domain.Entities;
using TFG.Infrastructure.Data;
using TFG.OpenProjectClient;
using TFG.SonarQubeClient;

namespace TFG.Application.Services.Experiences.Commands
{
    /// <summary>
    /// Servicio encargado de registrar snapshots de estado de proyectos y usuarios.
    /// </summary>
    public class ProjectSnapshotService(ApplicationDbContext context, ISonarQubeClient sonarQubeClient, IOpenProjectClient openProjectClient, IGitLabClient gitlabClient, UserManager<User> userManager)
	{
		public async Task RegisterSnapshotsAsync(CancellationToken cancellationToken = default)
        {
            // TODO: Implementar la lógica real de snapshot
            // Ejemplo: Recorrer proyectos y usuarios, calcular métricas y guardar snapshots
            var now = DateTime.UtcNow;
            var projects = await context.Projects.Include(p => p.Users).ToListAsync(cancellationToken);
            foreach (var project in projects)
			{
				var kpis = await GetMetrics(project, cancellationToken);
				var gitlabMetrics = await GetGitlabMetrics(project, cancellationToken);
				var openProjectMetrics = await GetOpenProjectMetrics(project, cancellationToken);
				var snapshot = new ProjectStatusSnapshot
				{
					Id = Guid.NewGuid(),
					ProjectId = project.Id,
					SnapshotDate = now,
					Bugs = kpis.Measures.Count(m => m.Metric == SonarMetricKeys.Bugs),
					CodeSmells = kpis.Measures.Count(m => m.Metric == SonarMetricKeys.CodeSmells),
					Vulnerabilities = kpis.Measures.Count(m => m.Metric == SonarMetricKeys.Vulnerabilities),
					Loc = kpis.Measures.Count(m => m.Metric == SonarMetricKeys.Ncloc),
					ClosedTasks = openProjectMetrics.ClosedTasks,
					OpenTasks = openProjectMetrics.OpenTasks,
					OverdueTasks = openProjectMetrics.OverdueAssignedTasks,
					MergeRequestsCreated = gitlabMetrics.CreatedMergeRequests,
					MergeRequestsMerged = gitlabMetrics.MergedMergeRequests,
				};


				context.Set<ProjectStatusSnapshot>().Add(snapshot);

				// Ejemplo para usuarios del proyecto
				foreach (var user in project.Users)
				{
					var userSnapshot = new UserProjectStatusSnapshot
					{
						Id = Guid.NewGuid(),
						ProjectId = project.Id,
						Project = project,
						UserId = user.Id,
						User = user,
						SnapshotDate = now,
						// Rellenar métricas reales aquí
					};
					context.Set<UserProjectStatusSnapshot>().Add(userSnapshot);
				}
			}
			await context.SaveChangesAsync(cancellationToken);
        }

		private Task<ProjectMetricsDto> GetMetrics(Project project, CancellationToken cancellationToken)
		{
			GetProjectKpisQuery query = new()
			{
				ProjectId = project.Id,
			};
			return new GetProjectKpisQueryHandler(sonarQubeClient, context).Handle(query, cancellationToken);
		}

		private Task<GitlabMetricsDto> GetGitlabMetrics(Project project, CancellationToken cancellationToken)
		{
			GetGitlabMetricsQuery request = new()
			{
				ProjectId = project.Id
			};
			return new GetGitlabMetricsQueryHandler(gitlabClient, context).Handle(request, cancellationToken);
		}

		private Task<OpenProjectMetricsDto> GetOpenProjectMetrics(Project project, CancellationToken cancellationToken)
		{
			GetOpenProjectMetricsQuery query = new() { ProjectId = project.Id };
			return new GetOpenProjectMetricsQueryHandler(openProjectClient, context).Handle(query, cancellationToken);
		}
	}
}
