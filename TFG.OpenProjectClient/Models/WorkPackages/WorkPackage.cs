using System.Text.Json.Serialization;
using TFG.OpenProjectClient.Models.BasicObjects;

namespace TFG.OpenProjectClient.Models.WorkPackages
{
	public class WorkPackageLinks
	{
		[JsonPropertyName("status")]
		public Link? Status { get; set; }
	}

	public class WorkPackage : HalResource<WorkPackageLinks>
	{
		[JsonPropertyName("id")]
		public int Id { get; set; }

		[JsonPropertyName("lockVersion")]
		public int LockVersion { get; set; }

		[JsonPropertyName("subject")]
		public required string Subject { get; set; }

		[JsonPropertyName("description")]
		public FormattableText Description { get; set; }

		[JsonPropertyName("scheduleManually")]
		public bool ScheduleManually { get; set; }

		[JsonPropertyName("startDate")]
		public DateTime? StartDate { get; set; }

		[JsonPropertyName("dueDate")]
		public DateTime? DueDate { get; set; }

		[JsonPropertyName("createdAt")]
		public DateTime CreatedAt { get; set; }

		[JsonPropertyName("updatedAt")]
		public DateTime UpdatedAt { get; set; }

		[JsonPropertyName("percentageDone")]
		public int? PercentageDone { get; set; }

		// Helper para acceder al status
		public Link? Status => Links?.Status;
	}
}
