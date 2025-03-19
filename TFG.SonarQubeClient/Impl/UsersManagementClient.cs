using System.Text.Json;
using TFG.SonarQubeClient.Models;

namespace TFG.SonarQubeClient.Impl
{
	public class UsersManagementClient(ISonarHttpClient client) : IUsersManagementClient
	{
		private readonly JsonSerializerOptions _serializerOptions = new(JsonSerializerDefaults.Web);
		public async Task<User> CreateAsync(UserCreation user)
		{
			var response = await client.PostAsync("users-management/users", user, "v2");
			string responseBody = await response.Content.ReadAsStringAsync();
			User createdUser = JsonSerializer.Deserialize<User>(responseBody, _serializerOptions) ?? throw new HttpRequestException("Failed user deserialization");
			return createdUser;
		}

		public Task DeleteAsync(string userId, bool anonymize = false)
		{
			return client.DeleteAsync($"users-management/users/{userId}", "v2");
		}
	}
}
