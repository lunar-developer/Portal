using Website.Library.DataTransfer;

namespace Modules.UserManagement.DataTransfer
{
    public class UserData : CacheData
    {
        public string UserID { get; set; }
        public string UserName { get; set; }
        public string DisplayName { get; set; }
        public string BranchID { get; set; }
        public string StaffID { get; set; }
        public string Mobile { get; set; }
        public string PhoneExtension { get; set; }
    }
}