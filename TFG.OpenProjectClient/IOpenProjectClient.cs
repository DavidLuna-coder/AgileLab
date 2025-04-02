namespace TFG.OpenProjectClient
{
	public interface IOpenProjectClient
	{
		IUsersClient Users { get; }
		IProjectsClient Project { get; }
		IMembershipsClient Memberships { get; }
		IStatusesClient Statuses { get; }
		IWorkPackagesClient WorkPackages { get; }
	}
}
