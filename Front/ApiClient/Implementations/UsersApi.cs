using Front.ApiClient.Interfaces;
using Shared.DTOs.Pagination;
using Shared.DTOs.Users;

namespace Front.ApiClient.Implementations
{
    public class UsersApi(IApiHttpClient apiHttpClient) : IUsersApi
    {
        private const string BASE_URL = "api/users";
        IApiHttpClient _apiHttpClient = apiHttpClient;

        public async Task<PaginatedResponseDto<FilteredUserDto>> GetFilteredUsers(PaginatedRequestDto<GetUsersQueryParameters> queryParameters, CancellationToken cancellationToken = default)
        {
            var response = await _apiHttpClient.PostAsync<PaginatedRequestDto<GetUsersQueryParameters> ,PaginatedResponseDto<FilteredUserDto>>($"{BASE_URL}/search", queryParameters);
            return response;
        }
    }
}
