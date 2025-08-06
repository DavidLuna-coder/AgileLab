using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.DTOs.Pagination;
using Shared.DTOs.Users;
using TFG.Api.FilterHandlers;
using TFG.Domain.Entities;
using TFG.Infrastructure.Data;

namespace TFG.Application.Services.Users.Queries.SearchUsers;

public class SearchUsersQueryHandler : IRequestHandler<SearchUsersQuery, PaginatedResponseDto<FilteredUserDto>>
{
	private readonly ApplicationDbContext _context;

	public SearchUsersQueryHandler(ApplicationDbContext context)
	{
		_context = context;
	}

	public async Task<PaginatedResponseDto<FilteredUserDto>> Handle(SearchUsersQuery query, CancellationToken cancellationToken)
	{
		var request = query.Request;
		var usersQuery = _context.Users.AsQueryable();
		IFiltersHandler<User, GetUsersQueryParameters> filtersHandler = new UserFiltersHandler();
		// Apply filters
		var predicate = filtersHandler.GetFilters(request.Filters);
		usersQuery = usersQuery.Where(predicate);

		// Pagination
		var totalItems = await usersQuery.CountAsync(cancellationToken);
		List<User> users;
		if (request.PageSize >= 0)
		{
			users = await usersQuery.Include(u => u.Roles)
				.Skip(request.Page * request.PageSize)
				.Take(request.PageSize)
				.ToListAsync(cancellationToken);
		}
		else
		{
			users = await usersQuery.ToListAsync(cancellationToken);
		}

		// Map to DTOs
		var items = users.Select(u => new FilteredUserDto
		{
			Id = u.Id,
			FirstName = u.FirstName,
			LastName = u.LastName,
			Email = u.Email,
			IsAdmin = u.IsAdmin,
			Roles = u.Roles?.Select(r => new FilteredUserRolDto
			{
				Id = r.Id,
				Name = r.Name
			}).ToList() ?? []
		}).ToList();

		return new PaginatedResponseDto<FilteredUserDto>
		{
			Items = items,
			TotalCount = totalItems,
			PageNumber = request.Page,
			PageSize = request.PageSize
		};
	}
}