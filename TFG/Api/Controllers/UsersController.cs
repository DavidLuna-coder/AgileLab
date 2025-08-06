using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs;
using Shared.DTOs.Pagination;
using Shared.DTOs.Users;
using Shared.Enums;
using System.Threading.Tasks;
using TFG.Api.FilterHandlers;
using TFG.Api.Mappers;
using TFG.Application.Security;
using TFG.Application.Services.Users.Commands.DeleteUser;
using TFG.Application.Services.Users.Commands.EditUser;
using TFG.Application.Services.Users.Commands.RegisterUser;
using TFG.Domain.Entities;
using TFG.Infrastructure.Data;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TFG.Api.Controllers
{
	[Route("api/users")]
	[ApiController]
	public class UsersController(UserManager<User> userManager, IMediator mediator, IUserInfoAccessor userInfoAccessor, ApplicationDbContext context) : ControllerBase
	{
		private readonly UserManager<User> _userManager = userManager;
		// GET: api/<UsersController>
		[HttpPost("search")]
		public IActionResult Get(FilteredPaginatedRequestDto<GetUsersQueryParameters> request)
		{
			var usersQuery = _userManager.Users.AsQueryable();
			IFiltersHandler<User, GetUsersQueryParameters> filtersHandler = new UserFiltersHandler();

			var predicate = filtersHandler.GetFilters(request.Filters);
			usersQuery = usersQuery.Where(predicate);

			if (request.PageSize >= 0)
			{
				usersQuery = usersQuery.Skip((request.Page) * request.PageSize).Take(request.PageSize);
			}

			List<User> users = [.. usersQuery];
			List<FilteredUserDto> usersDto = users.Select(u => u.ToFilteredUserDto()).ToList();
			PaginatedResponseDto<FilteredUserDto> response = new()
			{
				Items = usersDto,
				TotalCount = usersQuery.Count(),
				PageNumber = request.Page,
				PageSize = request.PageSize
			};
			return Ok(response);
		}

		// GET api/<UsersController>/5
		[HttpGet("{id}")]
		public IActionResult Get(string id)
		{
			var usersQuery = _userManager.Users.AsQueryable();
			User? user = usersQuery.FirstOrDefault(u => u.Id == id);
			if (user == null) return NotFound();

			UserDto userDto = user.ToUserDto();
			return Ok(userDto);
		}

		// POST api/<UsersController>
		[HttpPost]
		public async Task<IActionResult> Post([FromBody] CreateUserDto value)
		{
			RegisterUserCommand command = new()
			{
				Email = value.Email,
				Password = value.Password,
				UserName = value.UserName,
				FirstName = value.FirstName,
				LastName = value.LastName,
				Language = value.Language,
				Roles = value.Roles ?? new List<Guid>()
			};

			await mediator.Send(command);
			return Created("", "");
		}

		// PUT api/<UsersController>/5
		[HttpPut("{id}")]
		public async Task<IActionResult> Put(string id, [FromBody] EditUserDto request)
		{
			EditUserCommand command = new()
			{
				UserId = id,
				RolesIds = request.RolesIds
			};

			var result = await mediator.Send(command);
			return Ok(result);
		}

		// DELETE api/<UsersController>/5
		[HttpDelete("{id}")]
		public async Task<IActionResult> Delete(string id)
		{
			DeleteUserCommand command = new() { UserId = id };

			await mediator.Send(command);

			return NoContent();
		}

		[HttpGet("my-permissions")]
		public async Task<IActionResult> GetMyPermissions()
		{
			var userId = userInfoAccessor.UserInfo?.UserId ?? throw new Exception("UserId is null");

			if (userInfoAccessor.UserInfo.IsAdmin)
				return Ok(Permissions.All);

			var userPermissions = await context.GetCombinedPermissionsAsync(userId);

			return Ok(userPermissions);
		}
	}
}
