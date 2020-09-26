using System;
using Remosys.Api.Core.Application.Employees.Dto;

namespace Remosys.Api.Core.Application.Organization.Dto
{
    public class OrganizationDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public DateTime CreateDate { get; set; }
        public virtual EmployeeDto Manager { get; set; }
    }
}