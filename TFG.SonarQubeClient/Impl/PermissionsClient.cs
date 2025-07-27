using System.Text.Json;
using System.Text.Json.Serialization;
using TFG.SonarQubeClient.Models;

namespace TFG.SonarQubeClient.Impl
{
	public class PermissionsClient(ISonarHttpClient client) : IPermissionsClient
	{
		private readonly JsonSerializerOptions jsonSerializerOptions = new JsonSerializerOptions
		{
			Converters = { new JsonStringEnumConverter() }
		};
		public async Task AddUserAsync(UserPermission userPermission)
		{
			string permissionString = JsonSerializer.Serialize(userPermission.Permission,jsonSerializerOptions).Trim('"');
			await client.PostAsync($"permissions/add_user?login={userPermission.Login}&permission={permissionString.ToLowerInvariant()}&projectKey={userPermission.ProjectKey}", userPermission);
		}

		public async Task DeleteUserAsync(UserPermission userPermission)
		{
			string permissionString = JsonSerializer.Serialize(userPermission.Permission, jsonSerializerOptions).Trim('"');
			await client.PostAsync($"permissions/remove_user?login={userPermission.Login}&permission={permissionString.ToLowerInvariant()}&projectKey={userPermission.ProjectKey}", userPermission);
		}
	}
}
