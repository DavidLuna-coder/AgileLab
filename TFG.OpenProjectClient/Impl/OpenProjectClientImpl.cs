namespace TFG.OpenProjectClient.Impl
{
	public class OpenProjectClientImpl(IUsersClient usersClient, IProjectsClient projectsClient, IMembershipsClient membershipsClient, IStatusesClient statusesClient, IWorkPackagesClient workPackagesClient) : IOpenProjectClient
	{
		public IUsersClient Users { get; } = usersClient;

		public IProjectsClient Projects { get; } = projectsClient;

		public IMembershipsClient Memberships { get; } = membershipsClient;

		public IStatusesClient Statuses { get; } = statusesClient;
		public IWorkPackagesClient WorkPackages { get; } = workPackagesClient;
	}
}
