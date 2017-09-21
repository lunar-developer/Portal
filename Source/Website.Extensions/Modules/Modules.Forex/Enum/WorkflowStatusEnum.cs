namespace Modules.Forex.Enum
{
    public class WorkflowStatusEnum
    {
        public const int BRAsk = 1;
        public const int HOReceiveRequest = 2;
        public const int HOBid = 3;
        public const int BRReceive = 4;
        public const int Timeout = 5;
        public const int BRInputCustomerInvoiceAmount = 6;
        public const int BRRquestException = 7;
        public const int BRApprovalException = 8;
        public const int HOReceiveBrokerage = 9;
        public const int HOInputBrokerage = 10;
        public const int BRApprovalTransaction = 11;
        public const int HOFinishTransaction = 12;

        public const int BRRequestEdit = 13;
        public const int BRApprovalEdit = 14;
        public const int HOApprovalEdit= 15;
        public const int HORequestEditException = 16;
        public const int HOApprovalEditException = 17;
        public const int HOFinishEdit = 18;

        public const int BRRequestCancel = 19;
        public const int BRApprovalCancel = 20;
        public const int HOApprovalCancel = 21;
        public const int HORequestCancelException = 22;
        public const int HOApprovalCancelException = 23;
        public const int HOFinishCancel = 24;

        public const int Reject = -1;
        public const int Open = 0;
    }
}
