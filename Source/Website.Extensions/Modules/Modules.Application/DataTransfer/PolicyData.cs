using Website.Library.DataTransfer;

namespace Modules.Application.DataTransfer
{
    public class PolicyData : CacheData
    {
        public string PolicyID { get; set; }
        public string PolicyCode { get; set; }
        public string Name { get; set; }
        public string Remark { get; set; }
        public string IsDisable { get; set; }
    }
}