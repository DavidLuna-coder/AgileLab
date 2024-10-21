using AutoMapper;
using Shared.DTOs.Projects;
using TFG.Model.Entities;

namespace TFG.Api.Mappers
{
	public class ProjectsMapperProfile : Profile
	{
		public ProjectsMapperProfile()
		{
			CreateMap<Project, FilteredProjectDto>();
			CreateMap<CreateProjectDto, Project>();
			CreateMap<Project, ProjectDto>();
		}
	}
}
