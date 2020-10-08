using System;
using MediatR;
using Remosys.Api.Core.Application.Tools.Dto;
using Remosys.Common.Enums;
using Remosys.Common.Result;

namespace Remosys.Api.Core.Application.Tools.Command.Create
{
    public class CreateToolCommand : IRequest<Result<ToolDto>>
    {
        public string Name { get; set; }
        public string Logo { get; set; }

        public ToolType ToolType { get; set; }
        public HarmfulType HarmfulType { get; set; }
        public Guid CategoryId { get; set; }
    }
}