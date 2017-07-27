using Website.Library.DataTransfer;

namespace Modules.Application.DataTransfer
{
    public class ScheduleData : CacheData
    {
        public string ScheduleName { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string Period { get; set; }
        public string IsDisable { get; set; }
    }
}