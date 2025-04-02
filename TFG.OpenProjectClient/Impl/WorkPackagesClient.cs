using System.Text.Json;
using TFG.OpenProjectClient.Models;
using TFG.OpenProjectClient.Models.WorkPackages;

namespace TFG.OpenProjectClient.Impl
{
	public class WorkPackagesClient(IOpenProjectHttpClient httpClient) : IWorkPackagesClient
	{
		public async Task<OpenProjectCollection<WorkPackage>> GetAsync(int projectId, GetWorkPackagesQuery query)
		{
			var response = await httpClient.GetAsync($"projects/{projectId}/work_packages?{query}");
			string responseBody = await response.Content.ReadAsStringAsync();
			OpenProjectCollection<WorkPackage> workPackages = JsonSerializer.Deserialize<OpenProjectCollection<WorkPackage>>(responseBody)!;
			return workPackages;
		}
	}
}
