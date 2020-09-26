using System;
using System.Collections.Generic;
using MediatR;
using Remosys.Api.Core.Application.Departments.Dto;
using Remosys.Common.Result;

namespace Remosys.Api.Core.Application.Departments.Command.Create
{
    public class CreateDepartmentCommand : IRequest<Result<DepartmentDto>>
    {
        public string Name { get; set; }

        public Guid OrganizationId { get; set; }

        public Guid? ManagerId { get; set; }

        public List<Guid> EmployeeIds { get; set; }

        public List<Guid> ToolCategoryIds { get; set; }
    }
}