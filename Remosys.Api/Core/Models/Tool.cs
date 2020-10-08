using System;
using Remosys.Common.Enums;
using Remosys.Common.Types;

namespace Remosys.Api.Core.Models
{
    public class Tool : IIdentifiable
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Logo { get; set; }
        public ToolType ToolType { get; set; }
        public HarmfulType HarmfulType { get; set; }
        public bool IsActive { get; set; }

        public bool IsDeleted { get; set; }

        public virtual ToolsCategory ToolsCategory { get; set; }
    }
}