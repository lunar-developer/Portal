using Website.Library.DataTransfer;

namespace Modules.UserManagement.DataTransfer
{
    public class UserData : CacheData
    {
        public string UserID { get; set; }
        public string BranchID { get; set; }
    }
}