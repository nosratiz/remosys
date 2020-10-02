using System;
using System.Collections.Generic;
using MediatR;
using Remosys.Api.Core.Application.AgentSetting.Dto;
using Remosys.Api.Core.Models;
using Remosys.Common.Result;

namespace Remosys.Api.Core.Application.AgentSetting.Command.Create
{
    public class CreateAgentSettingCommand : IRequest<Result>
    {
        public string Name { get; set; }
        public ScreenShotSetting ScreenShotSetting { get; set; }
        public UserAccess UserAccess { get; set; }
        public TimeSettingDto TimeSetting { get; set; }
        public TaskSetting Task { get; set; }
        public UserInterActionDto UserInterAction { get; set; }
        public TimeTracking TimeTracking { get; set; }
        public List<Guid> EmployeeIds { get; set; }
    }
}