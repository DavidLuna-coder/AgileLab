namespace TFG.Application.Services.SonarQubeIntegration
{
    public record SonarQubeCreateUserRequestDto
    {
        public string? Email { get; set; }
        public bool? Local { get;set; }
        public string Login { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public List<string>? ScmAccounts { get; set; }
    }
}
