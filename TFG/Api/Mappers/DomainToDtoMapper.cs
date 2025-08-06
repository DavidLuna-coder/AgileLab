using Shared.DTOs.Projects;
using Shared.DTOs.Projects.Metrics;
using Shared.DTOs.Roles;
using Shared.DTOs.Users;
using Shared.DTOs.Experiences;
using TFG.Domain.Entities;
using TFG.SonarQubeClient.Models.Metrics;

namespace TFG.Api.Mappers
{
	public static class DomainToDtoMapper
	{
		public static ProjectDto ToProjectDto(this Project project)
		{
			return new ProjectDto
			{
				Id = project.Id,
				Name = project.Name,
				Description = project.Description,
				UsersIds = project.Users.Select(u => u.Id).ToList() ,
				CreatedAt = project.CreatedAt
			};
		}

		public static FilteredProjectDto ToFilteredProjectDto(this Project project)
		{
			return new FilteredProjectDto
			{
				Id = project.Id,
				Name = project.Name,
				CreatedAt = project.CreatedAt,
				Members = project.Users.Select(u => new UserReferenceDto
				{
					Id = u.Id,
					Email = u.Email!
				}).ToList()
			};
		}

		public static UserDto ToUserDto(this User user)
		{
			return new UserDto
			{
				Id = user.Id,
				FirstName = user.FirstName,
				LastName = user.LastName,
				Email = user.Email!,
				Roles = user.Roles?.Select(r => new FilteredUserRolDto
				{
					Id = r.Id,
					Name = r.Name,
				}).ToList() ?? []
			};
		}

		public static FilteredUserDto ToFilteredUserDto(this User user)
		{
			return new FilteredUserDto
			{
				Id = user.Id,
				FirstName = user.FirstName,
				LastName = user.LastName,
				Email = user.Email!,
				IsAdmin = user.IsAdmin
			};
		}

		public static ProjectMetricsDto ToProjectMetricsDto(this SonarMetricsResponse sonarMetricsResponse)
		{
			ProjectMetricsDto dto = new()
			{
				Measures = sonarMetricsResponse.Component.Measures.Select(m => new ProjectMeasureDto
				{
					Value = m.Value,
					Metric = m.Metric,
				}).ToList()
			};

			return dto;
		}

		public static RolDto ToRolDto(this Rol rol)
		{
			return new RolDto
			{
				Id = rol.Id,
				Name = rol.Name,
				Permissions = rol.Permissions
			};
		}

		public static GoRaceExperienceDto ToGoRaceExperienceDto(this GoRaceExperience exp)
		{
			var dto = new GoRaceExperienceDto
			{
				Id = exp.Id,
				Name = exp.Name,
				Url = exp.Url,
				Token = exp.Token,
				Description = exp.Description,
				CreatedAt = exp.CreatedAt,
				ProjectId = (exp is GoRaceProjectExperience p) ? p.ProjectId : null,
				ProjectOwners = (exp is GoRacePlatformExperience plat) ? 
					plat.Projects?.Select(pr => new ProjectOwnerDto
					{
						ProjectId = pr.ProjectId,
						Email = pr.OwnerEmail
					}).ToList() : null,
				ExperienceType = exp switch
				{
					GoRaceProjectExperience => GoRaceExperienceTypes.Project,
					GoRacePlatformExperience => GoRaceExperienceTypes.Platform,
					_ => "Base"
				},
				ImprovementScoreFactor = exp.ImprovementScoreFactor,
				MaxOnTimeTasksScore = exp.MaxOnTimeTasksScore,
				MaxQualityScore = exp.MaxQualityScore
			};
			return dto;
		}
	}
}
