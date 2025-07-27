using System.Text.Json;
using TFG.OpenProjectClient.Models;
using TFG.OpenProjectClient.Models.Memberships;

namespace TFG.OpenProjectClient.Impl
{
	public class MembershipClient(IOpenProjectHttpClient httpClient) : IMembershipsClient
	{
		public async Task<Membership> CreateAsync(MembershipCreation membershipCreation)
		{
			var response = await httpClient.PostAsync("memberships", membershipCreation);
			string responseBody = await response.Content.ReadAsStringAsync();
			Membership membershipCreated = JsonSerializer.Deserialize<Membership>(responseBody)!;
			return membershipCreated;
		}

		public Task DeleteAsync(int membershipId)
		{
			return httpClient.DeleteAsync($"memberships/{membershipId}");
		}

		public async Task<OpenProjectCollection<Membership>> GetAsync(GetMembershipsQuery? query = null)
		{
			var response = await httpClient.GetAsync($"memberships?{query}");
			return await ParseReponseBody<OpenProjectCollection<Membership>>(response);
		}

		private static async Task<T> ParseReponseBody<T>(HttpResponseMessage responseMessage)
		{
			string responseBody = await responseMessage.Content.ReadAsStringAsync();
			return JsonSerializer.Deserialize<T>(responseBody)!;
		}
	}
}
