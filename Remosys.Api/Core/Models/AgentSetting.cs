using System;
using System.Collections.Generic;
using Remosys.Common.Enums;
using Remosys.Common.Types;

namespace Remosys.Api.Core.Models
{
    public class AgentSetting : IIdentifiable
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public ScreenShotSetting ScreenShotSetting { get; set; }
        public UserAccess UserAccess { get; set; }
        public TimeSetting TimeSetting { get; set; }
        public TaskSetting Task { get; set; }
        public UserInterAction UserInterAction { get; set; }
        public TimeTracking TimeTracking { get; set; }
        public DateTime CreateDate { get; set; }
        public bool IsDeleted { get; set; }
    
        public ICollection<Employee> Employees { get; set; }

    }

    public class ScreenShotSetting
    {
        public int ScreenShotPerHour { get; set; }
        public bool IsUserSeeInPanel { get; set; }
        public bool HasBlurPhoto { get; set; }
        public bool AllowDelete { get; set; }
    }

    public class UserAccess
    {
        public bool SeeAnalytic { get; set; }
        public bool SeeReport { get; set; }
    }

    public class TimeSetting
    {
        public TimeSpan FreeTime { get; set; }
        public TimeSpan IdleTime { get; set; }
        public TimeSpan CloseAgentInIdle { get; set; }
        public bool AllowOffline { get; set; }
        public bool ContinueToFree { get; set; }

    }

    public class TaskSetting
    {
        public bool CreateTask { get; set; }
        public bool HasWatcher { get; set; }

    }

    public class UserInterAction
    {
        public TimeSpan IdleTimeForReaction { get; set; }
        public int FailedInterAction { get; set; }
        public UserInterActionType UserActionType { get; set; }
    }

    public class TimeTracking
    {
        public TrackType TrackType { get; set; }

        public Day Days { get; set; }
    }
}