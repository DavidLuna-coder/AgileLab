using MudBlazor;
using Shared.DTOs.Pagination;

namespace Front.Helpers
{
	public static class MudGridExtensions
	{
		public static FilteredPaginatedRequestDto<TFilter> ToFilteredPaginatedRequestDto<TFilter,TData>(this GridState<TData> state)
		{
			FilteredPaginatedRequestDto<TFilter> request = new()
			{
				Page = state.Page,
				PageSize = state.PageSize
			};

			return request;
		}

		public static GridData<T> ToGridData<T>(this PaginatedResponseDto<T> data)
		{
			GridData<T> gridData = new()
			{
				Items = data.Items,
				TotalItems = data.TotalCount
			};
			return gridData;
		}
	}
}
