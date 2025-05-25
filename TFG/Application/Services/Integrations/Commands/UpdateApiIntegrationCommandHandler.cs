using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.DTOs.Integrations;
using TFG.Api.Exeptions;
using TFG.Infrastructure.Data;

namespace TFG.Application.Services.Integrations.Commands
{
	public class UpdateApiIntegrationCommandHandler(ApplicationDbContext context) : IRequestHandler<UpdateApiIntegrationCommand,IntegrationDto>
	{
		public async Task<IntegrationDto> Handle(UpdateApiIntegrationCommand request, CancellationToken cancellationToken)
		{
			// Buscar la configuración de la API por tipo
			var apiConfiguration = await context.ApiConfigurations
				.FirstOrDefaultAsync(c => c.Type == request.ApiConfigurationType)
					?? throw new NotFoundException($"No se encontró la configuración del tipo '{request.ApiConfigurationType}'.");

			apiConfiguration.BaseUrl = request.Dto.BaseUrl.Trim();
			apiConfiguration.Token = request.Dto.Token.Trim();
			await context.SaveChangesAsync(cancellationToken);

			return new()
			{
				BaseUrl = apiConfiguration.BaseUrl,
				Name = apiConfiguration.Name,
				Token = apiConfiguration.Token,
				Type = apiConfiguration.Type
			};
		}
	}
}
