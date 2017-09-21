using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using DotNetNuke.UI.Skins.Controls;
using Modules.Application.Business;
using Modules.Application.Database;
using Modules.Application.DataTransfer;
using Modules.Application.Enum;
using Modules.Application.Global;
using Modules.UserManagement.DataTransfer;
using Telerik.Web.UI;
using Website.Library.Database;
using Website.Library.DataTransfer;
using Website.Library.Enum;
using Website.Library.Global;

namespace DesktopModules.Modules.Application
{
    public partial class UserAssignmentEditor : ApplicationModuleBase
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
            string phaseID = Request[UserPhaseTable.PhaseID];
            string userID = Request[UserPhaseTable.UserID];
            if (string.IsNullOrWhiteSpace(phaseID))
            {
                phaseID = PhaseEnum.Inputting;
            }
            LoadUserData(phaseID, userID);
        }

        public void LoadUserData(string phaseID, string userUniqueID)
        {
            ResetData();

            int userID;
            bool isEditMode = int.TryParse(userUniqueID, out userID) && userID > 0;
            hidUserID.Value = userID.ToString();
            hidPhaseID.Value = phaseID;
            lblPhase.Text = FormatPhase(phaseID);

            if (isEditMode)
            {
                string key = UserPhaseBusiness.GetCacheKey(phaseID, userID.ToString());
                UserPhaseData userPhaseData = CacheBase.Receive<UserPhaseData>(key);
                UserData userData = CacheBase.Receive<UserData>(userID.ToString());
                if (userData == null || userPhaseData == null)
                {
                    ShowMessage("Không tìm thấy thông tin User xử lý.", ModuleMessage.ModuleMessageType.RedError);
                    return;
                }

                ddlUser.Items.Add(CreateItem(userData));
                ddlUser.SelectedIndex = 0;
                txtKPI.Text = userPhaseData.KPI.ToString();
                ddlIsDisable.SelectedValue = userPhaseData.IsDisable ? "1" : "0";
                BindListPolicy(ListSource, ListDestination, userPhaseData.GetListPolicyCode());
            }
            else
            {
                List<int> listExcludeUser = CacheBase
                    .Filter<UserPhaseData>(UserPhaseTable.PhaseID, phaseID)
                    .Select(item => item.UserID).ToList();

                List<UserData> listUsers = phaseID == PhaseEnum.Assessing
                    ? ApplicationBusiness.GetListUserAssessment()
                    : ApplicationBusiness.GetListUserCredit();
                foreach (UserData userData in listUsers)
                {
                    userID = int.Parse(userData.UserID);
                    if (listExcludeUser.Contains(userID) || IsRoleApproval(userID))
                    {
                        continue;
                    }
                    ddlUser.Items.Add(CreateItem(userData));
                }
                ddlUser.SelectedIndex = -1;
                ddlUser.Enabled = true;
                BindListPolicy(ListSource);
            }
        }

        private static RadComboBoxItem CreateItem(UserData userData)
        {
            string text = FunctionBase.FormatUser(userData.UserName, userData.DisplayName);
            string value = userData.UserID;
            return new RadComboBoxItem(text, value);
        }

        private void ResetData()
        {
            hidUserID.Value = string.Empty;
            hidPhaseID.Value = string.Empty;
            lblPhase.Text = string.Empty;
            ddlUser.Items.Clear();
            ddlUser.ClearSelection();
            txtKPI.Text = string.Empty;
            ddlIsDisable.SelectedIndex = 0;
            ListSource.Items.Clear();
            ListSource.ClearSelection();
            ListDestination.Items.Clear();
            ListDestination.ClearSelection();
        }

        protected void Save(object sender, EventArgs e)
        {
            List<string> listPolicyCode = new List<string>();
            foreach (RadListBoxItem item in ListDestination.Items)
            {
                listPolicyCode.Add(item.Value);
            }
            Dictionary<string, SQLParameterData> dataDictionary = new Dictionary<string, SQLParameterData>
            {
                { UserPhaseTable.UserID, new SQLParameterData(ddlUser.SelectedValue, SqlDbType.Int) },
                { UserPhaseTable.PhaseID, new SQLParameterData(hidPhaseID.Value, SqlDbType.Int) },
                { UserPhaseTable.PolicyCode, new SQLParameterData(string.Join(",", listPolicyCode)) },
                { UserPhaseTable.KPI, new SQLParameterData(txtKPI.Text.Trim(), SqlDbType.Int) },
                { UserPhaseTable.IsDisable, new SQLParameterData((ddlIsDisable.SelectedValue == "1").ToString(), SqlDbType.Bit) },
                { BaseTable.UserIDModify, new SQLParameterData(UserInfo.UserID, SqlDbType.Int) },
                { BaseTable.DateTimeModify, new SQLParameterData(DateTime.Now.ToString(PatternEnum.DateTime), SqlDbType.BigInt) }
            };

            int userID = int.TryParse(hidUserID.Value, out userID) && userID > 0
                ? UserPhaseBusiness.UpdateUserPhase(dataDictionary)
                : UserPhaseBusiness.InsertUserPhase(dataDictionary); 

            if (userID > 0)
            {
                hidUserID.Value = userID.ToString();
                string key = UserPhaseBusiness.GetCacheKey(hidPhaseID.Value, hidUserID.Value);
                CacheBase.Reload<UserPhaseData>(key);
                LoadUserData(hidPhaseID.Value, hidUserID.Value);
                ShowMessage("Cập nhật thông tin thành công.", ModuleMessage.ModuleMessageType.GreenSuccess);
            }
            else
            {
                ShowMessage("Cập nhật thông tin thất bại.", ModuleMessage.ModuleMessageType.RedError);
            }
        }
    }
}