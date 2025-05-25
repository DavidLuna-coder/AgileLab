using MediatR;
using Shared.DTOs.Integrations;

namespace TFG.Application.Services.Integrations.Queries.GetIntegrationsQuery
{
	public class GetIntegrationsQuery : IRequest<List<IntegrationDto>>
	{
	}
}
