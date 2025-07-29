using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.DTOs.Projects;
using TFG.Api.Exeptions;
using TFG.Api.Mappers;
using TFG.Domain.Entities;
using TFG.Infrastructure.Data;

namespace TFG.Application.Services.Projects.Queries.GetProject
{
    public class GetProjectQueryHandler(ApplicationDbContext context) : IRequestHandler<GetProjectQuery, ProjectDto>
    {
        public async Task<ProjectDto> Handle(GetProjectQuery request, CancellationToken cancellationToken)
        {
            var project = await context.Projects
                .Include(p => p.Users)
                .FirstOrDefaultAsync(p => p.Id == request.ProjectId, cancellationToken);

            if (project == null)
                throw new NotFoundException("Project not found");

            return project.ToProjectDto();
        }
    }
}
