using MediatR;
using Shared.DTOs.Projects.Metrics;

namespace TFG.Application.Services.Projects.Queries.GetMosAffectedFiles
{
	public class GetMostAffectedFilesQuery : IRequest<List<AffectedFileDto>>
	{
		public Guid ProjectId { get; set; }
	}
}
