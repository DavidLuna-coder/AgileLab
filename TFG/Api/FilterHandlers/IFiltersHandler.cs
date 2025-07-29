using System.Linq.Expressions;

namespace TFG.Api.FilterHandlers
{
	public interface IFiltersHandler<TEntity,TFilters>
	{
		public Expression<Func<TEntity, bool>> GetFilters(TFilters? filters);
	}
}
