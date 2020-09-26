using System;
using System.Collections.Generic;
using MediatR;
using Remosys.Common.Result;

namespace Remosys.Api.Core.Application.Departments.Command.Update
{
    public class UpdateDepartmentCommand : IRequest<Result>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public Guid? ManagerId { get; set; }

        public List<Guid> EmployeeIds { get; set; }

        public List<Guid> ToolCategoryIds { get; set; }
    }
}