using MediatR;
using Remosys.Api.Core.Application.Auth.Dto;
using Remosys.Common.Result;

namespace Remosys.Api.Core.Application.Auth.Command.LoginCommand
{
    public class LoginCommand : IRequest<Result<TokenDto>>
    {
        public string Mobile { get; set; }
        public string Password { get; set; }
    }
}