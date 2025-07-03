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
			if (dto.ExperienceType == "Project")
			{
				entity = new GoRaceProjectExperience
				{
					Id = Guid.NewGuid(),
					Name = dto.Name,
					Token = dto.Token,
					Description = dto.Description,
					CreatedAt = DateTimeOffset.UtcNow,
					ProjectId = dto.ProjectId ?? Guid.Empty
				};
			}
			else if (dto.ExperienceType == "Platform")
			{
				var projects = await _context.Projects.Where(p => dto.ProjectOwners!.Contains(p.Id)).ToListAsync(cancellationToken);
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
						OwnerEmail = request.Dto.OwnerEmail // Assuming OwnerEmail is passed in the command
					})]
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
					CreatedAt = DateTimeOffset.UtcNow
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
			if (entity is GoRaceProjectExperience p && request.Dto.ProjectId.HasValue)
				p.ProjectId = request.Dto.ProjectId.Value;
			if (entity is GoRacePlatformExperience plat && request.Dto.ProjectOwners != null)
			{

				plat.Projects = await _context.Projects.Where(pr => request.Dto.ProjectOwners.Contains(pr.Id))
					.Select(p => new GoRacePlatformExperienceProject
					{
						GoRacePlatformExperienceId = plat.Id,
						ProjectId = p.Id,
						Project = p,
						OwnerEmail = request.Dto.OwnerEmail
					}).ToListAsync(cancellationToken);
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
