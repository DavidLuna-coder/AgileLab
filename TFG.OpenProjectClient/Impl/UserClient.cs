using System.Text.Json;
using TFG.OpenProjectClient.Models.Users;

namespace TFG.OpenProjectClient.Impl
{
	public class UserClient(IOpenProjectHttpClient httpClient) : IUsersClient
	{
		public async Task<UserCreated> CreateAsync(UserCreation userCreation)
		{
			var response = await httpClient.PostAsync("users", userCreation);
			string responseBody = await response.Content.ReadAsStringAsync();

			UserCreated userCreated = JsonSerializer.Deserialize<UserCreated>(responseBody)!;

			return userCreated;
		}

		public async Task DeleteAsync(int userId)
		{
			await httpClient.DeleteAsync($"users/{userId}");
		}
	}
}
