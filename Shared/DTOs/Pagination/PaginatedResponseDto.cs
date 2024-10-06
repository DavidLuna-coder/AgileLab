namespace Shared.DTOs.Pagination
{
	public record PaginatedResponseDto<T>
	{
		public IEnumerable<T> Items { get; set; } = [];
		public int TotalCount { get; set; }
		public int PageNumber { get; set; }
		public int PageSize { get; set; }
	}
}
