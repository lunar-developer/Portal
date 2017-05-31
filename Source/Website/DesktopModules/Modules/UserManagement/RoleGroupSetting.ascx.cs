using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.WebControls;
using DotNetNuke.Security.Roles;
using DotNetNuke.UI.Skins.Controls;
using Modules.UserManagement.Business;
using Modules.UserManagement.Database;
using Modules.UserManagement.DataTransfer;
using Modules.UserManagement.Global;
using Telerik.Web.UI;
using Website.Library.Database;
using Website.Library.DataTransfer;
using Website.Library.Enum;
using Website.Library.Global;

namespace DesktopModules.Modules.UserManagement
{
    public partial class RoleGroupSetting : UserManagementModuleBase
    {
        protected override void OnLoad(EventArgs e)
        {
            if (IsPostBack)
            {
                return;
            }

            BindData();
        }

        private void BindData()
        {
            ListItem item = new ListItem("Vui lòng chọn Nhóm Quyền", string.Empty);
            item.Attributes.Add("disabled", "disabled");
            ddlRoleGroup.Items.Add(item);
            foreach (RoleGroupInfo roleGroupInfo in RoleController.GetRoleGroups(PortalId))
            {
                string text = $"{roleGroupInfo.RoleGroupName} - {roleGroupInfo.Description}";
                string value = roleGroupInfo.RoleGroupID.ToString();
                ddlRoleGroup.Items.Add(new ListItem(text, value));
            }

            btnUpdate.Visible = btnRefresh.Visible = false;
            RegisterConfirmDialog(btnUpdate, "Bạn có chắc muốn cập nhật thay đổi?");
        }

        protected void LoadSettings(object sender, EventArgs e)
        {
            btnUpdate.Visible = btnRefresh.Visible = true;
            ListSource.Items.Clear();
            ListDestination.Items.Clear();

            string roleGroupID = ddlRoleGroup.SelectedValue;
            List<string> listBranchID = new List<string>();
            DataTable dtResult = RoleGroupBusiness.GetRoleGroupSetting(roleGroupID);
            foreach (DataRow row in dtResult.Rows)
            {
                listBranchID.Add(row[BranchTable.BranchID].ToString());
            }
            foreach (BranchData branchInfo in CacheBase.Receive<BranchData>())
            {
                if (branchInfo.BranchID == "-1")
                {
                    continue;
                }

                string text = $"{branchInfo.BranchCode} - {branchInfo.BranchName}";
                string value = branchInfo.BranchID;
                RadListBoxItem item = new RadListBoxItem(text, value);
                if (listBranchID.Contains(branchInfo.BranchID))
                {
                    ListDestination.Items.Add(item);
                }
                else
                {
                    ListSource.Items.Add(item);
                }
            }
        }

        protected void Save(object sender, EventArgs e)
        {
            List<string> list = new List<string>();
            foreach (RadListBoxItem item in ListDestination.Items)
            {
                list.Add(item.Value);
            }
            Dictionary<string, SQLParameterData> parametedDictionary = new Dictionary<string, SQLParameterData>
            {
                { RoleGroupTable.RoleGroupID, new SQLParameterData(ddlRoleGroup.SelectedValue, SqlDbType.Int) },
                { "ListBranchID", new SQLParameterData(string.Join(",", list), SqlDbType.VarChar) },
                { BaseTable.ModifyUserID, new SQLParameterData(UserInfo.UserID, SqlDbType.Int) },
                {
                    BaseTable.ModifyDateTime,
                    new SQLParameterData(DateTime.Now.ToString(PatternEnum.DateTime), SqlDbType.BigInt)
                },
            };

            bool result = RoleGroupBusiness.UpdateRoleGroupSetting(parametedDictionary);
            if (result)
            {
                ShowMessage("Cập nhật phân quyền thành công.", ModuleMessage.ModuleMessageType.GreenSuccess);
            }
            else
            {
                ShowMessage("Cập nhật phân quyền thất bại.", ModuleMessage.ModuleMessageType.RedError);
            }
        }

        protected void Refresh(object sender, EventArgs e)
        {
            LoadSettings(null, null);
        }
    }
}