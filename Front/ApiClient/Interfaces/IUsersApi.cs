using Shared.DTOs.Pagination;
using Shared.DTOs.Users;
using Shared.Enums;

namespace Front.ApiClient.Interfaces
{
	public interface IUsersApi
	{
		Task<PaginatedResponseDto<FilteredUserDto>> GetFilteredUsers(FilteredPaginatedRequestDto<GetUsersQueryParameters> queryParameters, CancellationToken cancellationToken = default);
		Task Create(CreateUserDto registrationDto, CancellationToken cancellationToken = default);
		Task Delete(string id, CancellationToken cancellationToken = default);
		Task<UserDto> Update(string id, EditUserDto dto, CancellationToken cancellationToken = default);
		Task<Permissions> GetMyPermissions();
		Task<UserDto> GetUserById(string id, CancellationToken cancellationToken = default);
	}
}
