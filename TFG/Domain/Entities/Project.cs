namespace TFG.Model.Entities
{
    public class Project
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; } = string.Empty;
        public ICollection<Notification> Notifications { get; set; }
        public ICollection<User> Users { get; set; }
		public DateTime CreatedAt { get; set; }
	}
}
