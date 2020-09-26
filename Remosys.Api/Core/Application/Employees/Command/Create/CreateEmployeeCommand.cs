using System;
using MediatR;
using Remosys.Api.Core.Application.Employees.Dto;
using Remosys.Common.Result;

namespace Remosys.Api.Core.Application.Employees.Command.Create
{
    public class CreateEmployeeCommand : IRequest<Result<EmployeeDto>>
    {
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Mobile { get; set; }
        public string NationalCode { get; set; }
        public string Code { get; set; }
        public string Position { get; set; }
        public DateTime? StartContract { get; set; }
        public DateTime? EndContract { get; set; }
    }
}