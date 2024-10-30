using LinqKit;
using Shared.DTOs.Projects;
using System.Linq.Expressions;
using TFG.Model.Entities;

namespace TFG.Api.FilterHandlers
{
	public class ProjectFiltersHandler : IFiltersHandler<Project,ProjectQueryParameters>
	{
		public Expression<Func<Project, bool>> GetFilters(ProjectQueryParameters? filters)
		{
			var predicate = PredicateBuilder.New<Project>(true);

			if (filters == null) return predicate;

			predicate = predicate.And(p => string.IsNullOrEmpty(filters.Name) || p.Name.Contains(filters.Name));

			return predicate;
		}
	}
}
