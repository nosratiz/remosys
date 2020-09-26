using MediatR;
using Remosys.Api.Core.Application.ToolCategories.Dto;
using Remosys.Common.Result;

namespace Remosys.Api.Core.Application.ToolCategories.Command.Create
{
    public class CreateCategoryCommand : IRequest<Result<ToolCategoryDto>>
    {
        public string Name { get; set; }
        public string Logo { get; set; }
    }
}