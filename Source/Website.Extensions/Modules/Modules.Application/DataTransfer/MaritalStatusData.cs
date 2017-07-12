using Website.Library.DataTransfer;

namespace Modules.Application.DataTransfer
{
    public class MaritalStatusData : CacheData
    {
        public string MaritalStatusCode { get; set; }
        public string MaritalStatusName { get; set; }
        public string IsDisable { get; set; }
    }
}