namespace TFG.Domain.Entities
{
	public class UserProject
	{
		public string UserId { get; set; }
		public User User { get; set; }

		public Guid ProjectId { get; set; }
		public Project Project { get; set; }

		public bool IsOwner { get; set; }
	}
}
