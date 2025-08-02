using NGitLab;
using NGitLab.Models;

namespace TFG.Tests.Fakes;

public class FakeGitLabClient : IGitLabClient
{
	public IUserClient Users => throw new NotImplementedException();
	public IProjectClient Projects { get; } = new FakeProjectClient();
	public IIssueClient Issues => throw new NotImplementedException();
	public IGroupsClient Groups => throw new NotImplementedException();
	public IGlobalJobClient Jobs => throw new NotImplementedException();
	public ILabelClient Labels => throw new NotImplementedException();
	public IRunnerClient Runners => throw new NotImplementedException();
	public IMergeRequestClient MergeRequests => throw new NotImplementedException();
	public ILintClient Lint => throw new NotImplementedException();
	public IEventClient GetEvents() => throw new NotImplementedException();
	public IEventClient GetUserEvents(long userId) => throw new NotImplementedException();
	public IEventClient GetProjectEvents(ProjectId projectId) => throw new NotImplementedException();
	public IRepositoryClient GetRepository(ProjectId projectId) => throw new NotImplementedException();
	public ICommitClient GetCommits(ProjectId projectId) => throw new NotImplementedException();
	public ICommitStatusClient GetCommitStatus(ProjectId projectId) => throw new NotImplementedException();
	public IPipelineClient GetPipelines(ProjectId projectId) => throw new NotImplementedException();
	public IPipelineScheduleClient GetPipelineSchedules(ProjectId projectId) => throw new NotImplementedException();
	public ITriggerClient GetTriggers(ProjectId projectId) => throw new NotImplementedException();
	public IJobClient GetJobs(ProjectId projectId) => throw new NotImplementedException();
	public IMergeRequestClient GetMergeRequest(ProjectId projectId) => throw new NotImplementedException();
	public IMergeRequestClient GetGroupMergeRequest(GroupId groupId) => throw new NotImplementedException();
	public IMilestoneClient GetMilestone(ProjectId projectId) => throw new NotImplementedException();
	public IMilestoneClient GetGroupMilestone(GroupId groupId) => throw new NotImplementedException();
	public IReleaseClient GetReleases(ProjectId projectId) => throw new NotImplementedException();
	public IMembersClient Members { get; } = new FakeMembersClient();
	public IVersionClient Version => throw new NotImplementedException();
	public INamespacesClient Namespaces => throw new NotImplementedException();
	public ISnippetClient Snippets => throw new NotImplementedException();
	public ISystemHookClient SystemHooks => throw new NotImplementedException();
	public IDeploymentClient Deployments => throw new NotImplementedException();
	public IEpicClient Epics => throw new NotImplementedException();
	public IGraphQLClient GraphQL => throw new NotImplementedException();
	public ISearchClient AdvancedSearch => throw new NotImplementedException();
	public IProjectIssueNoteClient GetProjectIssueNoteClient(ProjectId projectId) => throw new NotImplementedException();
	public IEnvironmentClient GetEnvironmentClient(ProjectId projectId) => throw new NotImplementedException();
	public IClusterClient GetClusterClient(ProjectId projectId) => throw new NotImplementedException();
	public IWikiClient GetWikiClient(ProjectId projectId) => throw new NotImplementedException();
	public IProjectBadgeClient GetProjectBadgeClient(ProjectId projectId) => throw new NotImplementedException();
	public IGroupBadgeClient GetGroupBadgeClient(GroupId groupId) => throw new NotImplementedException();
	public IProjectVariableClient GetProjectVariableClient(ProjectId projectId) => throw new NotImplementedException();
	public IGroupVariableClient GetGroupVariableClient(GroupId groupId) => throw new NotImplementedException();
	public IProjectLevelApprovalRulesClient GetProjectLevelApprovalRulesClient(ProjectId projectId) => throw new NotImplementedException();
	public IProtectedBranchClient GetProtectedBranchClient(ProjectId projectId) => throw new NotImplementedException();
	public IProtectedTagClient GetProtectedTagClient(ProjectId projectId) => throw new NotImplementedException();
	public ISearchClient GetGroupSearchClient(GroupId groupId) => throw new NotImplementedException();
	public ISearchClient GetProjectSearchClient(ProjectId projectId) => throw new NotImplementedException();
	public IGroupHooksClient GetGroupHooksClient(GroupId groupId) => throw new NotImplementedException();

	public bool CreateProjectCalled => ((FakeProjectClient)Projects).CreateProjectCalled;
	public bool ShouldFailOnCreate { get => ((FakeProjectClient)Projects).ShouldFailOnCreate; set => ((FakeProjectClient)Projects).ShouldFailOnCreate = value; }

	private class FakeProjectClient : IProjectClient
	{
		public bool CreateProjectCalled { get; private set; } = false;
		public bool ShouldFailOnCreate { get; set; } = false;

		public Task<Project> CreateAsync(ProjectCreate projectCreate, CancellationToken cancellationToken = default)
		{
			if (ShouldFailOnCreate)
			{
				throw new Exception("Simulated GitLab project creation failure");
			}

			CreateProjectCalled = true;
			return Task.FromResult(new Project { Id = 1 });
		}

		public Task DeleteAsync(ProjectId projectId, CancellationToken cancellationToken = default)
		{
			return Task.CompletedTask;
		}

		public IEnumerable<Project> Accessible => throw new NotImplementedException();
		public IEnumerable<Project> Owned => throw new NotImplementedException();
		public IEnumerable<Project> Visible => throw new NotImplementedException();
		public IEnumerable<Project> Get(ProjectQuery query) => throw new NotImplementedException();
		public GitLabCollectionResponse<Project> GetAsync(ProjectQuery query) => throw new NotImplementedException();
		public Project Create(ProjectCreate project) => throw new NotImplementedException();
		public Project Update(string id, ProjectUpdate projectUpdate) => throw new NotImplementedException();
		public Task<Project> UpdateAsync(ProjectId projectId, ProjectUpdate projectUpdate, CancellationToken cancellationToken = default) => Task.FromResult(new Project { Id = 1 });
		public void Delete(long id) => throw new NotImplementedException();
		public void Archive(long id) => throw new NotImplementedException();
		public void Unarchive(long id) => throw new NotImplementedException();
		public Project GetById(long id, SingleProjectQuery query) => throw new NotImplementedException();
		public Project Fork(string id, ForkProject forkProject) => throw new NotImplementedException();
		public Task<Project> GetByIdAsync(long id, SingleProjectQuery query, CancellationToken cancellationToken = default) => throw new NotImplementedException();
		public Task<Project> GetByNamespacedPathAsync(string path, SingleProjectQuery query, CancellationToken cancellationToken = default) => throw new NotImplementedException();
		public Task<Project> GetAsync(ProjectId id, SingleProjectQuery query, CancellationToken cancellationToken = default) => Task.FromResult(new Project { Id = long.Parse(id.ValueAsString()) });
		public Task<Project> ForkAsync(string id, ForkProject forkProject, CancellationToken cancellationToken = default) => throw new NotImplementedException();
		public IEnumerable<Project> GetForks(string id, ForkedProjectQuery query) => throw new NotImplementedException();
		public GitLabCollectionResponse<Project> GetForksAsync(string id, ForkedProjectQuery query) => throw new NotImplementedException();
		public GitLabCollectionResponse<Group> GetGroupsAsync(ProjectId id, ProjectGroupsQuery query) => throw new NotImplementedException();
		public GitLabCollectionResponse<ProjectTemplate> GetProjectTemplatesAsync(ProjectId id, ProjectTemplateType type) => throw new NotImplementedException();
		public Task<ProjectMergeRequestTemplate> GetProjectMergeRequestTemplateAsync(ProjectId id, string templateName, CancellationToken cancellationToken = default) => throw new NotImplementedException();
		public Dictionary<string, double> GetLanguages(string id) => throw new NotImplementedException();
		public Project this[long id] => throw new NotImplementedException();
		public Project this[string path] => throw new NotImplementedException();
		public Task<UploadedProjectFile> UploadFile(string id, FormDataContent content) => Task.FromResult(new UploadedProjectFile());

		UploadedProjectFile IProjectClient.UploadFile(string id, FormDataContent data)
		{
			throw new NotImplementedException();
		}
	}

	private class FakeMembersClient : IMembersClient
	{
		public Task<Membership> AddMemberToProjectAsync(ProjectId projectId, ProjectMemberCreate user, CancellationToken cancellationToken = default)
		{
			return Task.FromResult(new Membership());
		}

		public IEnumerable<Membership> OfProject(string projectId) => throw new NotImplementedException();
		public IEnumerable<Membership> OfProject(string projectId, bool includeInheritedMembers) => throw new NotImplementedException();
		public IEnumerable<Membership> OfProject(string projectId, bool includeInheritedMembers, MemberQuery query) => throw new NotImplementedException();
		public GitLabCollectionResponse<Membership> OfProjectAsync(ProjectId projectId, bool includeInheritedMembers) => throw new NotImplementedException();
		public GitLabCollectionResponse<Membership> OfProjectAsync(ProjectId projectId, bool includeInheritedMembers = false, MemberQuery query = null) => throw new NotImplementedException();
		public Membership GetMemberOfProject(string projectId, string userId) => throw new NotImplementedException();
		public Membership GetMemberOfProject(string projectId, string userId, bool includeInheritedMembers) => throw new NotImplementedException();
		public Task<Membership> GetMemberOfProjectAsync(ProjectId projectId, long userId, bool includeInheritedMembers = false, CancellationToken cancellationToken = default) => throw new NotImplementedException();
		public Membership AddMemberToProject(string projectId, ProjectMemberCreate user) => throw new NotImplementedException();
		public Membership UpdateMemberOfProject(string projectId, ProjectMemberUpdate user) => throw new NotImplementedException();
		public Task<Membership> UpdateMemberOfProjectAsync(ProjectId projectId, ProjectMemberUpdate user, CancellationToken cancellationToken = default) => throw new NotImplementedException();
		public Task RemoveMemberFromProjectAsync(ProjectId projectId, long userId, CancellationToken cancellationToken = default) => throw new NotImplementedException();
		public IEnumerable<Membership> OfNamespace(string groupId) => throw new NotImplementedException();
		public IEnumerable<Membership> OfGroup(string groupId) => throw new NotImplementedException();
		public IEnumerable<Membership> OfGroup(string groupId, bool includeInheritedMembers) => throw new NotImplementedException();
		public IEnumerable<Membership> OfGroup(string groupId, bool includeInheritedMembers, MemberQuery query) => throw new NotImplementedException();
		public GitLabCollectionResponse<Membership> OfGroupAsync(GroupId groupId, bool includeInheritedMembers) => throw new NotImplementedException();
		public GitLabCollectionResponse<Membership> OfGroupAsync(GroupId groupId, bool includeInheritedMembers = false, MemberQuery query = null) => throw new NotImplementedException();
		public Membership GetMemberOfGroup(string groupId, string userId) => throw new NotImplementedException();
		public Membership GetMemberOfGroup(string groupId, string userId, bool includeInheritedMembers) => throw new NotImplementedException();
		public Task<Membership> GetMemberOfGroupAsync(GroupId groupId, long userId, bool includeInheritedMembers = false, CancellationToken cancellationToken = default) => throw new NotImplementedException();
		public Membership AddMemberToGroup(string groupId, GroupMemberCreate user) => throw new NotImplementedException();
		public Task<Membership> AddMemberToGroupAsync(GroupId groupId, GroupMemberCreate user, CancellationToken cancellationToken = default) => throw new NotImplementedException();
		public Membership UpdateMemberOfGroup(string groupId, GroupMemberUpdate user) => throw new NotImplementedException();
		public Task<Membership> UpdateMemberOfGroupAsync(GroupId groupId, GroupMemberUpdate user, CancellationToken cancellationToken = default) => throw new NotImplementedException();
		public Task RemoveMemberFromGroupAsync(GroupId groupId, long userId, CancellationToken cancellationToken = default) => throw new NotImplementedException();
	}
}