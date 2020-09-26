using System;
using MediatR;
using Remosys.Common.Result;

namespace Remosys.Api.Core.Application.Organization.Command.Update
{
    public class UpdateOrganizationCommand : IRequest<Result>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public Guid EmployeeId { get; set; }
    }
}