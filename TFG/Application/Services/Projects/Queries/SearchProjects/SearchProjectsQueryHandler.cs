using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.DTOs.Pagination;
using Shared.DTOs.Projects;
using TFG.Domain.Entities;
using TFG.Infrastructure.Data;
using TFG.Api.Mappers;
using TFG.Application.Security;
using Shared.Enums;
using TFG.Api.FilterHandlers;
using TFG.Application.Services.Projects.Queries.SearchProjects;

namespace TFG.Application.Services.Projects.Queries.SearchProjects
{
    public class SearchProjectsQueryHandler(ApplicationDbContext context, IUserInfoAccessor userInfoAccessor) : IRequestHandler<SearchProjectsQuery, PaginatedResponseDto<FilteredProjectDto>>
    {
        public async Task<PaginatedResponseDto<FilteredProjectDto>> Handle(SearchProjectsQuery query, CancellationToken cancellationToken)
        {
            var userId = query.UserId;
            var request = query.Request;
            var projectsQuery = context.Projects.AsQueryable();

            // Comprobar permisos
            var userPermissions = await context.GetCombinedPermissionsAsync(userId);
            bool isAdmin = userInfoAccessor.UserInfo?.IsAdmin ?? false;
			if (!isAdmin && (userPermissions & Permissions.ViewAllProjects) == Permissions.None)
            {
                // Solo proyectos en los que participa el usuario
                projectsQuery = projectsQuery.Where(p => p.Users.Any(u => u.Id == userId));
            }

            // Aplicar filtros
            IFiltersHandler<Project, ProjectQueryParameters> projectFilterHandler = new ProjectFiltersHandler();
            var predicate = projectFilterHandler.GetFilters(request.Filters);
            projectsQuery = projectsQuery.Where(predicate);

            // Paginación
            var totalItems = await projectsQuery.CountAsync(cancellationToken);
            List<Project> projects;
            if (request.PageSize >= 0)
            {
                projects = await projectsQuery.Include(p => p.Users)
                    .Skip((request.Page) * request.PageSize)
                    .Take(request.PageSize)
                    .ToListAsync(cancellationToken);
            }
            else
            {
                projects = await projectsQuery.Include(p => p.Users)
                    .ToListAsync(cancellationToken);
            }

            List<FilteredProjectDto> items = projects.Select(p => p.ToFilteredProjectDto()).ToList();

            return new PaginatedResponseDto<FilteredProjectDto>
            {
                Items = items,
                TotalCount = totalItems,
                PageNumber = request.Page,
                PageSize = request.PageSize
            };
        }
    }
}
