using Front.ApiClient.Interfaces;
using Shared.DTOs.Pagination;
using Shared.DTOs.Users;

namespace Front.ApiClient.Implementations
{
	public class UsersApi(IApiHttpClient apiHttpClient) : IUsersApi
	{
		private const string BASE_URL = "api/users";
		readonly IApiHttpClient _apiHttpClient = apiHttpClient;

		public Task Create(CreateUserDto registrationDto, CancellationToken cancellationToken = default)
		{
			return _apiHttpClient.PostAsync<CreateUserDto>($"{BASE_URL}", registrationDto);
		}

		public Task Delete(string id, CancellationToken cancellationToken = default)
		{
			return _apiHttpClient.DeleteAsync($"{BASE_URL}/{id}");
		}

		public async Task<PaginatedResponseDto<FilteredUserDto>> GetFilteredUsers(FilteredPaginatedRequestDto<GetUsersQueryParameters> queryParameters, CancellationToken cancellationToken = default)
		{
			var response = await _apiHttpClient.PostAsync<FilteredPaginatedRequestDto<GetUsersQueryParameters>, PaginatedResponseDto<FilteredUserDto>>($"{BASE_URL}/search", queryParameters);
			return response;
		}

		public Task<UserDto> Update(string id, EditUserDto dto, CancellationToken cancellationToken = default)
		{
			return _apiHttpClient.PutAsync<EditUserDto, UserDto>($"{BASE_URL}/{id}", dto);
		}
	}
}
