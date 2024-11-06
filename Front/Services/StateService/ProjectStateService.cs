using Front.ApiClient.Interfaces;
using Shared.DTOs.Projects;

namespace Front.Services.StateService
{
	public class ProjectStateService(IProjectsApi projectsApi)
	{
		public ProjectDto CurrentProject { get; private set; }
		public event Action OnChange;

		public void SetCurrentProject(ProjectDto project)
		{
			CurrentProject = project;
			NotifyStateChanged();
		}

		private void NotifyStateChanged() => OnChange?.Invoke();

		public async Task LoadProject(Guid id)
		{
			CurrentProject = await projectsApi.GetProject(id);
			NotifyStateChanged();
		}
	}

}
