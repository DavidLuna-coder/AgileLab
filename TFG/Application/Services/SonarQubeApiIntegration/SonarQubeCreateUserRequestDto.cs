namespace TFG.Application.Services.SonarQubeApiIntegration
{
    public record SonarQubeCreateUserRequestDto
    {
        public string? Email { get; set; }
        public bool? Local { get;set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public List<string>? ScmAccounts { get; set; }
    }
}
