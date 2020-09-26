namespace Remosys.Api.Core.Application.Auth.Dto
{
    public class AuthenticationResult : TokenDto
    {
        public bool IsSuccess { get; set; }

        public string Error { get; set; }
    }

    public class TokenDto
    {
        public string AccessToken { get; set; }

        public string RoleName { get; set; }

    }
}