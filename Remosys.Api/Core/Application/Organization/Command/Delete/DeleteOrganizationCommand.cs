using System;
using MediatR;
using Remosys.Common.Result;

namespace Remosys.Api.Core.Application.Organization.Command.Delete
{
    public class DeleteOrganizationCommand : IRequest<Result>
    {
        public Guid Id { get; set; }
    }
}