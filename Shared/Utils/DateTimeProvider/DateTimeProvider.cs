namespace Shared.Utils.DateTimeProvider
{
	public class DateTimeProvider : IDateTimeProvider
	{
		public DateTime Now => DateTime.Now;
		public DateTime UtcNow => DateTime.UtcNow;
	}
}
