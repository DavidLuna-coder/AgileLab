namespace Shared.DTOs
{
	public record LoginResponseDto
	{
		public string Token { get; set; }
		public DateTime ExpirationDate { get; set; }
	}
}
