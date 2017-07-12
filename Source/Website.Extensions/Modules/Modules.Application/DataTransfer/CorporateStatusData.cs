using Website.Library.DataTransfer;

namespace Modules.Application.DataTransfer
{
    public class CorporateStatusData : CacheData
    {
        public string CorporateStatusCode { get; set; }
        public string CorporateStatusName { get; set; }
        public string IsDisable { get; set; }
    }
}
