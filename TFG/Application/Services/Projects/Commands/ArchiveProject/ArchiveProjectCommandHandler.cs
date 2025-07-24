using MediatR;
using Microsoft.EntityFrameworkCore;
using TFG.Api.Exeptions;
using TFG.Infrastructure.Data;

namespace TFG.Application.Services.Projects.Commands.ArchiveProject
{
    public class ArchiveProjectCommandHandler(ApplicationDbContext dbContext) : IRequestHandler<ArchiveProjectCommand>
    {
        public async Task Handle(ArchiveProjectCommand request, CancellationToken cancellationToken)
        {
            var project = await dbContext.Projects.FirstOrDefaultAsync(p => p.Id == request.ProjectId, cancellationToken);
            if (project == null)
                throw new NotFoundException($"No se encontró el proyecto con id {request.ProjectId}");
            project.IsArchived = request.Archive;
            dbContext.Projects.Update(project);
            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
