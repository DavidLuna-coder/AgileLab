using LinqKit;
using Shared.DTOs.Projects;
using System.Linq.Expressions;
using TFG.Domain.Entities;

namespace TFG.Api.FilterHandlers
{
	public class ProjectFiltersHandler : IFiltersHandler<Project,ProjectQueryParameters>
	{
		public Expression<Func<Project, bool>> GetFilters(ProjectQueryParameters? filters)
		{
			var predicate = PredicateBuilder.New<Project>(true);

			if (filters == null) return predicate;

			predicate = predicate.And(p => string.IsNullOrEmpty(filters.Name) || p.Name.Contains(filters.Name));
			predicate = predicate.And(p => filters.ExperiencesIds == null
								  || filters.ExperiencesIds.Count == 0
								  || p.ProjectExperiences.Any(e => filters.ExperiencesIds.Contains(e.Id))
								  || p.PlatformExperiences.Any(e => filters.ExperiencesIds.Contains(e.GoRacePlatformExperienceId)));

			predicate = predicate.And(p => filters.ProjectIds == null
					  || filters.ProjectIds.Count == 0
					  || filters.ProjectIds.Contains(p.Id));

			predicate = predicate.And(p => filters.IsArchived == null || p.IsArchived == filters.IsArchived);

			return predicate;
		}
	}
}
