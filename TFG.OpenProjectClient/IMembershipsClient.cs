using TFG.OpenProjectClient.Models.Memberships;

namespace TFG.OpenProjectClient
{
	public interface IMembershipsClient
	{
		Task<MembershipCreated> CreateAsync(MembershipCreation membershipCreation);
	}
}
