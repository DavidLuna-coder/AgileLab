using Shared.Enums;

namespace Front.Services.StateService
{
	public interface IUserService
	{
		Permissions? Permissions { get; }
		Task InitializeAsync();
	}
}
