using Microsoft.AspNetCore.Identity;

namespace TFG.Domain.Entities
{
    public class User : IdentityUser
    {
        public bool IsAdmin { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string GitlabId { get; set; }
        public string OpenProjectId { get; set; }
        public string SonarQubeId { get; set; }
        public ICollection<Project> Projects { get; set; }
        public ICollection<Rol> Roles { get; set; }
    }
}
