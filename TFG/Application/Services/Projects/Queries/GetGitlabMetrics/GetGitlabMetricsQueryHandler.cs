using MediatR;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using NGitLab;
using NGitLab.Models;
using Shared.DTOs.Projects.Metrics;
using System.Globalization;
using TFG.Api.Exeptions;
using TFG.Infrastructure.Data;
using Project = TFG.Domain.Entities.Project;
using User = TFG.Domain.Entities.User;

namespace TFG.Application.Services.Projects.Queries.GetGitlabMetrics;

public class GetGitlabMetricsQueryHandler(IGitLabClient gitlabClient, ApplicationDbContext dbContext) : IRequestHandler<GetGitlabMetricsQuery, GitlabMetricsDto>
{
	public async Task<GitlabMetricsDto> Handle(GetGitlabMetricsQuery request, CancellationToken cancellationToken)
	{
		Project project = await dbContext.Projects.AsNoTracking().FirstOrDefaultAsync(p => p.Id == request.ProjectId) ?? throw new NotFoundException("Project not found");

		User? user;
		if (!string.IsNullOrEmpty(request.UserId))
		{
			user = await dbContext.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == request.UserId) ?? throw new NotFoundException("User not found");
		}
		else
		{
			user = null;
		}

		IRepositoryClient repositoryClient = gitlabClient.GetRepository(project.GitlabId);
		IEnumerable<Commit> commits = repositoryClient.Commits.Where(FilterCommitByUserExprresion(user));
		long? gitlabUserId = user is null ? null : long.Parse(user.GitlabId);
		IEnumerable<MergeRequest> mergeRequests = GetMergeRequests(project, gitlabUserId);
		IEnumerable<Issue> issues = await GetIssues(project, gitlabUserId);

		GitlabMetricsDto gitlabMetrics = new()
		{
			UserId = request.UserId,
			TotalCommits = commits.Count(),
			CreatedMergeRequests = mergeRequests.Count(mr => mr.Author.Id == gitlabUserId),
			MergedMergeRequests = mergeRequests.Count(mr => mr.State == MergeRequestState.merged.ToString()),
			CommitsPerWeek = CommitsPerWeek(commits),
			ClosedIssues = issues.Count(i => i.State == IssueState.closed.ToString()),
			TotalIssues = issues.Count(),
		};

		return gitlabMetrics;
	}

	private async Task<IEnumerable<Issue>> GetIssues(Project project, long? gitlabUserId)
	{
		IssueQuery issueQuery = new();
		var gitlabProject = await gitlabClient.Projects.GetAsync(project.GitlabId);

		if (gitlabUserId is not null)
			issueQuery.AuthorId = gitlabUserId;

		IEnumerable<Issue> issues = gitlabClient.Issues.Get(gitlabProject.Id, issueQuery);
		return issues;
	}

	private IEnumerable<MergeRequest> GetMergeRequests(Project project, long? gitlabUserId = null)
	{
		MergeRequestQuery mergeRequestQuery = new();
		IMergeRequestClient mergeRequestClient = gitlabClient.GetMergeRequest(project.GitlabId);

		if (gitlabUserId is null)
			mergeRequestQuery.AuthorId = gitlabUserId;

		return mergeRequestClient.Get(mergeRequestQuery);
	}

	private static Func<Commit, bool> FilterCommitByUserExprresion(User? user = null)
	{
		if (user is null)
		{
			return c => true;
		}


		return c => c.AuthorEmail.ToLower() == user.Email.ToLower() ||
					c.CommitterEmail.ToLower() == user.Email.ToLower() ||
					c.AuthorName.ToLower() == user.UserName.ToLower() || c.CommitterName.ToLower() == user.UserName.ToLower();
	}


	public static double AverageWeeklyMergeRequest(IEnumerable<MergeRequest> mrs)
	{
		if (!mrs.Any())
			return 0;

		var culture = CultureInfo.CurrentCulture;
		var calendar = culture.Calendar;
		var weekRule = CalendarWeekRule.FirstFourDayWeek;
		var firstDayOfWeek = DayOfWeek.Monday;

		var mrsPorSemana = mrs
			.GroupBy(mr =>
			{
				var date = mr.CreatedAt;
				int year = date.Year;
				int week = calendar.GetWeekOfYear(date, weekRule, firstDayOfWeek);
				return (year, week);
			})
			.Select(g => new
			{
				Semana = g.Key,
				Count = g.Count()
			})
			.ToList();

		return mrsPorSemana.Average(g => g.Count);
	}

	public static double CommitsPerWeek(IEnumerable<Commit> commits)
	{
		if (!commits.Any())
			return 0;

		var culture = System.Globalization.CultureInfo.CurrentCulture;
		var calendar = culture.Calendar;
		var weekRule = System.Globalization.CalendarWeekRule.FirstFourDayWeek;
		var firstDayOfWeek = DayOfWeek.Monday;

		// Fecha de inicio (puede ser la primera semana con commits, o inicio proyecto)
		var minDate = commits.Min(c => c.AuthoredDate);
		var maxDate = commits.Max(c => c.AuthoredDate);

		// Agrupar commits por semana
		var commitsPerWeek = commits
			.GroupBy(c =>
			{
				var date = c.AuthoredDate;
				return (Year: date.Year, Week: calendar.GetWeekOfYear(date, weekRule, firstDayOfWeek));
			})
			.Select(g => g.Count())
			.ToList();

		// Calcular mediana para mitigar outliers
		commitsPerWeek.Sort();
		int count = commitsPerWeek.Count;
		double median;
		if (count % 2 == 0)
			median = (commitsPerWeek[count / 2 - 1] + commitsPerWeek[count / 2]) / 2.0;
		else
			median = commitsPerWeek[count / 2];

		return median;
	}
}
