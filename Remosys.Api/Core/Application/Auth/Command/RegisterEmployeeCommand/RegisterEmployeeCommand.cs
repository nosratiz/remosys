using MediatR;
using Remosys.Api.Core.Application.Auth.Dto;
using Remosys.Common.Result;

namespace Remosys.Api.Core.Application.Auth.Command.RegisterEmployeeCommand
{
    public class RegisterEmployeeCommand : IRequest<Result<RegisterResult>>
    {
        public string Mobile { get; set; }

    }
}