using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs.Roles;
using TFG.Application.Services.Roles.Commands.CreateRol;

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
		public async Task<IActionResult> UpdateRol(Guid id, CreateRolDto updateRolDto)
		{
			// Placeholder for updating a role
			return NotFound(); // Implement the logic to update a role by ID
		}

		[HttpGet]
		public IActionResult GetRoles()
		{
			// Placeholder for getting all roles
			return NotFound(); // Implement the logic to retrieve all roles
		}

		[HttpGet("{id}")]
		public IActionResult GetRol(Guid id)
		{
			// Placeholder for getting a role by ID
			return NotFound(); // Implement the logic to retrieve a role by ID
		}
	}
}
