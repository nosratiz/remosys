using System;
using Remosys.Api.Core.Application.ToolCategories.Dto;

namespace Remosys.Api.Core.Application.Tools.Dto
{
    public class ToolDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Logo { get; set; }

        public virtual ToolCategoryDto ToolsCategory { get; set; }
    }
}