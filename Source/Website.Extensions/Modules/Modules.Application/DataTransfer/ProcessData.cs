using Website.Library.DataTransfer;

namespace Modules.Application.DataTransfer
{
    public class ProcessData : CacheData
    {
        public string ProcessID { get; set; }
        public string Name { get; set; }
        public string Remark { get; set; }
        public string IsDisable { get; set; }
    }
}