using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;
using Modules.Application.DataTransfer;
using Modules.Application.Business;
using Modules.VSaleKit.Global;
using Modules.VSaleKit.Business;
using Modules.VSaleKit.DataTransfer;
using Modules.UserManagement.Business;
using Modules.UserManagement.DataTransfer;
using Modules.VSaleKit.Enum;
using Website.Library.Global;
using UserBusiness = Modules.UserManagement.Business.UserBusiness;

namespace DesktopModules.Modules.VSaleKit
{
    public partial class AssignUser : VSaleKitModuleBase
    {
        protected override void OnLoad(EventArgs e)
        {
            if (IsPostBack)
            {
                return;
            }
            RegisterConfirmDialog(btnAdd, "Gán người dùng đã chọn?");
            BindData();
        }
        private void BindData()
        {

            string userID = Request["UserID"];
            string branchID = Request["BranchID"];

            Dictionary <string, UserData> dicUserAdd = GetListUserAdd(userID, branchID);

            DataTable dt = new DataTable();
            dt.Columns.Add("Mã");
            dt.Columns.Add("Tên đăng nhập");
            dt.Columns.Add("Tên hiển thị");

            foreach (var p in dicUserAdd)
            {
                dt.Rows.Add(p.Value.UserID, p.Value.UserName, p.Value.DisplayName);
            }
            gridUser.DataSource = dt;
            gridUser.Rebind();

            ViewState["UserAdd"] = dt;
        }
        private Dictionary<string, UserData> GetListUserAdd(string userID, string branchID)
        {
            Dictionary<string, UserData> dicAdd = new Dictionary<string, UserData>();
            try
            {
                string username = (UserBusiness.LoadUser(userID, userID)).Tables[0].Rows[0][1].ToString();
                DataTable dtResult = AssignUserBusiness.GetAssignUser(username);
                List<string> listNotAdd = new List<string>();
                

                for (int i = 0; i < dtResult.Rows.Count; i++)
                {
                    listNotAdd.Add(dtResult.Rows[i][0].ToString());
                }

                foreach (var p in UserBusiness.GetUsersInBranch(int.Parse(branchID)))
                {
                    string roleName = GetRoleName(int.Parse(userID));
                    string roleNameOfEditUser = GetRoleName(int.Parse(p.UserID));
                    if (roleName == "TDV" &&
                        roleNameOfEditUser == "NVKD" &&
                        !listNotAdd.Any(s => s.Contains(p.UserName)))
                    {
                        dicAdd.Add(p.UserID, p);
                    }
                    else if (roleName == "NVKD" &&
                            roleNameOfEditUser == "CTV" &&
                            !listNotAdd.Any(s => s.Contains(p.UserName)))
                    {
                        dicAdd.Add(p.UserID, p);
                    }
                }
                return dicAdd;
            }
            catch(Exception ex)
            {
                ShowAlertDialog(ex.Message, "Lỗi");
                return null;
            }
        }
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (gridUser.SelectedItems.Count == 0)
                {
                    ShowAlertDialog("Chưa chọn người dùng cần thêm.", "Thông báo");
                    return;
                }
                DataSet ds = UserBusiness.LoadUser(Request["UserID"], Request["UserID"]);
                SuperUserData user = new SuperUserData();
                user.UserName = gridUser.SelectedItems[0].Cells[4].Text;
                user.ManagerID = ds.Tables[0].Rows[0]["UserName"].ToString();
                user.UserCreate = UserInfo.Username.ToString();
                user.DateCreate = DateTime.Now.ToString(Website.Library.Enum.PatternEnum.DateTime);
                DataTable dtResult = AssignUserBusiness.AddSuperUser(user);
                if (dtResult.Rows[0][0].ToString() == "0")
                {
                    ShowAlertDialog(dtResult.Rows[0][1].ToString(), "Thất bại");
                }
                else
                {
                    DataTable dt = ViewState["UserAdd"] as DataTable;
                    dt.Rows.RemoveAt(int.Parse(gridUser.SelectedIndexes[0]));

                    gridUser.DataSource = dt;
                    gridUser.Rebind();
                    ViewState["UserAdd"] = dt;
                    ShowAlertDialog(dtResult.Rows[0][1].ToString(), "Thành công");
                }
            }
            catch(Exception ex)
            {
                ShowAlertDialog(ex.Message, "Lỗi");
            }
        }
    }
}