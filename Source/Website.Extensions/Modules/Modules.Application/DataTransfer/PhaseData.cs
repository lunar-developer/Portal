using Website.Library.DataTransfer;

namespace Modules.Application.DataTransfer
{
    public class PhaseData: CacheData
    {
        public string PhaseID { get; set; }
        public string ApplicationStatus { get; set; }
        public string Name { get; set; }
        public string Remark { get; set; }
        public string IsDisable { get; set; }
    }
}