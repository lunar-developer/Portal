using Website.Library.DataTransfer;

namespace Modules.Application.DataTransfer
{
    public class CustomerClassData : CacheData
    {
        public string CustomerClassCode { get; set; }
        public string Name { get; set; }
        public string IsDisable { get; set; }
    }
}