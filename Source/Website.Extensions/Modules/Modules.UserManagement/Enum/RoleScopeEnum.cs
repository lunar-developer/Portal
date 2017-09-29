using Website.Library.Extension;

namespace Modules.UserManagement.Enum
{
    public static class RoleScopeEnum
    {
        public const string Both = "HB";
        public const string HeadOffice = "H";
        public const string BranchOffice = "B";

        private static readonly InsensitiveDictionary<string> DescriptionDictionary = new InsensitiveDictionary<string>
        {
            { HeadOffice, "Hội Sở" },
            { BranchOffice, "Chi Nhánh" },
            { Both, "Hội Sở & Chi Nhánh" },
        };

        public static string GetScopeName(string scope)
        {
            return DescriptionDictionary.GetValue(scope);
        }
    }
}