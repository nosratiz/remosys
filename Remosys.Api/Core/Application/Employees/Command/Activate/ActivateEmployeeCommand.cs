using System;
using MediatR;
using Remosys.Common.Result;

namespace Remosys.Api.Core.Application.Employees.Command.Activate
{
    public class ActivationEmployeeCommand : IRequest<Result>
    {
        public Guid EmployeeId { get; set; }
    }
}