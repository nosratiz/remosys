using MediatR;
using Remosys.Api.Core.Application.Auth.Dto;
using Remosys.Common.Result;

namespace Remosys.Api.Core.Application.Auth.Command.ConfirmCode
{
    public class ConfirmCodeCommand : IRequest<Result<TokenDto>>
    {
        public string Code { get; set; }

        public string Mobile { get; set; }
    }
}