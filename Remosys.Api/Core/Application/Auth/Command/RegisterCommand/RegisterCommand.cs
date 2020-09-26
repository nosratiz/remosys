using MediatR;
using Remosys.Api.Core.Application.Auth.Dto;
using Remosys.Api.Core.Application.Employees.Dto;
using Remosys.Common.Result;

namespace Remosys.Api.Core.Application.Auth.Command.RegisterCommand
{
    public class RegisterCommand : IRequest<Result<RegisterResult>>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Mobile { get; set; }
        public string NationalCode { get; set; }
        public string Password { get; set; }
    }
}