using Website.Library.DataTransfer;

namespace Modules.Application.DataTransfer
{
    public class CorporateEntityTypeData : CacheData
    {
        public string CorporateEntityTypeCode { get; set; }
        public string CorporateEntityTypeName { get; set; }
        public string IsDisable { get; set; }
    }
}