using System;
using MediatR;
using Remosys.Common.Result;

namespace Remosys.Api.Core.Application.ToolCategories.Command.Update
{
    public class UpdateCategoryCommand : IRequest<Result>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Logo { get; set; }
    }
}