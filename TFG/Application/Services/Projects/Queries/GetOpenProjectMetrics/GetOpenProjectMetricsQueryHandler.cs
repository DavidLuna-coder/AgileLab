using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.DTOs.Projects.Metrics;
using TFG.Api.Exeptions;
using TFG.Domain.Entities;
using TFG.Infrastructure.Data;
using TFG.OpenProjectClient;
using TFG.OpenProjectClient.Models.BasicObjects;
using TFG.OpenProjectClient.Models.WorkPackages;

namespace TFG.Application.Services.Projects.Queries.GetOpenProjectMetrics;

public class GetOpenProjectMetricsQueryHandler(IOpenProjectClient openProjectClient, ApplicationDbContext context) : IRequestHandler<GetOpenProjectMetricsQuery, OpenProjectMetricsDto>
{
	public async Task<OpenProjectMetricsDto> Handle(GetOpenProjectMetricsQuery request, CancellationToken cancellationToken)
	{
		Project project = await context.Projects.FirstOrDefaultAsync(p => p.Id == request.ProjectId) ?? throw new NotFoundException("El proyecto no existe");

		User? user = await context.Users.FirstOrDefaultAsync(u => u.Id == request.UserId);

		int openProjectProjectId = project.OpenProjectId;
		IWorkPackagesClient workPackagesClient = openProjectClient.WorkPackages;

		// Obtener todos los estados para saber cuáles están cerrados
		var statusesCollection = await openProjectClient.Statuses.GetStatusesAsync();
		var closedStatusIds = statusesCollection.Embedded.Elements.Where(s => s.IsClosed).Select(s => s.Id.ToString()).ToHashSet();

		WorkPackage[] assignedTasks = await GetAssignedTasks(openProjectProjectId, workPackagesClient, user?.OpenProjectId);
		WorkPackage[] createdTasks = await GetCreatedTasks(openProjectProjectId, workPackagesClient, user?.OpenProjectId);
		WorkPackage[] closedTasks = await GetClosedTasks(openProjectProjectId, workPackagesClient, user?.OpenProjectId);

		// Calcular tareas asignadas vencidas
		int overdueAssignedTasks = assignedTasks.Count(wp =>
			wp.DueDate.HasValue && wp.DueDate.Value < DateTime.UtcNow &&
			wp.Links.Status != null &&
			int.TryParse(wp.Links.Status.Href?.Split('/').LastOrDefault(), out var statusId) &&
			!closedStatusIds.Contains(statusId.ToString())
		);

		return new OpenProjectMetricsDto
		{
			TotalTasks = assignedTasks.Length + createdTasks.Length,
			OpenTasks = assignedTasks.Length,
			ClosedTasks = closedTasks.Length,
			AssignedTasks = assignedTasks.Length,
			CreatedTasks = createdTasks.Length,
			OverdueAssignedTasks = overdueAssignedTasks
		};
	}

	private static async Task<WorkPackage[]> GetAssignedTasks(int openProjectProjectId, IWorkPackagesClient workPackagesClient, string? userId)
	{
		GetWorkPackagesQuery query = new();
		
		if(!string.IsNullOrEmpty(userId))
		{
			query.Filters = [new() { Name = "assignee", Operator = "=", Values = [userId] }];
		}
		var assignedTasks = await workPackagesClient.GetAsync(openProjectProjectId, query);
		return assignedTasks?.Embedded?.Elements ?? [];
	}

	private static async Task<WorkPackage[]> GetCreatedTasks(int openProjectProjectId, IWorkPackagesClient workPackagesClient, string? userId)
	{
		GetWorkPackagesQuery query = new();

		if (!string.IsNullOrEmpty(userId))
		{
			query.Filters = [new() { Name = "author", Operator = "=", Values = [userId] }];
		}
		var assignedTasks = await workPackagesClient.GetAsync(openProjectProjectId, query);
		return assignedTasks?.Embedded?.Elements ?? [];
	}

	private static async Task<WorkPackage[]> GetClosedTasks(int openProjectProjectId, IWorkPackagesClient workPackagesClient, string? userId)
	{
		GetWorkPackagesQuery query = new();
		List<OpenProjectFilters> filters = [new() { Name = "status", Operator = "operator", Values = ["5"] }]; // Assuming "5" is the ID for closed tasks in OpenProject
		if (!string.IsNullOrEmpty(userId))
		{
			OpenProjectFilters filter = new() { Name = "author", Operator = "=", Values = [userId] };
			filters.Add(filter);
		}
		var assignedTasks = await workPackagesClient.GetAsync(openProjectProjectId, query);
		return assignedTasks?.Embedded?.Elements ?? [];
	}
}
