using System.Collections.Generic;

namespace Remosys.Api.Core.Models
{
    public class AgentActivitySchema
    {
        public int SnapshotId { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public int Duration { get; set; }
        public MonitorDatas MonitorDatas { get; set; }
    }
    public class ActiveWindow
    {
        public string Title { get; set; }
        public string Process { get; set; }
        public int Duration { get; set; }
    }

    public class OpenWindow
    {
        public string Title { get; set; }
        public string Process { get; set; }
        public int Duration { get; set; }
    }

    public class Url
    {
        public string Title { get; set; }
        public string UtcTime { get; set; }
        public string url { get; set; }
    }

    public class Clock
    {
        public string Type { get; set; }
        public string UtcTime { get; set; }
    }

    public class ScreenShot
    {
        public string Base64 { get; set; }
        public string UtcTime { get; set; }
    }

    public class MonitorDatas
    {
        public List<ActiveWindow> ActiveWindows { get; set; }
        public List<OpenWindow> OpenWindows { get; set; }
        public List<Url> Urls { get; set; }
        public List<Clock> Clock { get; set; }
        public List<ScreenShot> Screenshots { get; set; }
    }

}