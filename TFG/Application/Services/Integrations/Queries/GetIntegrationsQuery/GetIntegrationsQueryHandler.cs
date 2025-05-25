using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.DTOs.Integrations;
using TFG.Infrastructure.Data;

namespace TFG.Application.Services.Integrations.Queries.GetIntegrationsQuery
{
	public class GetIntegrationsQueryHandler(ApplicationDbContext context) : IRequestHandler<GetIntegrationsQuery, List<IntegrationDto>>
	{
		public async Task<List<IntegrationDto>> Handle(GetIntegrationsQuery request, CancellationToken cancellationToken)
		{
			var integrations = await context.ApiConfigurations.ToListAsync(cancellationToken);
			return integrations.Select(i => new IntegrationDto
			{
				Name = i.Name,
				Type = i.Type,
				BaseUrl = i.BaseUrl,
				Token = i.Token
			}).ToList();
		}
	}
}
