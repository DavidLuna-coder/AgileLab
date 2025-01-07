namespace TFG.Application.Services.GitlabIntegration.Enums
{
	public enum GitlabAcessLevel
	{
		NoAccess = 0,
		MinimalAccess = 5,
		Guest = 10,
		Reporter = 20,
		Developer = 30,
		Maintainer = 40,
		Owner = 50
	}
}
