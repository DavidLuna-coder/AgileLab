using MediatR;
using Shared.DTOs.Experiences;

namespace TFG.Application.Services.Experiences.Commands
{
	public class CreateGoRaceExperienceCommand : IRequest<GoRaceExperienceDto>
	{
		public CreateGoRaceExperienceDto Dto { get; set; }
	}

	public class UpdateGoRaceExperienceCommand : IRequest<GoRaceExperienceDto?>
	{
		public Guid Id { get; set; }
		public UpdateGoRaceExperienceDto Dto { get; set; }
	}

	public class DeleteGoRaceExperienceCommand : IRequest
	{
		public Guid Id { get; set; }
	}
}
