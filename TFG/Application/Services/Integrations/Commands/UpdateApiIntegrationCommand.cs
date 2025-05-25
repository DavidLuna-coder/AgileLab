using MediatR;
using Shared.DTOs.Integrations;

namespace TFG.Application.Services.Integrations.Commands
{
	public class UpdateApiIntegrationCommand : IRequest<IntegrationDto>
	{
		public ApiConfigurationType ApiConfigurationType { get; set; } // Tipo de configuración de la API a actualizar
		public UpdateApiIntegrationDto Dto { get; set; }
	}
}
