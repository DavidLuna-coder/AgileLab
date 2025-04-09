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
			OpenProjectCollection<WorkPackage> workPackages = await ParseReponseBody<OpenProjectCollection<WorkPackage>>(response);
			return workPackages;
		}

		public async Task<OpenProjectCollection<GitlabMergeRequest>> GetGitlabMergeRequestsAsync(int workPackageId)
		{
			var response = await httpClient.GetAsync($"work_packages/{workPackageId}/gitlab_merge_requests");
			OpenProjectCollection<GitlabMergeRequest> mergeRequests = await ParseReponseBody<OpenProjectCollection<GitlabMergeRequest>>(response);
			return mergeRequests;
		}

		private static async Task<T> ParseReponseBody<T>(HttpResponseMessage responseMessage)
		{
			string responseBody = await responseMessage.Content.ReadAsStringAsync();
			return JsonSerializer.Deserialize<T>(responseBody)!;
		}
	}
}
