using MediatR;
using Shared.DTOs.Experiences;

namespace TFG.Application.Services.Experiences.Queries
{
	public class GetGoRaceExperienceQuery : IRequest<GoRaceExperienceDto?>
	{
		public Guid Id { get; set; }
	}

	public class GetAllGoRaceExperiencesQuery : IRequest<List<GoRaceExperienceDto>>
	{
		public string ExperienceType { get; set; } // Use GoRaceExperienceTypes constants
	}
}
