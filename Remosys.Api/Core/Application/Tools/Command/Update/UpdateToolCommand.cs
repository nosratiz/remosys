using System;
using MediatR;
using Remosys.Common.Enums;
using Remosys.Common.Result;

namespace Remosys.Api.Core.Application.Tools.Command.Update
{
    public class UpdateToolCommand : IRequest<Result>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Logo { get; set; }
        public Guid CategoryId { get; set; }
        public ToolType ToolType { get; set; }
        public HarmfulType HarmfulType { get; set; }
    }
}