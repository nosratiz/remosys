using System;
using MediatR;
using Remosys.Common.Result;

namespace Remosys.Api.Core.Application.Employees.Command.Update
{
    public class UpdateEmployeeCommand : IRequest<Result>
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Code { get; set; }
        public DateTime? StartContract { get; set; }
        public DateTime? EndContract { get; set; }
    }
}