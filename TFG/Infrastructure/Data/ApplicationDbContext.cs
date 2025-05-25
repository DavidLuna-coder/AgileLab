using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TFG.Domain.Entities;
using TFG.Model.Entities;

namespace TFG.Infrastructure.Data
{
	public class ApplicationDbContext : IdentityDbContext<User>
	{
		public DbSet<User> Users { get; set; }
		public DbSet<Project> Projects { get; set; }
		public DbSet<Notification> Notifications { get; set; }
		public DbSet<Rol> Roles { get; set; }
		public DbSet<ApiConfiguration> ApiConfigurations { get; set; }
		//public DbSet<Template> Templates { get; set; }
		public ApplicationDbContext(DbContextOptions options) : base(options)
		{
		}
		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder
				.UseSeeding((context, _) =>
				{
					SeedConfig(context);
				})
				.UseAsyncSeeding(async (context, _, cancellationToken) =>
				{
					await SeedConfigAsync(context);
				});
			base.OnConfiguring(optionsBuilder);
		}

		private void SeedConfig(DbContext context)
		{
			var gitlabConfig = context.Set<ApiConfiguration>().FirstOrDefault(c => c.Type == ApiConfigurationType.Gitlab);
			if (gitlabConfig == null)
			{
				context.Set<ApiConfiguration>().Add(new ApiConfiguration
				{
					Type = ApiConfigurationType.Gitlab,
					Name = "Gitlab",
					BaseUrl = string.Empty,
					Token = string.Empty
				});
			}

			var openProjectConfig = context.Set<ApiConfiguration>().FirstOrDefault(c => c.Type == ApiConfigurationType.OpenProject);
			if (openProjectConfig == null)
			{
				context.Set<ApiConfiguration>().Add(new ApiConfiguration
				{
					Type = ApiConfigurationType.OpenProject,
					Name = "OpenProject",
					BaseUrl = string.Empty,
					Token = string.Empty
				});
			}

			var sonarQubeConfig = context.Set<ApiConfiguration>().FirstOrDefault(c => c.Type == ApiConfigurationType.SonarQube);
			if (sonarQubeConfig == null)
			{
				context.Set<ApiConfiguration>().Add(new ApiConfiguration
				{
					Type = ApiConfigurationType.SonarQube,
					Name = "SonarQube",
					BaseUrl = string.Empty,
					Token = string.Empty
				});
			}
			context.SaveChanges();
		}

		private async Task SeedConfigAsync(DbContext context)
		{
			var gitlabConfig = await context.Set<ApiConfiguration>().FirstOrDefaultAsync(c => c.Type == ApiConfigurationType.Gitlab);
			if (gitlabConfig == null)
			{
				context.Set<ApiConfiguration>().Add(new ApiConfiguration
				{
					Type = ApiConfigurationType.Gitlab,
					Name = "Gitlab",
					BaseUrl = string.Empty,
					Token = string.Empty
				});
			}

			var openProjectConfig = context.Set<ApiConfiguration>().FirstOrDefaultAsync(c => c.Type == ApiConfigurationType.OpenProject);
			if (openProjectConfig == null)
			{
				context.Set<ApiConfiguration>().Add(new ApiConfiguration
				{
					Type = ApiConfigurationType.OpenProject,
					Name = "OpenProject",
					BaseUrl = string.Empty,
					Token = string.Empty
				});
			}

			var sonarQubeConfig = context.Set<ApiConfiguration>().FirstOrDefaultAsync(c => c.Type == ApiConfigurationType.SonarQube);
			if (sonarQubeConfig == null)
			{
				context.Set<ApiConfiguration>().Add(new ApiConfiguration
				{
					Type = ApiConfigurationType.SonarQube,
					Name = "SonarQube",
					BaseUrl = string.Empty,
					Token = string.Empty
				});
			}
			await context.SaveChangesAsync();
		}
	}
}
