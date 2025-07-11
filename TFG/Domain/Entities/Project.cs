﻿namespace TFG.Domain.Entities
{
    public class Project
    {
        public required Guid Id { get; set; }
        public required string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public ICollection<Notification> Notifications { get; set; } = [];
        public ICollection<User> Users { get; set; } = [];
		public DateTime CreatedAt { get; set; }
        public string GitlabId { get; set; }
        public int OpenProjectId { get; set; }
        public string SonarQubeProjectKey { get; set; }
        public ICollection<GoRaceProjectExperience> ProjectExperiences { get; set; }
        public ICollection<GoRacePlatformExperienceProject> PlatformExperiences { get; set; }
        public ICollection<ProjectStatusSnapshot> ProjectStatusSnapshots { get; set; }
        public ICollection<UserProjectStatusSnapshot> UserProjectStatusSnapshot { get; set; }
	}
}
