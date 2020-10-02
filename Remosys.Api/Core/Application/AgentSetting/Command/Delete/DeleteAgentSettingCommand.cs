using System;
using MediatR;
using Remosys.Common.Result;

namespace Remosys.Api.Core.Application.AgentSetting.Command.Delete
{
    public class DeleteAgentSettingCommand : IRequest<Result>
    {
        public Guid Id { get; set; }
    }
}