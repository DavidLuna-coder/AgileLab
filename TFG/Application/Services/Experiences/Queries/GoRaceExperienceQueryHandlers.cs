using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.DTOs.Experiences;
using TFG.Api.Mappers;
using TFG.Domain.Entities;
using TFG.Infrastructure.Data;

namespace TFG.Application.Services.Experiences.Queries
{
	public class GetGoRaceExperienceQueryHandler : IRequestHandler<GetGoRaceExperienceQuery, GoRaceExperienceDto?>
	{
		private readonly ApplicationDbContext _context;
		public GetGoRaceExperienceQueryHandler(ApplicationDbContext context) => _context = context;
		public async Task<GoRaceExperienceDto?> Handle(GetGoRaceExperienceQuery request, CancellationToken cancellationToken)
		{
			GoRaceExperience? exp = await _context.GoRaceProjectExperiences
				.Include(e => e.Project)
				.FirstOrDefaultAsync(e => e.Id == request.Id, cancellationToken);

			if (exp == null)
			{
				exp = await _context.GoRacePlatformExperiences
					.Include(e => e.Projects)
					.FirstOrDefaultAsync(e => e.Id == request.Id, cancellationToken);
			}

			return exp?.ToGoRaceExperienceDto();
		}
	}

	public class GetAllGoRaceExperiencesQueryHandler : IRequestHandler<GetAllGoRaceExperiencesQuery, List<GoRaceExperienceDto>>
	{
		private readonly ApplicationDbContext _context;
		public GetAllGoRaceExperiencesQueryHandler(ApplicationDbContext context) => _context = context;
		public async Task<List<GoRaceExperienceDto>> Handle(GetAllGoRaceExperiencesQuery request, CancellationToken cancellationToken)
		{
			IEnumerable<GoRaceExperience> exps;
			switch (request.ExperienceType)
			{
				case GoRaceExperienceTypes.Project:
					exps = (await _context.GoRaceProjectExperiences.Include(e => e.Project).ToListAsync(cancellationToken)).Cast<GoRaceExperience>();
					break;
				case GoRaceExperienceTypes.Platform:
					exps = (await _context.GoRacePlatformExperiences.Include(e => e.Projects).ToListAsync(cancellationToken)).Cast<GoRaceExperience>();
					break;
				default:
					exps = Enumerable.Empty<GoRaceExperience>();
					break;
			}
			return exps.Select(e => e.ToGoRaceExperienceDto()).ToList();
		}
	}
}
