using System;
using System.Collections.Generic;
using Remosys.Common.Types;

namespace Remosys.Api.Core.Models
{
    public class ToolsCategory : IIdentifiable
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Logo { get; set; }

        public DateTime CreateDate { get; set; }
        public bool IsDeleted { get; set; }

        public virtual ICollection<Tool> Tools { get; set; }
    }
}