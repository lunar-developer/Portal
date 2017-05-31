using Website.Library.DataTransfer;

namespace Modules.UserManagement.DataTransfer
{
    public class BranchRoleGroupData: CacheData
    {
        public string ID;
        public string BranchID;
        public string RoleGroupID;
        public string IsReadOnly;
        public string IsDisable;
        public string ModifyUserID;
        public string ModifyDateTime;
    }
}
