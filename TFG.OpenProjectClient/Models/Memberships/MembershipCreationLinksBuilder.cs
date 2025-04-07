using TFG.OpenProjectClient.Models.Roles;

namespace TFG.OpenProjectClient.Models.Memberships
{
	public static class MembershipCreationLinksBuilder
	{
		public static MembershipCreationLinks Build(int principalId, int[] rolesIds, int projectId)
		{
			return new MembershipCreationLinks
			{
				Principal = new Link { Href = $"/api/v3/users/{principalId}" },
				Roles = [.. rolesIds.Select(role => new Link { Href = $"/api/v3/roles/{role}" })],
				Project = new Link { Href = $"/api/v3/projects/{projectId}" }
			};
		}
	}
}
