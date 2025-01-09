using TFG.Application.Services.GitlabIntegration.Enums;

namespace TFG.Application.Services.GitlabIntegration.Dtos
{
	/*
	 *  Id: Project Id
	 *	UserId: UserIds separated by commas
	 *	AccessLevel: Nivel de acceso
	 */
	public record GitlabAddMemberToProjectRequestDto(int Id, string UserId, GitlabAcessLevel AccessLevel);
}
