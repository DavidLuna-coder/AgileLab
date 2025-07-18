﻿namespace Shared.DTOs
{
    public record RegistrationDto
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
        public required string UserName { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public string Language { get; set; } = "es";
    }
}
