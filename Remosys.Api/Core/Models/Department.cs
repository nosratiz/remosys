using System;
using System.Collections.Generic;
using Remosys.Common.Types;

namespace Remosys.Api.Core.Models
{
    public class Department : IIdentifiable
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public DateTime CreateDate { get; set; }
        public bool IsDeleted { get; set; }

        public Organization Organization { get; set; }

        public virtual Employee Manager { get; set; }
        public virtual ICollection<Employee> Employees { get; set; }
        public virtual ICollection<ToolsCategory> ToolsCategories { get; set; }

    }
}