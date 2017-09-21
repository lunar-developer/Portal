namespace Modules.UserManagement.DataTransfer
{
    public class UserRequestData
    {
        public string UserRequestID { get; set; }
        public string UserID { get; set; }
        public string BranchID { get; set; }
        public string NewBranchID { get; set; }
        public string RequestTypeID { get; set; }
        public string CurrentPermission { get; set; }
        public string RequestPermission { get; set; }
        public string RequestReason { get; set; }
        public string RequestStatus { get; set; }
        public string Remark { get; set; }
        public string UserIDCreate { get; set; }
        public string DateTimeCreate { get; set; }
        public string UserIDModify { get; set; }
        public string DateTimeModify { get; set; }
        public string UserIDProcess { get; set; }
        public string DateTimeProcess { get; set; }
    }
}