using MediatR;
using Microsoft.AspNetCore.Mvc;
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
	}
}
