using System.Collections.Generic;

namespace Modules.UserManagement.Enum
{
    public static class RequestTypeEnum
    {
        public const string UpdateRoles = "1";
        public const string UpdateBranch = "2";

        private static readonly Dictionary<string, string> DescriptionDictionary =
            new Dictionary<string, string>
            {
                { UpdateRoles, "Cập nhật Quyền" },
                { UpdateBranch, "Chuyển Chi Nhánh" }
            };

        public static string GetDescription(string type)
        {
            return DescriptionDictionary.ContainsKey(type) ? DescriptionDictionary[type] : type;
        }
    }
}