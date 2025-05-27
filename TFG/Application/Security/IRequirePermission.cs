using Shared.Enums;

namespace Shared.Security
{
    public interface IRequirePermission
    {
        Permissions RequiredPermission { get; }
    }
}