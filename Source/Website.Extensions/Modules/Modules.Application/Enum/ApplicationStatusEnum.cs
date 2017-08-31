using Modules.Application.Business;

namespace Modules.Application.Enum
{
    public static class ApplicationStatusEnum
    {
        public const string New00 = "0";
        public const string WaitForInput01 = "1";
        public const string Inputting02 = "2";
        public const string WaitForAccept03 = "3";
        public const string Accepting04 = "4";
        public const string WaitForValidate05 = "5";
        public const string Validating06 = "6";
        public const string WaitForAssess07 = "7";
        public const string Assessing08 = "8";
        public const string WaitForApprove09 = "9";
        public const string Approved10 = "10";
        public const string WaitForSupply11 = "11";
        public const string WaitForSupplyIncomplete12 = "12";
        public const string Incomplete13 = "13";
        public const string WaitForPreApprove14 = "14";
        public const string WaitForSupplyPreApprove15 = "15";
        public const string Closed = "-1";

        public static string GetStatusDescription(string applicationStatusID)
        {
            return ApplicationStatusBusiness.GetName(applicationStatusID);
        }
    }
}