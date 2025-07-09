using Humanizer;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.DTOs.Experiences;
using TFG.Api.Mappers;
using TFG.Domain.Entities;
using TFG.Infrastructure.Data;

namespace TFG.Application.Services.Experiences.Commands
{
	public class CreateGoRaceExperienceCommandHandler : IRequestHandler<CreateGoRaceExperienceCommand, GoRaceExperienceDto>
	{
		private readonly ApplicationDbContext _context;
		public CreateGoRaceExperienceCommandHandler(ApplicationDbContext context) => _context = context;
		public async Task<GoRaceExperienceDto> Handle(CreateGoRaceExperienceCommand request, CancellationToken cancellationToken)
		{
			GoRaceExperience entity;
			var dto = request.Dto;
			if (dto.ExperienceType == GoRaceExperienceTypes.Project)
			{
				entity = new GoRaceProjectExperience
				{
					Id = Guid.NewGuid(),
					Name = dto.Name,
					Token = dto.Token,
					Description = dto.Description,
					CreatedAt = DateTimeOffset.UtcNow,
					ProjectId = dto.ProjectId ?? Guid.Empty,
					ImprovementScoreFactor = dto.ImprovementScoreFactor,
					MaxOnTimeTasksScore = dto.MaxOnTimeTasksScore,
					MaxQualityScore = dto.MaxQualityScore
				};
			}
			else if (dto.ExperienceType == GoRaceExperienceTypes.Platform)
			{
				var projectOwnersProjectIds = dto.ProjectOwners!.Select(p => p.ProjectId).ToList();
				var projects = await _context.Projects.Where(p => projectOwnersProjectIds.Contains(p.Id)).ToListAsync(cancellationToken);
				var id = Guid.NewGuid();
				entity = new GoRacePlatformExperience
				{
					Id = id,
					Name = dto.Name,
					Token = dto.Token,
					Description = dto.Description,
					CreatedAt = DateTimeOffset.UtcNow,
					Projects = [.. projects.Select(p => new GoRacePlatformExperienceProject
					{
						GoRacePlatformExperienceId = id,
						ProjectId = p.Id,
						Project = p,
						OwnerEmail = request.Dto.ProjectOwners?.First(po => po.ProjectId == p.Id).Email,
					})],
					MaxQualityScore = dto.MaxQualityScore,
					MaxOnTimeTasksScore = dto.MaxOnTimeTasksScore,
					ImprovementScoreFactor = dto.ImprovementScoreFactor
				};
				
			}
			else
			{
				entity = new GoRaceExperience
				{
					Id = Guid.NewGuid(),
					Name = dto.Name,
					Token = dto.Token,
					Description = dto.Description,
					CreatedAt = DateTimeOffset.UtcNow,
					MaxQualityScore = dto.MaxQualityScore,
					MaxOnTimeTasksScore = dto.MaxOnTimeTasksScore,
					ImprovementScoreFactor = dto.ImprovementScoreFactor
				};
			}
			_context.GoRaceExperiences.Add(entity);
			await _context.SaveChangesAsync(cancellationToken);
			return entity.ToGoRaceExperienceDto();
		}
	}

	public class UpdateGoRaceExperienceCommandHandler : IRequestHandler<UpdateGoRaceExperienceCommand, GoRaceExperienceDto?>
	{
		private readonly ApplicationDbContext _context;
		public UpdateGoRaceExperienceCommandHandler(ApplicationDbContext context) => _context = context;
		public async Task<GoRaceExperienceDto?> Handle(UpdateGoRaceExperienceCommand request, CancellationToken cancellationToken)
		{
			var entity = await _context.GoRaceExperiences
				.Include(e => (e as GoRaceProjectExperience).Project)
				.Include(e => (e as GoRacePlatformExperience).Projects)
				.FirstOrDefaultAsync(e => e.Id == request.Id, cancellationToken);
			if (entity == null) return null;
			
			entity.Name = request.Dto.Name;
			entity.Token = request.Dto.Token;
			entity.Description = request.Dto.Description;
			entity.MaxQualityScore = request.Dto.MaxQualityScore;
			entity.ImprovementScoreFactor = request.Dto.ImprovementScoreFactor;
			entity.MaxOnTimeTasksScore = request.Dto.MaxOnTimeTasksScore;

			if (entity is GoRaceProjectExperience p && request.Dto.ProjectId.HasValue)
				p.ProjectId = request.Dto.ProjectId.Value;
			if (entity is GoRacePlatformExperience plat && request.Dto.ProjectOwners != null)
			{
				var projectOwnersProjectIds = request.Dto.ProjectOwners!.Select(p => p.ProjectId).ToList();
				var projects = await _context.Projects.Where(p => projectOwnersProjectIds.Contains(p.Id)).ToListAsync(cancellationToken);

				plat.Projects = projects
					.Select(p => new GoRacePlatformExperienceProject
					{
						GoRacePlatformExperienceId = plat.Id,
						ProjectId = p.Id,
						Project = p,
						OwnerEmail = request.Dto.ProjectOwners.First(po => po.ProjectId == p.Id).Email ?? null
					}).ToList();
			}
			await _context.SaveChangesAsync(cancellationToken);
			return entity.ToGoRaceExperienceDto();
		}
	}

	public class DeleteGoRaceExperienceCommandHandler : IRequestHandler<DeleteGoRaceExperienceCommand>
	{
		private readonly ApplicationDbContext _context;
		public DeleteGoRaceExperienceCommandHandler(ApplicationDbContext context) => _context = context;
		public async Task Handle(DeleteGoRaceExperienceCommand request, CancellationToken cancellationToken)
		{
			var entity = await _context.GoRaceExperiences.FindAsync([request.Id], cancellationToken);
			if (entity != null)
			{
				_context.GoRaceExperiences.Remove(entity);
				await _context.SaveChangesAsync(cancellationToken);
			}
		}
	}
}
