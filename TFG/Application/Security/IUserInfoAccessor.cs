namespace TFG.Application.Security
{
	public class HttpContextUserInfoAccessor : IUserInfoAccessor
	{
		private readonly IHttpContextAccessor _httpContextAccessor;

		public HttpContextUserInfoAccessor(IHttpContextAccessor httpContextAccessor)
		{
			_httpContextAccessor = httpContextAccessor;
		}

		public IUserInfo? UserInfo =>
			_httpContextAccessor.HttpContext?.Items[nameof(IUserInfo)] as IUserInfo;
	}

	public interface IUserInfoAccessor
	{
		IUserInfo? UserInfo { get; }
	}
}
