using System;
using MediatR;
using Remosys.Common.Result;

namespace Remosys.Api.Core.Application.Employees.Command.Delete
{
    public class DeleteEmployeeCommand : IRequest<Result>
    {
        public Guid Id { get; set; }
    }
}