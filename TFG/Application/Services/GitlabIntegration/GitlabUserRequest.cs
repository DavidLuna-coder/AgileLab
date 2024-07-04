namespace TFG.Application.Services.GitlabIntegration
{
    public struct GitlabUserRequest
    {
        public string Email { get; set; }
        public string Username { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
    }
}
