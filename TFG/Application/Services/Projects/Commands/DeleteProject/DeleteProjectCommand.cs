using MediatR;

namespace TFG.Application.Services.Projects.Commands.DeleteProject;

public class DeleteProjectCommand : IRequest
{
	public Guid ProjectId { get; set; }
}
