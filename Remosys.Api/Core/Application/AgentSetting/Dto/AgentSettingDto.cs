using System;
using System.Collections.Generic;
using Remosys.Api.Core.Application.Employees.Dto;
using Remosys.Api.Core.Application.Organization.Dto;
using Remosys.Api.Core.Application.Users.Dto;
using Remosys.Api.Core.Models;
using Remosys.Common.Enums;

namespace Remosys.Api.Core.Application.AgentSetting.Dto
{
    public class AgentSettingDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public ScreenShotSetting ScreenShotSetting { get; set; }
        public UserAccess UserAccess { get; set; }
        public TimeSetting TimeSetting { get; set; }
        public TaskSetting Task { get; set; }
        public UserInterAction UserInterAction { get; set; }
        public TimeTracking TimeTracking { get; set; }
        public OrganizationDto Organization { get; set; }
        public List<EmployeeDto> Employees { get; set; }
    }

    public class TimeSettingDto
    {
        public string FreeTime { get; set; }
        public string IdleTime { get; set; }
        public string CloseAgentInIdle { get; set; }
        public bool AllowOffline { get; set; }
        public bool ContinueToFree { get; set; }
    }

    public class UserInterActionDto
    {
        public string IdleTimeForReaction { get; set; }
        public int FailedInterAction { get; set; }
        public UserInterActionType UserActionType { get; set; }
    }
}