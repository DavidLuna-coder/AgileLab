using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using TFG.Application.Services.Experiences.ValueObjects;
using TFG.Domain.Entities;
using TFG.GoRaceClient;
using TFG.GoRaceClient.Dtos;
using TFG.Infrastructure.Data;

namespace TFG.Application.Services.Experiences.Commands
{
	public class GoRaceDataSender(ApplicationDbContext dbContext)
	{
		public void CalculatePlatformExperienceData()
		{
			var experiences = dbContext.GoRacePlatformExperiences.Include(e => e.Projects).ThenInclude(p => p.Project);

			foreach (var experience in experiences)
			{
				foreach (var projectRelation in experience.Projects)
				{
					var data = BuildExperienceSendableData(projectRelation, experience);
					if (data == null) continue;

					SendExperienceDataAsync(data.Value, experience, projectRelation.OwnerEmail);
				}
			}
		}

		public void CalculateProjectExperienceData()
		{
			var experiences = dbContext.GoRaceProjectExperiences.Include(e => e.Project).ThenInclude(p => p.Users);

			foreach (var experience in experiences)
			{
				if (experience.Project?.Users == null) continue;
				foreach (var user in experience.Project.Users)
				{
					if (string.IsNullOrEmpty(user.Email)) continue;

					var data = BuildUserProjectExperienceSendableData(experience, user);
					if (data == null) continue;

					SendExperienceDataAsync(data.Value, experience, user.Email);
				}
			}
		}

		private (ExperienceSendableData Data, string Manager)? BuildExperienceSendableData(GoRacePlatformExperienceProject projectRelation, GoRacePlatformExperience experience)
		{
			var snapshots = dbContext.ProjectStatusSnapshots
				.Where(s => s.ProjectId == projectRelation.Project.Id)
				.OrderByDescending(s => s.SnapshotDate)
				.Take(2)
				.ToList();

			if (snapshots.Count == 0 || string.IsNullOrEmpty(projectRelation.OwnerEmail))
				return null;

			var latest = snapshots[0];
			ProjectQualityValues latestValues = new(
				latest.Bugs,
				latest.Vulnerabilities,
				latest.CodeSmells,
				latest.Loc
			);
			QualityScore qualityScore = new(latestValues, experience.MaxQualityScore);
			ImprovementScore improvementScore;
			OnTimeTaskScore onTimeTaskScore = new(latest.OpenTasks - latest.OverdueTasks, latest.OpenTasks, experience.MaxOnTimeTasksScore);

			if (snapshots.Count == 2)
			{
				var previous = snapshots[1];
				ProjectQualityValues previousValues = new(
					previous.Bugs,
					previous.Vulnerabilities,
					previous.CodeSmells,
					previous.Loc
				);
				improvementScore = new ImprovementScore(latestValues, previousValues, experience.ImprovementScoreFactor);
			}
			else
			{
				improvementScore = new ImprovementScore(latestValues, latestValues, experience.ImprovementScoreFactor);
			}

			int closedTasks = latest.ClosedTasks;
			int mergedMergeRequest = latest.MergeRequestsMerged;

			var experienceSendableData = new ExperienceSendableData(
				qualityScore,
				improvementScore,
				onTimeTaskScore,
				closedTasks,
				mergedMergeRequest
			);

			return (experienceSendableData, projectRelation.OwnerEmail);
		}

		private (ExperienceSendableData Data, string Manager)? BuildUserProjectExperienceSendableData(GoRaceProjectExperience experience, User user)
		{
			var snapshots = dbContext.UserProjectStatusSnapshot
				.Where(s => s.ProjectId == experience.ProjectId && s.UserId == user.Id)
				.OrderByDescending(s => s.SnapshotDate)
				.Take(2)
				.ToList();

			if (snapshots.Count == 0)
				return null;

			var latest = snapshots[0];
			ProjectQualityValues latestValues = new(
				latest.Bugs,
				latest.Vulnerabilities,
				latest.CodeSmells,
				latest.Loc
			);
			QualityScore qualityScore = new(latestValues, experience.MaxQualityScore);
			ImprovementScore improvementScore;
			OnTimeTaskScore onTimeTaskScore = new(latest.OpenTasks - latest.OverdueTasks, latest.OpenTasks, experience.MaxOnTimeTasksScore);

			if (snapshots.Count == 2)
			{
				var previous = snapshots[1];
				ProjectQualityValues previousValues = new(
					previous.Bugs,
					previous.Vulnerabilities,
					previous.CodeSmells,
					previous.Loc
				);
				improvementScore = new ImprovementScore(latestValues, previousValues, experience.ImprovementScoreFactor);
			}
			else
			{
				improvementScore = new ImprovementScore(latestValues, latestValues, experience.ImprovementScoreFactor);
			}

			int closedTasks = latest.ClosedTasks;
			int mergedMergeRequest = latest.MergeRequestsMerged;

			var experienceSendableData = new ExperienceSendableData(
				qualityScore,
				improvementScore,
				onTimeTaskScore,
				closedTasks,
				mergedMergeRequest
			);

			return (experienceSendableData, user.Email);
		}

		private void SendExperienceDataAsync((ExperienceSendableData Data, string Manager) data, GoRaceExperience experience, string manager)
		{
			_ = SendProjectExperiencesData(data.Data, experience, data.Manager);
		}

		public async Task SendProjectExperiencesData(ExperienceSendableData experienceSendableData, GoRaceExperience experience, string manager)
		{
			if (experience.Token == null) return;

			var now = DateTime.Now;
			var client = GoRaceApiFactory.Create(experience.Url, experience.Token);
			GoRaceQualityRequest qualityRequest = new() { Assignment = "ACTIVITY", Email = manager, Quality = experienceSendableData.QualityScore.CalculateScore(), Time = now };
			GoRaceImprovementRequest improvementRequest = new() { Assignment = "ACTIVITY", Email = manager, Improvement = experienceSendableData.ImprovementScore.CalculateScore(), Time = now };
			GoRaceOnTimeTasksRequest onTimeTasksRequest = new() { Assignment = "ACTIVITY", Email = manager, OnTimeTask = experienceSendableData.OnTimeTaskScore.CalculateScore(), Time = now };
			GoRaceMergedMergeRequest mergedMergeRequest = new() { Assignment = "ACTIVITY2", Email = manager, MergedMergeRequest = experienceSendableData.MergedMergeRequest, Time = now };
			GoRaceClosedTasksRequest closedTasksRequest = new() { Assignment = "ACTIVITY2", Email = manager, ClosedTasks = experienceSendableData.ClosedTasks, Time = now };

			List<GoRaceDataRequestBase> requests = [qualityRequest, improvementRequest, onTimeTasksRequest, mergedMergeRequest, closedTasksRequest];

			await client.SendData(requests);
		}
	}
}
