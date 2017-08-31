using System.Collections.Generic;
using System.Reflection;
using System.Web.UI.WebControls;
using Modules.Disbursement.Enum;
using Modules.UserManagement.Global;
using Website.Library.Global;

namespace Modules.Disbursement.Global
{
    public class DisbursementModuleBase : DesktopModuleBase
    {
        protected void BindBranchData(DropDownList dropDownList, string userID)
        {
            if (IsAdministrator())
            {
                UserManagementModuleBase.BindAllBranchData(dropDownList, false , true);
            }
            else
            {
                UserManagementModuleBase.BindBranchData(dropDownList, userID);
            }
        }

        protected void BindStatusData(DropDownList dropDownList, params ListItem[] additionalItems)
        {
            if (additionalItems != null && additionalItems.Length > 0)
            {
                dropDownList.Items.AddRange(additionalItems);
            }
            foreach (FieldInfo fieldInfo in typeof(DisbursementStatusEnum).GetFields())
            {
                string value = fieldInfo.GetValue(null) + string.Empty; // Avoid exception
                string text = GetStatusDescription(value);
                dropDownList.Items.Add(new ListItem(text, value));
            }
        }

        protected void BindCurrencyData(DropDownList dropDownList, params ListItem[] additionalItems)
        {
            if (additionalItems != null && additionalItems.Length > 0)
            {
                dropDownList.Items.AddRange(additionalItems);
            }
            foreach (FieldInfo fieldInfo in typeof(CurrencyEnum).GetFields())
            {
                string text = fieldInfo.Name;
                string value = fieldInfo.GetValue(null) + string.Empty; // Avoid exception
                dropDownList.Items.Add(new ListItem(text, value));
            }
        }

        protected bool IsRoleInput()
        {
            return IsInRole(RoleEnum.Input);
        }

        protected bool IsRoleRevise()
        {
            return IsInRole(RoleEnum.Revise);
        }

        protected bool IsRoleALM()
        {
            return IsInRole(RoleEnum.Alm);
        }

        protected bool IsRoleApprove()
        {
            return IsInRole(RoleEnum.Approve);
        }

        protected bool IsAdministrator()
        {
            return IsRoleApprove() || IsRoleALM();// || 
        }

        protected string FormatCurrency(string currencyCode)
        {
            return currencyCode == CurrencyEnum.USD ? "USD" : "VND";
        }

        protected bool IsSensitiveStatus(string status)
        {
            return status == DisbursementStatusEnum.Preapproved || status == DisbursementStatusEnum.Approved ||
                status == DisbursementStatusEnum.Canceled;
        }


        private static readonly Dictionary<string, string> ActionDictionary = new Dictionary<string, string>
        {
            { DisbursementStatusEnum.New, "Thêm mới" },
            { DisbursementStatusEnum.Submited, "Chuyển TDV chi nhánh" },
            { DisbursementStatusEnum.Revised, "TDV chi nhánh duyệt" },
            { DisbursementStatusEnum.Preapproved, "Nhận xử lý" },
            { DisbursementStatusEnum.Approved, "SME duyệt" },
            { DisbursementStatusEnum.Disbursed, "Cập nhật kết quả" },

            { DisbursementStatusEnum.Rejected, "Từ chối yêu cầu" },

            { DisbursementStatusEnum.RequestCancel, "Yêu cầu Hủy" },
            { DisbursementStatusEnum.RequestApproved, "Duyệt yêu cầu Hủy" },
            { DisbursementStatusEnum.Canceled, "Hủy yêu cầu" },
            { DisbursementStatusEnum.Updated, "Điều chỉnh yêu cầu" },
            { DisbursementStatusEnum.ApplyResult, "Cập nhật kết quả" }
        };
        private static readonly Dictionary<string, string> StatusDictionary = new Dictionary<string, string>
        {
            { DisbursementStatusEnum.New, "Mới tạo" },
            { DisbursementStatusEnum.Submited, "Chờ TDV chi nhánh duyệt" },
            { DisbursementStatusEnum.Revised, "Đã chuyển đến Phòng SME" },
            { DisbursementStatusEnum.Preapproved, "Phòng SME đang xử lý" },
            { DisbursementStatusEnum.Approved, "Phòng SME đã duyệt" },
            { DisbursementStatusEnum.Disbursed, "Hoàn tất Giải Ngân" },

            { DisbursementStatusEnum.Rejected, "Bị từ chối" },

            { DisbursementStatusEnum.RequestCancel, "Yêu cầu Hủy" },
            { DisbursementStatusEnum.RequestApproved, "Chờ Phòng SME Hủy yêu cầu" },
            { DisbursementStatusEnum.Canceled, "Đã hủy" },
            { DisbursementStatusEnum.Updated, "Đã cập nhật" },
            { DisbursementStatusEnum.ApplyResult, "Cập nhật kết quả" }
        };

        protected string GetAction(string status)
        {
            return ActionDictionary.ContainsKey(status) ? ActionDictionary[status] : status;
        }

        protected static string GetStatusDescription(string status)
        {
            return StatusDictionary.ContainsKey(status) ? StatusDictionary[status] : status;
        }

        protected void RegisterButton(Button button, string status)
        {
            string action = GetAction(status);
            button.Text = action;
            button.CommandArgument = status;
            RegisterConfirmDialog(button, $"{action}?");
        }
    }
}