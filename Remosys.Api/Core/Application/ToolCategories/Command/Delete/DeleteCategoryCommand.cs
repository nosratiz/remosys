using System;
using MediatR;
using Remosys.Common.Result;

namespace Remosys.Api.Core.Application.ToolCategories.Command.Delete
{
    public class DeleteCategoryCommand : IRequest<Result>
    {
        public Guid Id { get; set; }
    }
}