using MediatR;
using Shared.DTOs.Projects;

namespace TFG.Application.Services.Projects.Queries.GetProject
{
    public class GetProjectQuery : IRequest<ProjectDto>
    {
        public Guid ProjectId { get; set; }
        public string? UserId { get; set; }
    }
}
