using Shared.DTOs.Pagination;

namespace TFG.Application.Services.Projects.Queries.GetTasksSummary
{
	public class GetTasksSummaryQuery
	{
		public Guid ProjectId { get; set; }
		public FilteredPaginatedRequestDto<PaginatedRequestDtoBase> Request { get; set; } = new();
	}
}
