using System.Text.Json;
using TFG.OpenProjectClient.Models.Memberships;

namespace TFG.OpenProjectClient.Impl
{
	public class MembershipClient(IOpenProjectHttpClient httpClient) : IMembershipsClient
	{
		public async Task<MembershipCreated> CreateAsync(MembershipCreation membershipCreation)
		{
			var response = await httpClient.PostAsync("memberships", membershipCreation);
			string responseBody = await response.Content.ReadAsStringAsync();
			MembershipCreated membershipCreated = JsonSerializer.Deserialize<MembershipCreated>(responseBody)!;
			return membershipCreated;
		}
	}
}
