using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs.Experiences;
using TFG.Application.Services.Experiences.Commands;
using TFG.Application.Services.Experiences.Queries;
using TFG.Api.Mappers;

namespace TFG.Api.Controllers
{
	[Route("api/experiences")]
	[ApiController]
	public class GoRaceExperiencesController(IMediator mediator) : ControllerBase
	{
		[HttpGet]
		public async Task<IActionResult> GetAll([FromQuery] string experienceType)
		{
			if (string.IsNullOrWhiteSpace(experienceType))
				return BadRequest($"experienceType is required. Use one of: {GoRaceExperienceTypes.Project}, {GoRaceExperienceTypes.Platform}");
			var result = await mediator.Send(new GetAllGoRaceExperiencesQuery { ExperienceType = experienceType });
			return Ok(result);
		}

		[HttpGet("{id}")]
		public async Task<IActionResult> Get(Guid id, [FromQuery] string experienceType)
		{
			if (string.IsNullOrWhiteSpace(experienceType))
				return BadRequest($"experienceType is required. Use one of: {GoRaceExperienceTypes.Project}, {GoRaceExperienceTypes.Platform}");
			var result = await mediator.Send(new GetGoRaceExperienceQuery { Id = id, ExperienceType = experienceType });
			if (result == null) return NotFound();
			return Ok(result);
		}

		[HttpPost]
		public async Task<IActionResult> Create([FromBody] CreateGoRaceExperienceDto dto)
		{
			var result = await mediator.Send(new CreateGoRaceExperienceCommand { Dto = dto });
			return CreatedAtAction(nameof(Get), new { id = result.Id, experienceType = result.ExperienceType }, result);
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> Update(Guid id, [FromBody] UpdateGoRaceExperienceDto dto)
		{
			var result = await mediator.Send(new UpdateGoRaceExperienceCommand { Id = id, Dto = dto });
			if (result == null) return NotFound();
			return Ok(result);
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> Delete(Guid id)
		{
			await mediator.Send(new DeleteGoRaceExperienceCommand { Id = id });
			return NoContent();
		}
	}
}
