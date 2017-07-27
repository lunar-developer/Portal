using System.Collections.Generic;

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
        public const string Incomplete12 = "12";
        public const string WaitForPreApprove13 = "13";
        public const string Closed = "-1";

        private static readonly Dictionary<string, string> StatusDictionary = new Dictionary<string, string>
        {
            { New00, "MỚI" },
            { WaitForInput01, "CHUYỂN NHẬP LIỆU" },
            { Inputting02, "ĐANG NHẬP LIỆU" },
            { WaitForAccept03, "CHUYỂN TÍN DỤNG" },
            { Accepting04, "TÍN DỤNG NHẬN" },
            { WaitForValidate05, "CHUYỂN KIỂM TRA" },
            { Validating06, "ĐANG KIỂM TRA" },
            { WaitForAssess07, "CHUYỂN THẨM ĐỊNH" },
            { Assessing08, "ĐANG THẨM ĐỊNH" },
            { WaitForApprove09, "CHUYỂN PHÊ DUYỆT" },
            { Approved10, "ĐÃ DUYỆT" },
            { WaitForSupply11, "CHỜ BỔ SUNG" },
            { Incomplete12, "CHỜ BỔ SUNG (INCOMPLETE)" },
            { WaitForPreApprove13, "CHỜ PHÊ DUYỆT(PREAPPROVE)" },
            { Closed, "ĐÓNG" }
        };

        public static string GetStatusDescription(string status)
        {
            return StatusDictionary.ContainsKey(status) ? StatusDictionary[status] : status;
        }
    }
}