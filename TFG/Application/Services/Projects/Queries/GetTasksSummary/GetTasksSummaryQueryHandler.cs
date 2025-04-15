using Azure.Core;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Shared.DTOs.Pagination;
using System.Data.Entity;
using TFG.Api.Exeptions;
using TFG.Application.Services.OpenProjectIntegration.Dtos;
using TFG.Infrastructure.Data;
using TFG.Model.Entities;
using TFG.OpenProjectClient;
using TFG.OpenProjectClient.Models.BasicObjects;
using TFG.OpenProjectClient.Models.WorkPackages;

namespace TFG.Application.Services.Projects.Queries.GetTasksSummary
{
	public class GetTasksSummaryQueryHandler(ApplicationDbContext dbContext, IOpenProjectClient openProjectClient, UserManager<User> userManager) : IRequestHandler<GetTasksSummaryQuery, PaginatedResponseDto<TaskSummaryDto>>
	{
		public async Task<PaginatedResponseDto<TaskSummaryDto>> Handle(GetTasksSummaryQuery request, CancellationToken cancellationToken)
		{
			Project project = await dbContext.Projects
							  .FirstOrDefaultAsync(p => p.Id == request.ProjectId) ?? throw new NotFoundException("Project not found");

			var opFilters = await GetOpenProjectFilters(request);
			GetWorkPackagesQuery query = new()
			{
				PageSize = request.Request.PageSize,
				Offset = request.Request.Page * request.Request.PageSize,
				Filters = opFilters
			};
			var workPackagesCollection = await openProjectClient.WorkPackages.GetAsync(project.OpenProjectId, query);
			List<Task<OpenProjectClient.Models.OpenProjectCollection<GitlabMergeRequest>>> tasks = new();
			foreach (var workPackage in workPackagesCollection.Embedded.Elements)
			{
				var getMergeRequest = openProjectClient.WorkPackages.GetGitlabMergeRequestsAsync(workPackage.Id);
				tasks.Add(getMergeRequest);
			}
			OpenProjectClient.Models.OpenProjectCollection<GitlabMergeRequest>[] mergeRequests;
			mergeRequests = await GetMergeRequests(tasks);

			var results = workPackagesCollection.Embedded.Elements.Zip(mergeRequests, (workPackage, mergeRequestCollection) =>
			{
				var (ids, titles) = ExtractIdsAndTitles(mergeRequestCollection.Embedded.Elements);

				return new TaskSummaryDto
				{
					MergeRequestsIds = ids,
					MergeRequestsTitles = titles,
					Name = workPackage.Subject,
					OpenProjectTaskId = workPackage.Id,
				};
			}
			).ToArray();

			PaginatedResponseDto<TaskSummaryDto> paginatedResponse = new()
			{
				Items = results,
				PageNumber = request.Request.Page,
				PageSize = request.Request.PageSize,
				TotalCount = workPackagesCollection.Total
			};

			return paginatedResponse;
		}

		private static async Task<OpenProjectClient.Models.OpenProjectCollection<GitlabMergeRequest>[]> GetMergeRequests(List<Task<OpenProjectClient.Models.OpenProjectCollection<GitlabMergeRequest>>> tasks)
		{
			try
			{
				return await Task.WhenAll(tasks);
			}
			catch
			{
				return [];
			}
		}

		private static (int[] ids, string[] titles) ExtractIdsAndTitles(IEnumerable<GitlabMergeRequest> mergeRequests)
		{
			var ids = new List<int>();
			var titles = new List<string>();

			foreach (var mr in mergeRequests)
			{
				ids.Add(mr.Id);
				titles.Add(mr.Title);
			}

			return (ids.ToArray(), titles.ToArray());
		}

		private async Task<string[]?> GetOpenProjectUserIds(GetTasksSummaryQuery query)
		{
			if (query.Request.Filters?.UserIds == null) return null;

			return await userManager.Users.Where(u => query.Request.Filters.UserIds.Contains(u.Id)).Select(u => u.OpenProjectId).ToArrayAsync();
		}

		private async Task<OpenProjectFilters[]?> GetOpenProjectFilters(GetTasksSummaryQuery query)
		{
			string[]? openProjectUserIds = await GetOpenProjectUserIds(query);

			List<OpenProjectFilters> filters = new();

			if (openProjectUserIds != null && openProjectUserIds.Length != 0)
			{
				var filter = new OpenProjectFilters() { Name = "user", Operator = "=", Values = openProjectUserIds };
				filters.Add(filter);
			}

			return filters.Count != 0 ? filters.ToArray() : null;

		}
	}
}
