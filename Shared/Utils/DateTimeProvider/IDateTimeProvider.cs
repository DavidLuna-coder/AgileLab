namespace Shared.Utils.DateTimeProvider
{
	public interface IDateTimeProvider
	{
		public DateTime Now { get; }
		public DateTime UtcNow { get; }
	}
}
