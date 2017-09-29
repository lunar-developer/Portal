using Website.Library.DataTransfer;

namespace Modules.UserManagement.DataTransfer
{
    public class RegionData : CacheData
    {
        public string RegionID { get; set; }
        public string RegionName { get; set; }
        public string ParentRegionID { get; set; }
        public string IsDisable { get; set; }
    }
}