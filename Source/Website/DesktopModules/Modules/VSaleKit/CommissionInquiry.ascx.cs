using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI.WebControls;
using DotNetNuke.Entities.Users;
using Modules.VSaleKit.DataTransfer;
using Modules.VSaleKit.Global;
using Modules.VSaleKit.Business;
using Modules.VSaleKit.Enum;
using Modules.UserManagement.Global;
using Modules.UserManagement.Business;
using Modules.UserManagement.DataTransfer;
using Telerik.Web.UI;
using UserBusiness = Modules.UserManagement.Business.UserBusiness;

namespace DesktopModules.Modules.VSaleKit
{
    public partial class CommissionInquiry : VSaleKitModuleBase
    {
        protected override void OnLoad(EventArgs e)
        {
            if (IsPostBack)
            {
                return;
            }
            BindData();
        }
        protected void BindData()
        {
            RegisterConfirmDialog(btnRemove, "Bỏ người dùng?");
            UserManagementModuleBase.BindBranchData(ddlBranch, UserInfo.UserID.ToString(), false, false, new List<string>());
            ddlBranchUpdate();
        }

        protected void ddlBranch_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlBranchUpdate();
            gridAssignUser.Visible = false;
            btnRemove.Visible = false;
            btnAdd.Visible = false;
        }
        private void ddlBranchUpdate()
        {
            try
            {
                ddlUsers.Items.Clear();
                List<UserData> users = UserBusiness.GetUsersInBranch(int.Parse(ddlBranch.SelectedValue));

                foreach (UserData user in users)
                {
                    string roleName = GetRoleName(int.Parse(user.UserID));
                    if (roleName == "TDV" || roleName == "NVKD")
                    {
                        ddlUsers.Items.Add(new ListItem(user.UserName, user.UserID));
                    }
                }
            }
            catch (Exception ex)
            {
                ShowAlertDialog(ex.Message, "Lỗi");
            }
        }
        protected void btnRemove_Click(object sender, EventArgs e)
        {
            try
            {
                if (gridAssignUser.SelectedItems.Count == 0)
                {
                    ShowAlertDialog("Chưa chọn người dùng cần bỏ.", "Thông báo");
                    return;
                }
                SuperUserData user = new SuperUserData();

                user.UserName = gridAssignUser.SelectedItems[0].Cells[4].Text;
                user.ManagerID = ddlUsers.SelectedItem.Text;
                user.UserModif = UserInfo.Username.ToString();
                user.DateModif = DateTime.Now.ToString(Website.Library.Enum.PatternEnum.DateTime);

                DataTable dtResult = AssignUserBusiness.RemoveSuperUser(user);

                if (dtResult.Rows[0][0].ToString() == "0")
                {
                    ShowAlertDialog(dtResult.Rows[0][1].ToString(), "Thất bại");
                }
                else
                {
                    DataTable dt = ViewState["UserAssign"] as DataTable;
                    dt.Rows.RemoveAt(int.Parse(gridAssignUser.SelectedIndexes[0]));

                    gridAssignUser.DataSource = dt;
                    gridAssignUser.Rebind();
                    ViewState["UserAdd"] = dt;
                    ShowAlertDialog(dtResult.Rows[0][1].ToString(), "Thành công");
                }
            }
            catch (Exception ex)
            {
                ShowAlertDialog(ex.Message, "Lỗi");
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            List<string> listUser = new List<string>();
            List<UserData> users = UserBusiness.GetUsersInBranch(Int32.Parse(ddlBranch.SelectedValue));
            if (ddlUsers.Items.Count > 0)
            {
                btnAdd.Visible = true;
                DataTable dt = AssignUserBusiness.GetAssignUser(ddlUsers.SelectedItem.Text);

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    listUser.Add(dt.Rows[i][0].ToString());
                }

                DataTable dtAdded = new DataTable();
                dtAdded.Columns.Add("Mã");
                dtAdded.Columns.Add("Tên đăng nhập");
                dtAdded.Columns.Add("Tên người dùng");

                foreach (var user in users)
                {
                    if (listUser.Any(s => s.Contains(user.UserName)))
                    {
                        dtAdded.Rows.Add(user.UserID, user.UserName, user.DisplayName);
                    }
                }
                gridAssignUser.DataSource = dtAdded;
                gridAssignUser.Rebind();
                ViewState["UserAssign"] = dtAdded;
                gridAssignUser.Visible = true;
                btnRemove.Visible = true;
            }
            else
            {
                btnAdd.Visible = false;
                gridAssignUser.Visible = false;
            }
        }
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            string script = EditUrl("Edit", 800, 400, true, true, null, "UserID", ddlUsers.SelectedValue, "BranchID", ddlBranch.SelectedValue);
            RegisterScript(script);
        }
    }

}