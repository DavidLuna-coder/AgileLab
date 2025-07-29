using MediatR;
using Shared.DTOs.Pagination;
using Shared.DTOs.Projects;

namespace TFG.Application.Services.Projects.Queries.SearchProjects
{
    public class SearchProjectsQuery : IRequest<PaginatedResponseDto<FilteredProjectDto>>
    {
        public FilteredPaginatedRequestDto<ProjectQueryParameters> Request { get; set; }
        public string? UserId { get; set; }
    }
}
