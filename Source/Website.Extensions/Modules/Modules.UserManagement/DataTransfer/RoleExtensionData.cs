using Website.Library.DataTransfer;

namespace Modules.UserManagement.DataTransfer
{
    public class RoleExtensionData : CacheData
    {
        public string RoleID { get; set; }
        public string RoleName { get; set; }
        public string RoleGroupID { get; set; }
        public string Description { get; set; }
        public string RoleScope { get; set; }
        public string ListExcludeRoleID { get; set; }
        public string IsDisable { get; set; }
    }
}