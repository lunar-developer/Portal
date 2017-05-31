using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.WebControls;
using DotNetNuke.Security.Roles;
using DotNetNuke.UI.Skins.Controls;
using Modules.UserManagement.Business;
using Modules.UserManagement.Database;
using Modules.UserManagement.Global;
using Telerik.Web.UI;
using Website.Library.Database;
using Website.Library.DataTransfer;
using Website.Library.Enum;

namespace DesktopModules.Modules.UserManagement
{
    public partial class BranchPermission : UserManagementModuleBase
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
            ListItem item = new ListItem("Vui lòng chọn Chi Nhánh", string.Empty);
            item.Attributes.Add("disabled", "disabled");
            BindAllBranchData(ddlBranch, false, false, item);

            btnUpdate.Visible = btnRefresh.Visible = false;
            RegisterConfirmDialog(btnUpdate, "Bạn có chắc muốn cập nhật thay đổi?");
        }

        protected void LoadPermission(object sender, EventArgs e)
        {
            btnUpdate.Visible = btnRefresh.Visible = true;
            ListSource.Items.Clear();
            ListDestination.Items.Clear();

            string branchID = ddlBranch.SelectedValue;
            List<string> listRoleGroup = new List<string>();
            DataTable dtResult = BranchBusiness.GetBranchPermission(branchID);
            foreach (DataRow row in dtResult.Rows)
            {
                listRoleGroup.Add(row[RoleGroupTable.RoleGroupID].ToString());
            }
            foreach (RoleGroupInfo roleGroupInfo in RoleController.GetRoleGroups(PortalId))
            {
                string text = $"{roleGroupInfo.RoleGroupName} - {roleGroupInfo.Description}";
                string value = roleGroupInfo.RoleGroupID.ToString();
                RadListBoxItem item = new RadListBoxItem(text, value);
                if (listRoleGroup.Contains(roleGroupInfo.RoleGroupID.ToString()))
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
                { BranchTable.BranchID, new SQLParameterData(ddlBranch.SelectedValue, SqlDbType.Int) },
                { "ListRoleGroupID", new SQLParameterData(string.Join(",", list), SqlDbType.VarChar) },
                { BaseTable.ModifyUserID, new SQLParameterData(UserInfo.UserID, SqlDbType.Int) },
                {
                    BaseTable.ModifyDateTime,
                    new SQLParameterData(DateTime.Now.ToString(PatternEnum.DateTime), SqlDbType.BigInt)
                },
            };

            bool result = BranchBusiness.UpdateBranchPermission(parametedDictionary);
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
            LoadPermission(null, null);
        }
    }
}