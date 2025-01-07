using TFG.Application.Dtos;
using TFG.Domain.Results;

namespace TFG.Application.Interfaces
{
	public interface ITokenService
	{
		public Result<UserInfo> ExtractUserInfo(string token);
	}
}
