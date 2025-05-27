using MediatR;

namespace TFG.Application.Security
{
	public class PermissionBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
		   where TRequest : notnull
	{
		private readonly IEnumerable<IPermissionValidator<TRequest>> _validators;

		public PermissionBehavior(IEnumerable<IPermissionValidator<TRequest>> validators)
		{
			_validators = validators;
		}

		public async Task<TResponse> Handle(
			TRequest request,
			RequestHandlerDelegate<TResponse> next,
			CancellationToken cancellationToken)
		{
			foreach (var validator in _validators)
			{
				if (!await validator.HasPermissionAsync(request))
				{
					throw new UnauthorizedAccessException("Permission denied.");
				}
			}

			return await next(cancellationToken);
		}
	}
}
