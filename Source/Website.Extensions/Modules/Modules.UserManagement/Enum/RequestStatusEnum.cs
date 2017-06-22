using System.Collections.Generic;

namespace Modules.UserManagement.Enum
{
    public static class RequestStatusEnum
    {
        public const string New = "0";
        public const string Approve = "1";
        public const string Decline = "2";
        public const string Cancel = "3";

        private static readonly Dictionary<string, string> DescriptionDictionary =
            new Dictionary<string, string>
            {
                { New , "Mới tạo" },
                { Approve, "Đã phê duyệt" },
                { Decline, "Bị từ chối" },
                { Cancel, "Đã huỷ" },
            };

        public static string GetDescription(string type)
        {
            return DescriptionDictionary.ContainsKey(type) ? DescriptionDictionary[type] : type;
        }
    }
}