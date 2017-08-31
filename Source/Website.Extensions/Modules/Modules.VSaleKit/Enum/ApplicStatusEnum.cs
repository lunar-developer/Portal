using System.Collections.Generic;

namespace Modules.VSaleKit.Enum
{
    public class ApplicStatusEnum
    {
        public const string Init = "0";
        public const string WaitForApprove = "1";
        public const string Processing = "2";
        public const string Return = "3";
        public const string Recall = "4";
        public const string Close = "5";
        public const string Decline = "6";
        public const string Approved = "7";
        public const string Editing = "0A";

        private static readonly Dictionary<string, string> StatusDictionary = new Dictionary<string, string>()
        {
            { Init ,"Khởi tạo" },
            { WaitForApprove ,"Đang chờ duyệt" },
            { Processing ,"Đang xử lý"},
            { Return ,"Trả lại"},
            { Recall ,"Đã rút"},
            { Close ,"Đã đóng"},
            { Decline ,"Thẩm định từ chối"},
            { Approved ,"Đã phê duyệt"},
            { Editing   ,"Đang bổ sung"}
        };

        public static string GetDescription(string status)
        {
            return StatusDictionary.ContainsKey(status) ? StatusDictionary[status] : status;
        }
    }
}
