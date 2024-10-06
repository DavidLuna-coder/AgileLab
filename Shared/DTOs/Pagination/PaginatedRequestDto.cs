namespace Shared.DTOs.Pagination
{
	public class PaginatedRequestDto<T>
	{
		public T? Filters { get; set; }
		public int Page { get; set; }
		public int PageSize { get; set; }
	}
}
