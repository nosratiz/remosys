using System;
using MediatR;
using Remosys.Common.Result;

namespace Remosys.Api.Core.Application.Users.Command.UpdateUser
{
    public class UpdateUserCommand : IRequest<Result>
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Mobile { get; set; }
        public string NationalCode { get; set; }
        public string Avatar { get; set; }
        public Guid RoleId { get; set; }
    }
}