using System;

namespace Remosys.Api.Core.Application.Employees.Dto
{
    public class EmployeeDto
    {
        public Guid Id { get; set; }

        public string Code { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? StartContract { get; set; }
        public DateTime? EndContract { get; set; }
        public Guid UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
      
    }
}