using Website.Library.DataTransfer;

namespace Modules.UserManagement.DataTransfer
{
    public class BranchData : CacheData
    {
        public string BranchID { get; set; }
        public string ParentID { get; set; }
        public string BranchCode { get; set; }
        public string BranchName { get; set; }
        public string LDAPCommonName { get; set; }
        public string RegionID { get; set; }
        public string IsHeadOffice { get; set; }
        public string IsLogical { get; set; }
        public string IsDisable { get; set; }
    }
}