using TFG.OpenProjectClient.Models.Memberships;

namespace TFG.OpenProjectClient
{
	public interface IMembershipsClient
	{
		Task<Membership> CreateAsync(MembershipCreation membershipCreation);
		Task DeleteAsync(int membershipId);
	}
}
