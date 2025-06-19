using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.DTOs.Users;
using TFG.Api.Mappers;
using TFG.Infrastructure.Data;
using TFG.Api.Exeptions;

namespace TFG.Application.Services.Users.Queries.GetUserById
{
    public class GetUserByIdQueryHandler(ApplicationDbContext context) : IRequestHandler<GetUserByIdQuery, UserDto>
    {
        public async Task<UserDto> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            var user = await context.Users.FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken);
            if (user == null)
                throw new NotFoundException($"User with id {request.UserId} not found");
            return user.ToUserDto();
        }
    }
}
