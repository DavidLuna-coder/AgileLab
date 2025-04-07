using System.Text.Json.Serialization;
using TFG.OpenProjectClient.Models.BasicObjects;

namespace TFG.OpenProjectClient.Models.WorkPackages
{
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
	}

	public class WorkPackageLinks
	{
		[JsonPropertyName("self")]
		public required Link Self { get; set; }

		[JsonPropertyName("schema")]
		public required Link Schema { get; set; }
		[JsonPropertyName("attachments")]
		public Link? Attachments { get; set; }

		[JsonPropertyName("author")]
		public Link? Author { get; set; }

		[JsonPropertyName("assignee")]
		public Link? Assignee { get; set; }

		[JsonPropertyName("project")]
		public required Link Project { get; set; }

		[JsonPropertyName("priority")]
		public required Link Priority { get; set; }

		[JsonPropertyName("status")]
		public required Link Status { get; set; }

		[JsonPropertyName("watchers")]
		public Link? Watchers { get; set; }

		[JsonPropertyName("relations")]
		public Link? Relations { get; set; }

		// Actions
		[JsonPropertyName("update")]
		public Link? Update { get; set; }

		[JsonPropertyName("delete")]
		public Link? Delete { get; set; }

		[JsonPropertyName("addComment")]
		public Link? AddComment { get; set; }

		[JsonPropertyName("addAttachment")]
		public Link? AddAttachment { get; set; }

		[JsonPropertyName("logTime")]
		public Link? LogTime { get; set; }

	}
}
