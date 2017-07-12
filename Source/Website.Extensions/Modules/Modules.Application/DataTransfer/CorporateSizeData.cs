using Website.Library.DataTransfer;

namespace Modules.Application.DataTransfer
{
    public class CorporateSizeData : CacheData
    {
        public string CorporateSizeCode { get; set; }
        public string CorporateSizeName { get; set; }
        public string IsDisable { get; set; }
    }
}
