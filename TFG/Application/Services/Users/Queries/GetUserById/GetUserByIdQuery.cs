using MediatR;
using Shared.DTOs.Users;

namespace TFG.Application.Services.Users.Queries.GetUserById
{
    public class GetUserByIdQuery : IRequest<UserDto>
    {
        public string UserId { get; set; }
    }
}
