using System.Text.Json;
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
	}
}
