namespace TFG.Application.Services.GitlabIntegration.Dtos
{
	public record GitlabProjectMembersResponseDto(List<GitlabProjectMemberResponseDto> Members);
	public record GitlabProjectMemberResponseDto(int Id, string Name, string Username, string State, string AvatarUrl, string WebUrl);
}
