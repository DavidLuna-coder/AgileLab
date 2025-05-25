using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs.Integrations;
using TFG.Api.Exeptions;
using TFG.Application.Services.Integrations.Commands;
using TFG.Application.Services.Integrations.Queries.GetIntegrationsQuery;

namespace TFG.Api.Controllers
{
	[Route("api/integrations/")]
	[ApiController]
	public class ApiIntegrationsController(IMediator mediator) : ControllerBase
	{
		[HttpGet]
		public async Task<IActionResult> GetIntegrations()
		{
			GetIntegrationsQuery query = new();
			var result = await mediator.Send(query);
			return Ok(result);
		}
		[HttpPut("{apiIntegrationType}")]
		public async Task<IActionResult> UpdateApiIntegration(ApiConfigurationType apiIntegrationType, [FromBody] UpdateApiIntegrationDto dto)
		{
			var command = new UpdateApiIntegrationCommand { ApiConfigurationType = apiIntegrationType, Dto = dto };
			try
			{
				await mediator.Send(command);
				return NoContent();
			}
			catch (NotFoundException ex)
			{
				return NotFound(ex.Message);
			}
		}
	}
}
