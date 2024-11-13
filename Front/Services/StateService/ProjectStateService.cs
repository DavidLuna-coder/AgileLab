using Front.ApiClient.Interfaces;
using Shared.DTOs.Projects;

namespace Front.Services.StateService
{
	public class ProjectStateService
	{
		private readonly IProjectsApi _projectsApi;
		private DateTime _lastLoaded;
		private readonly TimeSpan _expirationTime = TimeSpan.FromMinutes(10);
		private bool _needRefresh = false;
		public ProjectDto CurrentProject { get; set; } = default!;
		public event Action OnChange;

		public ProjectStateService(IProjectsApi projectsApi)
		{
			_projectsApi = projectsApi;
		}

		public void SetCurrentProject(ProjectDto project)
		{
			CurrentProject = project;
			_lastLoaded = DateTime.UtcNow;
			NotifyStateChanged();
		}

		public async Task LoadProject(Guid id, bool forceRefresh = false)
		{
			if (forceRefresh || _needRefresh || CurrentProject == null || CurrentProject.Id != id || DateTime.UtcNow - _lastLoaded > _expirationTime)
			{
				CurrentProject = await _projectsApi.GetProject(id);
				_lastLoaded = DateTime.UtcNow; // Reset the last loaded time
				_needRefresh = false;
				NotifyStateChanged();
			}
		}

		public void NeedsRefresh() => _needRefresh = true;
		private void NotifyStateChanged() => OnChange?.Invoke();

	}
}
