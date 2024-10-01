namespace Shared.DTOs.Pagination
{
	public record PaginatedResponseDto<T>
	{
		public List<T> Items { get; set; }
		public int ItemsCount { get; set; }
		public int Page { get; set; } = 0;
		public int PageCount { get; set; }
	}
}
