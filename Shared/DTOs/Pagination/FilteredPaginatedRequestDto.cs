namespace Shared.DTOs.Pagination
{
	public class FilteredPaginatedRequestDto<T> : PaginatedRequestDtoBase
	{
		public T? Filters { get; set; }
	}
}
