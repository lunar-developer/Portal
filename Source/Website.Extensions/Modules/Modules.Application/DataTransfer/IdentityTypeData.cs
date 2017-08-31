using Website.Library.DataTransfer;

namespace Modules.Application.DataTransfer
{
    public class IdentityTypeData : CacheData
    {
        public string IdentityTypeID { get; set; }
        public string IdentityTypeCode { get; set; }
        public string Name { get; set; }
        public string SortOrder { get; set; }
        public string IsDisable { get; set; }
    }
}