using Website.Library.DataTransfer;

namespace Modules.Forex.DataTransfer
{
    public class WorkflowStatusData : CacheData
    {
        public string ID;
        public string Status;
        public string TargetStatus;
        public string RequestTypeID;
        public string Title;
        public string Description;
        public string IsDisable;
    }
}
