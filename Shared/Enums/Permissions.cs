namespace Shared.Enums
{
	[Flags]
    public enum Permissions
    {
        None = 0,
        //Projects
        ViewAllProjects = 1 << 0,
        EditProjects = 1 << 1,
        DeleteProjects = 1 << 2,
        CreateProjects = 1 << 3,
        ViewProjectUsers = 1 << 4,
        ViewProjectKpis = 1 << 5,
        ViewProjectOtherUserData = 1 << 6,
		//Users
		ViewUsers = 1 << 7,
        CreateUsers = 1 << 8,
        EditUsers = 1 << 9,
        DeleteUsers = 1 << 10,
        CreateRoles = 1 << 11,
        UpdateRoles = 1 << 12,

        //Config
        UpdateApiIntegrations = 1 << 13,

        All = ViewAllProjects | EditProjects | DeleteProjects | CreateProjects | ViewProjectUsers | ViewProjectKpis | ViewProjectOtherUserData |
              ViewUsers | CreateUsers | EditUsers | DeleteUsers | CreateRoles | UpdateRoles | UpdateApiIntegrations
	}
}