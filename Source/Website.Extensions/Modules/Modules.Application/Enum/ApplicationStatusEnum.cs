using System.Collections.Generic;

namespace Modules.Application.Enum
{
    public static class ApplicationStatusEnum
    {
        public const string New = "1";
        public const string SendToInputTeam = "2";
        public const string Inputting = "3";
        public const string SendToCreditTeam = "4";
        public const string CreditAccepted = "5";
        public const string SendToValidateTeam = "6";
        public const string Validating = "7";
        public const string SendToAssessTeam = "8";
        public const string Assessing = "9";
        public const string SendToApprover = "10";
        public const string Approved = "11";
        public const string RequestDocument = "12";
        public const string WaitForPreApprove = "13";

        private static readonly Dictionary<string, string> StatusDictionary = new Dictionary<string, string>
        {
            { New, "Thêm Mới" },
            { SendToInputTeam, "Chuyển Nhập Liệu" },
            { Inputting, "Nhập Liệu" },
            { SendToCreditTeam, "Chuyển Tín Dụng" },
            { CreditAccepted, "Tín Dụng Nhận" },
            { SendToValidateTeam, "Chuyển Kiểm Tra" },
            { Validating, "Kiểm Tra" },
            { SendToAssessTeam, "Chuyển Thẩm Định" },
            { Assessing, "Thẩm Định" },
            { SendToApprover, "Chuyển Phê Duyệt" },
            { Approved, "Phê Duyệt" },
            { RequestDocument, "Yêu Cầu Bổ Sung" },
            { WaitForPreApprove, "Chờ Phê Duyệt (PreApprove)" }
        };

        public static string GetStatusDescription(string status)
        {
            return StatusDictionary.ContainsKey(status) ? StatusDictionary[status] : status;
        }
    }
}