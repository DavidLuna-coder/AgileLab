namespace TFG.Application.Services.OpenProjectIntegration
{
    public struct OpenProjectCreateUserRequest
    {
        public bool Admin { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Login { get; set; } // This is the UserName
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Language { get; set; } // User's language | ISO 639-1 format
    }
}
