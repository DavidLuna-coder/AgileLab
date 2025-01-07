using TFG.Application.Services.GitlabIntegration.Enums;

namespace TFG.Application.Services.GitlabIntegration.Dtos
{
	public record GitlabAddMembersToProjectDto
	{
		public required int Id { get; set; } // The Id of the project
		public required string UserId { get; set; } // Can receive multiple user ids separated by commas
		public required GitlabAcessLevel AccessLevel { get; set; } // The access level of the user
	}
}
