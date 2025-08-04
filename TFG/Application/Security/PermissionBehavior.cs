using MediatR;

namespace TFG.Application.Security;

public class PermissionBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
	   where TRequest : notnull
{
	private readonly IEnumerable<IPermissionValidator<TRequest>> _validators;
	private readonly IUserInfoAccessor _userInfoAccessor;
	public PermissionBehavior(IEnumerable<IPermissionValidator<TRequest>> validators, IUserInfoAccessor userInfoAccessor)
	{
		_validators = validators;
		_userInfoAccessor = userInfoAccessor;
	}

	public async Task<TResponse> Handle(
		TRequest request,
		RequestHandlerDelegate<TResponse> next,
		CancellationToken cancellationToken)
	{
		foreach (var validator in _validators)
		{
			bool hasPermission = (_userInfoAccessor.UserInfo is not null && _userInfoAccessor.UserInfo.IsAdmin) || await validator.HasPermissionAsync(request);
			if (!hasPermission)
			{
				throw new UnauthorizedAccessException("Permission denied.");
			}
		}

		return await next(cancellationToken);
	}
}
