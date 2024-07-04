namespace Shared.DTOs
{
    public class RegistrationDto
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
        public required string UserName { get; set; }
    }
}
