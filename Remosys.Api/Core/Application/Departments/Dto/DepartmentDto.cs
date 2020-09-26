using System;
using System.Collections.Generic;
using Remosys.Api.Core.Application.Employees.Dto;
using Remosys.Api.Core.Application.ToolCategories.Dto;

namespace Remosys.Api.Core.Application.Departments.Dto
{
    public class DepartmentDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime CreateDate { get; set; }

        public virtual EmployeeDto Manager { get; set; }

        public virtual ICollection<EmployeeDto> Employees { get; set; }
        public virtual ICollection<ToolCategoryDto> ToolsCategories { get; set; }
    }
}