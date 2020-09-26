using System;
using MediatR;
using Remosys.Common.Result;

namespace Remosys.Api.Core.Application.Users.Command.DeleteUser
{
    public class DeleteUserCommand : IRequest<Result>
    {
        public Guid Id { get; set; }
    }
}