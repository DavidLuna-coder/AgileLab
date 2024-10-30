using LinqKit;
using Shared.DTOs.Users;
using System.Linq.Expressions;
using TFG.Model.Entities;

namespace TFG.Api.FilterHandlers
{
	public interface IFiltersHandler<TEntity,TFilters>
	{
		public Expression<Func<TEntity, bool>> GetFilters(TFilters? filters);
	}
}
