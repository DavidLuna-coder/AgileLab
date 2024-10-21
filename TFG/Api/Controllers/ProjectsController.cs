using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shared.DTOs.Pagination;
using Shared.DTOs.Projects;
using TFG.Infrastructure.Data;
using TFG.Model.Entities;

namespace TFG.Api.Controllers
{
	[Route("api/projects")]
	[ApiController]
	public class ProjectsController(ApplicationDbContext context, IMapper mapper) : ControllerBase
	{
		private readonly ApplicationDbContext _context = context;
		private readonly IMapper _mapper = mapper;

		// POST: api/Projects/search
		[HttpPost("search")]
		public async Task<ActionResult<PaginatedResponseDto<FilteredProjectDto>>> SearchProjects([FromBody] PaginatedRequestDto<ProjectQueryParameters> request)
		{
			var projectsQuery = _context.Projects.AsQueryable();

			// Aplicar filtros
			if (request.Filters != null)
			{
				if (!string.IsNullOrEmpty(request.Filters.Name))
				{
					projectsQuery = projectsQuery.Where(p => p.Name.Contains(request.Filters.Name));
				}
			}

			// Paginación
			var totalItems = await projectsQuery.CountAsync();
			var projects = await projectsQuery
				.Skip((request.Page) * request.PageSize)
				.Take(request.PageSize)
				.ToListAsync();

			List<FilteredProjectDto> items = _mapper.Map<List<FilteredProjectDto>>(projects);

			return new PaginatedResponseDto<FilteredProjectDto>
			{
				Items = items,
				TotalCount = totalItems,
				PageNumber = request.Page,
				PageSize = request.PageSize
			};
		}

		// GET: api/Projects/5
		[HttpGet("{id}")]
		public async Task<ActionResult<ProjectDto>> GetProject(Guid id)
		{
			var project = await _context.Projects.FindAsync(id);

			if (project == null)
			{
				return NotFound();
			}
			var projectDto = _mapper.Map<ProjectDto>(project);
			return projectDto;
		}

		// PUT: api/Projects/5
		// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
		[HttpPut("{id}")]
		public async Task<IActionResult> PutProject(Guid id, Project project)
		{
			if (id != project.Id)
			{
				return BadRequest();
			}

			_context.Entry(project).State = EntityState.Modified;

			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!ProjectExists(id))
				{
					return NotFound();
				}
				else
				{
					throw;
				}
			}

			return NoContent();
		}

		// POST: api/Projects
		// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
		[HttpPost]
		public async Task<ActionResult<Project>> CreateProject(CreateProjectDto projectDto)
		{
			var project = _mapper.Map<Project>(projectDto);
			project.CreatedAt = DateTime.UtcNow;
			_context.Projects.Add(project);
			await _context.SaveChangesAsync();

			return CreatedAtAction(nameof(GetProject), new { id = project.Id }, projectDto);
		}

		// DELETE: api/Projects/5
		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteProject(Guid id)
		{
			var project = await _context.Projects.FindAsync(id);
			if (project == null)
			{
				return NotFound();
			}

			_context.Projects.Remove(project);
			await _context.SaveChangesAsync();

			return NoContent();
		}

		private bool ProjectExists(Guid id)
		{
			return _context.Projects.Any(e => e.Id == id);
		}
	}
}
