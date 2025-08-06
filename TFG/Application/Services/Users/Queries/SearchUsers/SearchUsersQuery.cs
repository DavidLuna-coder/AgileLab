using MediatR;
using Shared.DTOs.Pagination;
using Shared.DTOs.Users;

namespace TFG.Application.Services.Users.Queries.SearchUsers
{
    public class SearchUsersQuery : IRequest<PaginatedResponseDto<FilteredUserDto>>
    {
        public FilteredPaginatedRequestDto<GetUsersQueryParameters> Request { get; set; }
    }
}