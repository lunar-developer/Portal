
namespace Modules.Disbursement.DataTransfer
{
    public class DisbursementData
    {
        public string DisbursementID;
        public string BranchID;
        public string CustomerID;
        public string CustomerName;
        public string CurrencyCode;
        public string Amount;
        public string DisbursementDate;
        public string DisbursementMethod;
        public string DisbursementPurpose;
        public string LoanMethod;
        public string CollectAmount;
        public string ProcessDate;
        public string DisbursementStatus;
        public string IsCancel;
        public string CancelUserID;
        public string CancelDateTime;
        public string CreateUserID;
        public string CreateDateTime;
        public string ModifyUserID;
        public string ModifyDateTime;
        public string ReviewUserID;
        public string ReviewDateTime;
        public string PreapproveUserID;
        public string PreapproveDateTime;
        public string ApproveUserID;
        public string ApproveDateTime;
        public string Remark;
        public string LoanExpire;
        public string CustomerType;
        public string InterestRate;
        public string Note;

        public bool Equals(DisbursementData data)
        {
            if (!string.IsNullOrEmpty(CustomerID) && !CustomerID.Equals(data.CustomerID))
            {
                return false;
            }

            if (!string.IsNullOrEmpty(CustomerName) && !CustomerName.Equals(data.CustomerName))
            {
                return false;
            }

            if (!string.IsNullOrEmpty(Amount) && decimal.Parse(Amount) != decimal.Parse(data.Amount))
            {
                return false;
            }

            if (!string.IsNullOrEmpty(CurrencyCode) && !CurrencyCode.Equals(data.CurrencyCode))
            {
                return false;
            }

            if (!string.IsNullOrEmpty(DisbursementDate) && !DisbursementDate.Equals(data.DisbursementDate))
            {
                return false;
            }

            if (!string.IsNullOrEmpty(DisbursementMethod) && !DisbursementMethod.Equals(data.DisbursementMethod))
            {
                return false;
            }

            if (!string.IsNullOrEmpty(DisbursementPurpose) && !DisbursementPurpose.Equals(data.DisbursementPurpose))
            {
                return false;
            }

            if (!string.IsNullOrEmpty(LoanExpire) && !LoanExpire.Equals(data.LoanExpire))
            {
                return false;
            }

            if (!string.IsNullOrEmpty(InterestRate) && !InterestRate.Equals(data.InterestRate))
            {
                return false;
            }

            if (!string.IsNullOrEmpty(CustomerType) && !CustomerType.Equals(data.CustomerType))
            {
                return false;
            }

            if (!string.IsNullOrEmpty(Note) && !Note.Equals(data.Note))
            {
                return false;
            }
            return string.IsNullOrEmpty(LoanMethod) || LoanMethod.Equals(data.LoanMethod);
        }
    }
}
