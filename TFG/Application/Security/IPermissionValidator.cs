namespace TFG.Application.Security;

public interface IPermissionValidator<in TRequest>
{
	Task<bool> HasPermissionAsync(TRequest request);
}
