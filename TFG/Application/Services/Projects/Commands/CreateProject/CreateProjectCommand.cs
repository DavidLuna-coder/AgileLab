using MediatR;
using Shared.DTOs.Projects;
using TFG.Application.Services.Roles.Queries.GetRol;
using TFG.Domain.Results;

namespace TFG.Application.Services.Projects.Commands.CreateProject;

public class CreateProjectCommand : IRequest<ProjectDto>
{
	public string Name { get; set; }
	public string? Description { get; set; } = string.Empty;
	public List<string>? UsersIds { get; set; }
	public string? Template { get; set; }
}