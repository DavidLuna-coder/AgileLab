namespace TFG.OpenProjectClient
{
	public interface IOpenProjectApiClient
	{
		IUsersClient Users { get; }
		IProjectsClient Project { get; }
		IMembershipsClient Memberships { get; }
		IStatusesClient Statuses { get; }
	}
}
