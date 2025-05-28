using MediatR;
using Shared.DTOs.Roles;

namespace TFG.Application.Services.Roles.Queries.GetRol
{
	public class GetRolQuery : IRequest<RolDto>
	{
		public Guid Id { get; set; }
	}
}
