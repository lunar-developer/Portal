using Website.Library.DataTransfer;

namespace Modules.Application.DataTransfer
{
    public class RouteData : CacheData
    {
        public string RouteID { get; set; }
        public string ProcessID { get; set; }
        public string PhaseID { get; set; }
        public string TargetPhaseID { get; set; }
    }
}