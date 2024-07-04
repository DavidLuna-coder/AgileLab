namespace TFG.Model.Entities
{
    public class Project
    {
        public Guid Id { get; set; }
        public ICollection<Notification> Notifications { get; set; }
    }
}
