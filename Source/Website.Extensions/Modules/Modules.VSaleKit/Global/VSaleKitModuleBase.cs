using Modules.Application.Global;
using Modules.VSaleKit.Enum;
using Website.Library.Global;

namespace Modules.VSaleKit.Global
{
    public class VSaleKitModuleBase : ApplicationModuleBase
    {
        protected string FormatPriority(string value)
        {
            return value == "1" ? "Có" : "Không";
        }

        protected bool IsRoleInsert(int userId)
        {
            return IsInRole(RoleEnum.Collaboration, userId) || IsInRole(RoleEnum.Sale, userId);
        }


        private static string DetailUrl;
        protected string ApplicationUrl => DetailUrl ??
            (DetailUrl = FunctionBase.GetTabUrl(FunctionBase.GetConfiguration("VSK_EditUrl")) ?? string.Empty);
    }
}