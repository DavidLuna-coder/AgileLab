using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs.Pagination;
using Shared.DTOs.Users;
using TFG.Api.FilterHandlers;
using TFG.Api.Mappers;
using TFG.Model.Entities;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TFG.Api.Controllers
{
	[Route("api/users")]
	[ApiController]
	public class UsersController(UserManager<User> userManager) : ControllerBase
	{
		private readonly UserManager<User> _userManager = userManager;
		// GET: api/<UsersController>
		[HttpPost("search")]
		public IActionResult Get(PaginatedRequestDto<GetUsersQueryParameters> request)
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
			if(user == null) return NotFound();
			
			UserDto userDto = user.ToUserDto();
			return Ok(userDto);
		}

		// POST api/<UsersController>
		[HttpPost]
		public IActionResult Post([FromBody] string value)
		{
			return Created("", "");
		}

		// PUT api/<UsersController>/5
		[HttpPut("{id}")]
		public void Put(string id, [FromBody] string value)
		{
		}

		// DELETE api/<UsersController>/5
		[HttpDelete("{id}")]
		public void Delete(string id)
		{
		}
	}
}
