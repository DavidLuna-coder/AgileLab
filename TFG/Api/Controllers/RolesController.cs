using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs.Roles;
using TFG.Application.Services.Roles.Commands.CreateRol;
using TFG.Application.Services.Roles.Commands.DeleteRol;
using TFG.Application.Services.Roles.Commands.UpdateRol;
using TFG.Application.Services.Roles.Queries.GetAllRoles;
using TFG.Application.Services.Roles.Queries.GetRol;

namespace TFG.Api.Controllers
{
	[Route("api/roles")]
	[ApiController]
	public class RolesController(IMediator mediator) : ControllerBase
	{
		[HttpPost]
		public async Task<IActionResult> CreateRol(CreateRolDto createRolDto)
		{
			CreateRolCommand command = new()
			{
				Name = createRolDto.Name,
				Permissions = createRolDto.Permissions
			};

			var result = await mediator.Send(command);

			return CreatedAtAction(nameof(GetRol), new { id = result.Id }, result);
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> UpdateRol(Guid id, UpdateRolDto updateRolDto)
		{
			UpdateRolCommand command = new()
			{
				Id = id,
				UpdateRol = updateRolDto
			};

			var response = await mediator.Send(command);
			return Ok(response);
		}

		[HttpGet]
		public async Task<IActionResult> GetRoles()
		{
			GetAllRolesQuery query = new();
			var result = await mediator.Send(query);
			return Ok(result); // Implement the logic to retrieve all roles
		}

		[HttpGet("{id}")]
		public async Task<IActionResult> GetRol(Guid id)
		{
			GetRolQuery query = new() { Id = id };

			var result = await mediator.Send(query);

			return Ok(result);
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteRol(Guid id)
		{
			DeleteRolCommand command = new() { Id = id };
			await mediator.Send(command);
			return NoContent();
		}
	}
}
