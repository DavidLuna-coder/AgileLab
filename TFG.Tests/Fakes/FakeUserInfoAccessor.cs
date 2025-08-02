using TFG.Application.Security;

namespace TFG.Tests.Fakes;

public class FakeUserInfoAccessor : IUserInfoAccessor
{
    public IUserInfo? UserInfo { get; set; } = new FakeUserInfo();

    private class FakeUserInfo : IUserInfo
    {
        public string UserId => "fake-user-id";
        public string Email => "fakeuser@example.com";
        public string Username => "FakeUser";
        public bool IsAdmin => true;
    }
}