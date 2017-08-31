using Website.Library.DataTransfer;

namespace Modules.Application.DataTransfer
{
    public class HomeOwnershipData : CacheData
    {
        public string HomeOwnershipCode { get; set; }
        public string HomeOwnershipName { get; set; }
        public string SortOrder { get; set; }
        public string IsDisable { get; set; }
    }
}