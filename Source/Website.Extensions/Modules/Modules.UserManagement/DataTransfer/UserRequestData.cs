namespace Modules.UserManagement.DataTransfer
{
    public class UserRequestData
    {
        public string UserRequestID { get; set; }
        public string UserID { get; set; }
        public string BranchID { get; set; }
        public string NewBranchID { get; set; }
        public string RequestTypeID { get; set; }
        public string RequestPermission { get; set; }
        public string RequestReason { get; set; }
        public string RequestStatus { get; set; }
        public string Remark { get; set; }
        public string CreateUserID { get; set; }
        public string CreateDateTime { get; set; }
        public string ModifyUserID { get; set; }
        public string ModifyDateTime { get; set; }
        public string ProcessUserID { get; set; }
        public string ProcessDateTime { get; set; }
    }
}