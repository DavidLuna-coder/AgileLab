using System;
using System.Collections.Generic;
using System.Linq;
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
			var now = DateTime.UtcNow;
			var projects = await context.Projects.Include(p => p.Users).ToListAsync(cancellationToken);
			foreach (var project in projects)
			{
				var kpis = await GetMetrics(project, cancellationToken);
				var gitlabMetrics = await GetGitlabMetrics(project, cancellationToken);
				var openProjectMetrics = await GetOpenProjectMetrics(project, cancellationToken);
				var bugs = kpis.Measures.Count(m => m.Metric == SonarMetricKeys.Bugs);
				var codeSmells = kpis.Measures.Count(m => m.Metric == SonarMetricKeys.CodeSmells);
				var vulnerabilities = kpis.Measures.Count(m => m.Metric == SonarMetricKeys.Vulnerabilities);
				var loc = kpis.Measures.Count(m => m.Metric == SonarMetricKeys.Ncloc);
				var snapshot = new ProjectStatusSnapshot
				{
					Id = Guid.NewGuid(),
					ProjectId = project.Id,
					SnapshotDate = now,
					Bugs = bugs,
					CodeSmells = codeSmells,
					Vulnerabilities = vulnerabilities,
					Loc = loc,
					ClosedTasks = openProjectMetrics.ClosedTasks,
					OpenTasks = openProjectMetrics.OpenTasks,
					OverdueTasks = openProjectMetrics.OverdueAssignedTasks,
					MergeRequestsCreated = gitlabMetrics.CreatedMergeRequests,
					MergeRequestsMerged = gitlabMetrics.MergedMergeRequests,
				};

				context.Set<ProjectStatusSnapshot>().Add(snapshot);

				// Guardar snapshots de todos los usuarios del proyecto
				foreach (var user in project.Users)
				{
					var userGitlabMetrics = await GetGitlabMetrics(project, cancellationToken, user.Id);
					var userOpenProjectMetrics = await GetOpenProjectMetrics(project, cancellationToken, user.Id);
					var userSnapshot = new UserProjectStatusSnapshot
					{
						Id = Guid.NewGuid(),
						ProjectId = project.Id,
						Project = project,
						UserId = user.Id,
						User = user,
						SnapshotDate = now,
						ClosedTasks = userOpenProjectMetrics.ClosedTasks,
						OpenTasks = userOpenProjectMetrics.OpenTasks,
						OverdueTasks = userOpenProjectMetrics.OverdueAssignedTasks,
						MergeRequestsCreated = userGitlabMetrics.CreatedMergeRequests,
						MergeRequestsMerged = userGitlabMetrics.MergedMergeRequests,
						Bugs = bugs,
						CodeSmells = codeSmells,
						Vulnerabilities = vulnerabilities,
						Loc = loc,
					};
					context.Set<UserProjectStatusSnapshot>().Add(userSnapshot);
				}
			}
			await context.SaveChangesAsync(cancellationToken);

			// Actualizar el estado de las experiencias de GoRace
			GoRaceDataSender dataSender = new(context);
			//dataSender.CalculateProjectExperienceData();
			await dataSender.CalculatePlatformExperienceData();
		}

		private Task<ProjectMetricsDto> GetMetrics(Project project, CancellationToken cancellationToken)
		{
			GetProjectKpisQuery query = new()
			{
				ProjectId = project.Id,
			};
			return new GetProjectKpisQueryHandler(sonarQubeClient, context).Handle(query, cancellationToken);
		}

		private Task<GitlabMetricsDto> GetGitlabMetrics(Project project, CancellationToken cancellationToken, string? userId = null)
		{
			GetGitlabMetricsQuery request = new()
			{
				ProjectId = project.Id,
				UserId = userId
			};
			return new GetGitlabMetricsQueryHandler(gitlabClient, context).Handle(request, cancellationToken);
		}

		private Task<OpenProjectMetricsDto> GetOpenProjectMetrics(Project project, CancellationToken cancellationToken, string? userId = null)
		{
			GetOpenProjectMetricsQuery query = new() { ProjectId = project.Id, UserId = userId };
			return new GetOpenProjectMetricsQueryHandler(openProjectClient, context).Handle(query, cancellationToken);
		}
	}
}
