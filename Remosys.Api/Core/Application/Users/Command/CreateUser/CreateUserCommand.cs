using System;
using MediatR;
using Remosys.Api.Core.Application.Users.Dto;
using Remosys.Common.Result;

namespace Remosys.Api.Core.Application.Users.Command.CreateUser
{
    public class CreateUserCommand : IRequest<Result<UserDto>>
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Mobile { get; set; }
        public string NationalCode { get; set; }

        public string Avatar { get; set; }

    }
}