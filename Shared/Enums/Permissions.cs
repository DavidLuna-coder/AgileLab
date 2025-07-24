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
        ArchiveProjects = 1 << 7, // Nuevo permiso para archivar proyectos
		//Users
		ViewUsers = 1 << 8,
        CreateUsers = 1 << 9,
        EditUsers = 1 << 10,
        DeleteUsers = 1 << 11,
        CreateRoles = 1 << 12,
        EditRoles = 1 << 13,
        ViewRoles = 1 << 14,
        DeleteRoles = 1 << 15,
		//Config
		UpdateApiIntegrations = 1 << 16,

        // Experiences
        CreateExperiences = 1 << 17,
        EditExperiences = 1 << 18,
        DeleteExperiences = 1 << 19,

		All = ViewAllProjects | EditProjects | DeleteProjects | CreateProjects | ViewProjectUsers | ViewProjectKpis | ViewProjectOtherUserData |
              ArchiveProjects |
              ViewUsers | CreateUsers | EditUsers | DeleteUsers | CreateRoles | EditRoles | UpdateApiIntegrations | ViewRoles | DeleteRoles |
              CreateExperiences | EditExperiences | DeleteExperiences
	}
}