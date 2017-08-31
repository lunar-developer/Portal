using System.Collections.Generic;
using System.Linq;
using Modules.Application.DataTransfer;
using Modules.Application.Global;
using Modules.VSaleKit.Business;
using Modules.VSaleKit.DataTransfer;
using Modules.VSaleKit.Enum;
using Telerik.Web.UI;
using Website.Library.Global;

namespace Modules.VSaleKit.Global
{
    public class VSaleKitModuleBase : ApplicationModuleBase
    {
        private static readonly string TemplateUrl =
            FunctionBase.GetAbsoluteUrl("/DesktopModules/Modules/VSaleKit/Assets/Template/Template_BatchUpload.xlsx");

        protected string LinkTemplate => TemplateUrl;


        private List<PermissionData> Permissions;
        protected List<PermissionData> UserPermission
        {
            get
            {
                if (Session["UserPermission"] == null)
                {
                    Permissions = UserBusiness.GetUserPermission(GetRoleName());
                    Session["UserPermission"] = Permissions;
                }
                // ReSharper disable once ConvertIfStatementToNullCoalescingExpression
                if (Permissions == null)
                {
                    Permissions = (List<PermissionData>)Session["UserPermission"];
                }
                return Permissions;
            }
        }

        protected string GetRoleName(int userID = 0)
        {
            if (userID == 0)
            {
                userID = UserInfo.UserID;
            }

            if (IsInRole(RoleEnum.Collaboration, userID))
            {
                return RoleEnum.Collaboration;
            }
            if (IsInRole(RoleEnum.Sale, userID))
            {
                return RoleEnum.Sale;
            }
            if (IsInRole(RoleEnum.Leader, userID))
            {
                return RoleEnum.Leader;
            }
            if (IsInRole(RoleEnum.Manager, userID))
            {
                return RoleEnum.Manager;
            }
            return IsInRole(RoleEnum.Director) ? RoleEnum.Director : null;
        }

        protected bool IsRoleLeader(string roleName)
        {
            return roleName == RoleEnum.Leader;
        }

        protected bool IsRoleInsert(int userID)
        {
            return IsInRole(RoleEnum.Collaboration, userID) || IsInRole(RoleEnum.Sale, userID) || IsRoleInput(userID);
        }


        protected bool IsHasPermission(string action)
        {
            PermissionData permission = UserPermission.FirstOrDefault(item => item.ActionName == action);
            return permission != null;
        }

        protected string FormatPriority(string value)
        {
            return value == "1" ? "Có" : "Không";
        }

        protected string FormatAction(string status)
        {
            string msg;
            switch (status)
            {
                case "0":
                    msg = "Tạo hồ sơ.";
                    break;

                case "0A":
                    msg = "Cập nhật hồ sơ.";
                    break;

                case "1":
                    msg = "Trình hồ sơ.";
                    break;

                case "3":
                    msg = "Trả lại hồ sơ.";
                    break;

                case "4":
                    msg = "Rút hồ sơ.";
                    break;
                case "5":
                    msg = "Đóng hồ sơ.";
                    break;
                case "6":
                    msg = "Từ chối hồ sơ.";
                    break;
                case "7":
                    msg = "Duyệt hồ sơ.";
                    break;

                default:
                    msg = string.Empty;
                    break;
            }
            return msg;
        }

        private static string DetailUrl;
        protected string ApplicationUrl => DetailUrl ??
            (DetailUrl = FunctionBase.GetTabUrl(FunctionBase.GetConfiguration("VSK_EditUrl")) ?? string.Empty);


        protected static void LoadApplicationTypeData(RadComboBox dropDownList)
        {
            foreach (ApplicationTypeData cacheData in CacheBase.Receive<ApplicationTypeData>())
            {
                dropDownList.Items.Add(
                    CreateItem(cacheData.Name, cacheData.ApplicationTypeCode, cacheData.IsDisable));
            }
        }

        protected static void LoadPolicyData(RadComboBox dropDownList)
        {
            foreach (PolicyData cacheData in CacheBase.Receive<PolicyData>())
            {
                string text = $"{cacheData.PolicyCode} - {cacheData.Name}";
                string value = cacheData.PolicyID;
                dropDownList.Items.Add(CreateItem(text, value, cacheData.IsDisable));
            }
        }
    }
}