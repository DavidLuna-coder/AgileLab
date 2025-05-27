using LinqKit;
using Shared.DTOs.Pagination;
using Shared.DTOs.Users;
using System.Linq.Expressions;
using TFG.Domain.Entities;

namespace TFG.Api.FilterHandlers
{
	public class UserFiltersHandler : IFiltersHandler<User,GetUsersQueryParameters>
	{
		public Expression<Func<User,bool>> GetFilters(GetUsersQueryParameters? filters)
		{
			var predicate = PredicateBuilder.New<User>(true);
			if (filters != null)
			{
				predicate = predicate.And(u => filters.Ids == null || filters.Ids.Count == 0 || filters.Ids.Contains(u.Id));
				predicate = predicate.And(u => string.IsNullOrEmpty(filters.FirstName) || u.FirstName.Contains(filters.FirstName));
				predicate = predicate.And(u => string.IsNullOrEmpty(filters.LastName) || u.LastName.Contains(filters.LastName));
			}

			return predicate;
		}
	}
}
