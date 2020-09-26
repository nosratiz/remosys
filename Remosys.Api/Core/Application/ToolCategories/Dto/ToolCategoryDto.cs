using System;

namespace Remosys.Api.Core.Application.ToolCategories.Dto
{
    public class ToolCategoryDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Logo { get; set; }

        public DateTime CreateDate { get; set; }
    }
}