using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using DotNetNuke.Security.Roles;
using DotNetNuke.UI.Skins.Controls;
using Modules.UserManagement.Business;
using Modules.UserManagement.Database;
using Modules.UserManagement.DataTransfer;
using Modules.UserManagement.Enum;
using Telerik.Web.UI;
using Website.Library.Database;
using Website.Library.Global;

namespace DesktopModules.Modules.UserManagement
{
    public partial class RoleSetting : DesktopModuleBase
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
            ArrayList listRoleGroup = RoleController.GetRoleGroups(0);
            ddlRoleGroup.Items.Add(new RadComboBoxItem("Tất Cả", string.Empty));
            foreach (RoleGroupInfo roleGroup in listRoleGroup)
            {
                string text = $"{roleGroup.RoleGroupName} - {roleGroup.Description}";
                string value = roleGroup.RoleGroupID.ToString();
                ddlRoleGroup.Items.Add(new RadComboBoxItem(text, value));
            }
        }

        protected void AddRoleSetting(object sender, EventArgs e)
        {
            string script = EditUrl(
                "Edit", 600, 400, true, true, "refresh", RoleExtensionTable.RoleGroupID, ddlRoleGroup.SelectedValue);
            RegisterScript(script);
        }

        protected void SearchRoleSetting(object sender, EventArgs e)
        {
            hidRoleGroupID.Value = ddlRoleGroup.SelectedValue;
            gridData.NeedDataSource += ProcessOnGridNeedDataSource;
            RefreshGrid();
        }

        protected void Refresh(object sender, EventArgs e)
        {
            RefreshGrid();
        }

        private void RefreshGrid()
        {
            gridData.DataSource = GetData();
            gridData.DataBind();
        }

        private List<RoleExtensionData> GetData()
        {
            return string.IsNullOrWhiteSpace(hidRoleGroupID.Value)
                ? CacheBase.Receive<RoleExtensionData>()
                : CacheBase.Filter<RoleExtensionData>(RoleExtensionTable.RoleGroupID, ddlRoleGroup.SelectedValue);
        }

        protected void ProcessOnGridNeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            gridData.DataSource = GetData();
        }

        protected void ProcessOnGridItemCommand(object source, GridCommandEventArgs e)
        {
            GridDataItem item = e.Item as GridDataItem;
            if (item == null)
            {
                return;
            }

            string roleID = item.GetDataKeyValue(BaseTable.RoleID).ToString();
            RoleExtensionData roleData = CacheBase.Receive<RoleExtensionData>(roleID);
            if (roleData == null)
            {
                return;
            }

            switch (e.CommandName)
            {
                case "DeleteCommand":
                    bool result = RoleExtensionBusiness.DeleteRoleInfo(roleID);
                    if (result)
                    {
                        ShowMessage($"Xóa cấu hình phân Quyền <b>{roleData.RoleName}</b> thành công.",
                            ModuleMessage.ModuleMessageType.GreenSuccess);
                        RefreshGrid();
                    }
                    else
                        ShowMessage($"Xóa cấu hình phân Quyền <b>{roleData.RoleName}</b> thất bại.",
                            ModuleMessage.ModuleMessageType.RedError);
                    break;


                default:
                    Dictionary<string, string> dictionary = new Dictionary<string, string>
                    {
                        { BaseTable.RoleID, roleID },
                        { RoleExtensionTable.RoleGroupID, roleData.RoleGroupID }
                    };
                    string url = EditUrl("Edit");
                    string script = EditUrl(url, 600, 400, true, dictionary, "refresh");
                    RegisterScript(script);
                    break;
            }
        }

        protected string FormatScope(string scope)
        {
            return RoleScopeEnum.GetScopeName(scope);
        }

        protected string FormatListExcludeRole(string listRoles)
        {
            if (string.IsNullOrWhiteSpace(listRoles))
            {
                return string.Empty;
            }

            string[] data = listRoles.Split(';');
            StringBuilder htmlBuilder =new StringBuilder();
            foreach (string item in data)
            {
                htmlBuilder.Append($"<span class=\"badge alert-danger c-margin-r-5\">{item}</span>");
            }
            return htmlBuilder.ToString();
        }
    }
}
