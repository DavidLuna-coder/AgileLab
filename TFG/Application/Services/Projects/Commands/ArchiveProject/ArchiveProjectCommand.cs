using MediatR;

namespace TFG.Application.Services.Projects.Commands.ArchiveProject
{
    public class ArchiveProjectCommand : IRequest
    {
        public Guid ProjectId { get; set; }
        public bool Archive { get; set; } // true = archivar, false = desarchivar
    }
}
