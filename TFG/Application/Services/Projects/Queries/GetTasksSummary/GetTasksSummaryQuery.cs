using MediatR;
using Shared.DTOs.Pagination;
using TFG.Application.Services.OpenProjectIntegration.Dtos;

namespace TFG.Application.Services.Projects.Queries.GetTasksSummary
{
	public class GetTasksSummaryQuery : IRequest<PaginatedResponseDto<TaskSummaryDto>>
	{
		public Guid ProjectId { get; set; }
		public FilteredPaginatedRequestDto<GetTaskSummaryQueryFilters> Request { get; set; } = new();
	}
}
