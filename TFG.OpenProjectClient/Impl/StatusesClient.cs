using System.Text.Json;
using TFG.OpenProjectClient.Models;
using TFG.OpenProjectClient.Models.Statuses;

namespace TFG.OpenProjectClient.Impl
{
	public class StatusesClient(IOpenProjectHttpClient httpClient) : IStatusesClient
	{
		public async Task<OpenProjectCollection<Status>> GetStatusesAsync()
		{
			var response = await httpClient.GetAsync("statuses");
			string responseBody = await response.Content.ReadAsStringAsync();

			OpenProjectCollection<Status> statuses = JsonSerializer.Deserialize<OpenProjectCollection<Status>>(responseBody)!;

			return statuses;
		}
	}
}
