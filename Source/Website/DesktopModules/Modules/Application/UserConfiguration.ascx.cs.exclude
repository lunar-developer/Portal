using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using DotNetNuke.UI.Skins.Controls;
using Modules.Application.Business;
using Modules.Application.Database;
using Modules.Application.DataTransfer;
using Modules.Application.Enum;
using Modules.Application.Global;
using Website.Library.Enum;
using Website.Library.Global;

namespace DesktopModules.Modules.Applic
{
    public partial class UserConfiguration : ApplicationModuleBase
    {
        protected override void OnLoad(EventArgs e)
        {
            try
            {
                if (IsPostBack)
                {
                    return;
                }
                AutoWire();
                BindData();
            }
            catch (Exception exception)
            {
                ShowException(exception);
            }
            finally
            {
                SetPermission();
            }

        }

        private void BindData()
        {
            hidRequestType.Value = Request.QueryString[ApplicationEnum.RequestType] ?? "-1";
            if (RequestType != null)
            {
                if (!RequestType.Equals(ApplicationEnum.Add))
                {
                    string id = Request.QueryString[UserPhaseMappingTable.UserPhaseMappingID] ?? "-1";
                    hidKeyID.Value = id;
                    UserData = CacheBase.Receive<UserPhaseMappingData>(EditKeyID);
                    if (UserData == null)
                    {
                        ShowMessage("Không tìm thấy thông tin người dùng, vui lòng thưc hiện lại",
                            ModuleMessage.ModuleMessageType.RedError);
                    }
                    btnAddUserProcess.Text = GetResource("btnUpdate.Text");
                }

                ApplicationInfo(RequestType.Equals(ApplicationEnum.Add) || RequestType.Equals(ApplicationEnum.Edit));
                PhaseInfo(!RequestType.Equals(ApplicationEnum.EditPolicy));
                UserMappingInfo(RequestType.Equals(ApplicationEnum.Add));
                BindPolicyData(UserData?.PolicyCode);
                
            }
            else
            {
                ShowMessage("Yêu cầu không hợp lệ, vui lòng thực hiện lại",
                            ModuleMessage.ModuleMessageType.RedError);
            }
        }

        private void ApplicationInfo(bool isEdit)
        {
            if (isEdit)
            {
                rowtxtApplication.Visible = false;
                rowddlApplication.Visible = true;
                ddlApplicationType.Enabled = true;
                BindApplicationType();
            }
            else
            {
                rowtxtApplication.Visible = true;
                rowddlApplication.Visible = false;
                ddlApplicationType.Enabled = false;
                txtApplicationType.Text = UserData?.ApplicationTypeName;
                hidApplicationTypeCode.Value = UserData?.ApplicationTypeCode;
            }
        }

        private void PhaseInfo(bool isEdit)
        {
            if (isEdit)
            {
                rowtxtPhase.Visible = false;
                rowddlPhase.Visible = true;
                ddlPhase.Enabled = true;
                BindPhaseData();
            }
            else
            {
                rowtxtPhase.Visible = true;
                rowddlPhase.Visible = false;
                ddlPhase.Enabled = false;
                txtPhase.Text = UserData?.PhaseName;
                hidPhaseCode.Value = UserData?.PhaseCode;
            }
        }

        private void UserMappingInfo(bool isEdit)
        {
            if (isEdit)
            {
                rowtxtUser.Visible = false;
                rowddlUser.Visible = true;
                ddlUser.Enabled = true;
                BindUserData();
            }
            else
            {
                rowtxtUser.Visible = true;
                rowddlUser.Visible = false;
                ddlUser.Enabled = false;
                txtUser.Text = UserData?.FullName;
                txtKpi.Text = UserData?.KPI;
                hidUserID.Value = UserData?.UserID;
            }
            BindAbsentCalendarData();
        }
        private void BindApplicationType()
        {
            ddlApplicationType.Items.Clear();
            ddlApplicationType.Items.Add(new ListItem("Tất cả", "-1"));
            BindApplicationTypeData(ddlApplicationType, ApplicationTypeCode);
        }

        protected void ApplicationTypeChange(object sender, EventArgs e)
        {
            ddlPhase.Items.Clear();
            if (RequestType.Equals(ApplicationEnum.Add))
            {
                ddlUser.Items.Clear();
            }
            ddlPolicyNonSelected.Items.Clear();
            ddlPolicySelected.Items.Clear();
            BindPhaseData();
        }

        private void BindPhaseData()
        {
            if (!ApplicationTypeCode.Equals("-1"))
            {
                ddlPhase.Items.Clear();
                ddlPhase.Items.Add(new ListItem("Tất cả", "-1"));
                BindPhaseData(ddlPhase, ApplicationTypeCode, PhaseCode);
            }
        }

        protected void PhaseCodeChange(object sender, EventArgs e)
        {
            BindUserData();
            BindPolicyData();
        }

        private void BindUserData()
        {
            if (RequestType.Equals(ApplicationEnum.Add))
            {
                ddlUser.Items.Clear();
                ddlUser.Items.Add(new ListItem("Chưa chọn", "-1"));
                BinUserMappingData(ddlUser, PhaseCode);
            }
            
        }

        protected void UserIDChange(object sender, EventArgs e)
        {
            ddlPolicyNonSelected.Items.Clear();
            ddlPolicySelected.Items.Clear();
            BindPolicyData();
            BindAbsentCalendarData();
            
        }
        private void BindPolicyData(string policyData = null)
        {
            ddlPolicyNonSelected.Items.Clear();
            ddlPolicySelected.Items.Clear();
            if (policyData == null)
            {
                UserPhaseMappingData userData = GetUserData();
                btnAddUserProcess.Text = userData != null ? GetResource("btnUpdate.Text") : GetResource("btnAdd.Text");
                txtKpi.Text = userData?.KPI;
                policyData = userData?.PolicyCode;
            }

            BindPolicyData(ddlPolicySelected, policyData, true);
            BindPolicyData(ddlPolicyNonSelected, policyData, false);
        }

        protected void AddSelectedPolicy(object sender, EventArgs e)
        {
            AddPolicy();
        }

        protected void SelectAllPolicy(object sender, EventArgs e)
        {
            if (ddlPolicyNonSelected != null)
            {
                for (int i = 0; i < ddlPolicyNonSelected.Items.Count; i++)
                {
                    ddlPolicyNonSelected.Items[i].Selected = true;
                }
            }
            AddPolicy();
        }

        private void AddPolicy()
        {
            if (ddlPolicyNonSelected != null)
            {
                while (ddlPolicyNonSelected.SelectedIndex > -1)
                {
                    ListItem item = ddlPolicyNonSelected.SelectedItem;
                    item.Selected = false;
                    ddlPolicySelected.Items.Add(item);
                    ddlPolicyNonSelected.Items.Remove(item);
                }
            }
        }

        protected void RemoveSelectedPolicy(object sender, EventArgs e)
        {
            RemovePolicy();
        }

        protected void RemoveAllPolicy(object sender, EventArgs e)
        {
            if (ddlPolicySelected != null)
            {
                for (int i = 0; i < ddlPolicySelected.Items.Count; i++)
                {
                    ddlPolicySelected.Items[i].Selected = true;
                }
            }
            RemovePolicy();
        }

        private void RemovePolicy()
        {
            if (ddlPolicySelected != null)
            {
                while (ddlPolicySelected.SelectedIndex > -1)
                {
                    ListItem item = ddlPolicySelected.SelectedItem;
                    item.Selected = false;
                    ddlPolicyNonSelected.Items.Add(item);
                    ddlPolicySelected.Items.Remove(item);
                }
            }
        }

        private void BindAbsentCalendarData()
        {
            if (UserID.Equals("-1"))
            {
                DivAbSentCalendar.InnerHtml = string.Empty;
            }
            else
            {
                List<AbsentCalendarData> itemList = AbsentCalendarBusiness.GetListAbsentCalendar(UserID);
                if (itemList == null || itemList.Count == 0)
                {
                    DivAbSentCalendar.InnerHtml = string.Empty;
                }
                else
                {
                    StringBuilder sbAbsentData = new StringBuilder();
                    foreach (AbsentCalendarData item in itemList)
                    {
                        StringBuilder data = new StringBuilder();
                        string absentType;
                        int absentTypeValue =
                            int.Parse(string.IsNullOrWhiteSpace(item.AbsentType) ? "-1" : item.AbsentType);
                        switch (absentTypeValue)
                        {
                            case AbsentCalendarEnum.IsAllDay:
                                absentType = "Hết ngày";
                                break;
                            case AbsentCalendarEnum.IsMorning:
                                absentType = "Buổi sáng";
                                break;
                            case AbsentCalendarEnum.IsAfternoon:
                                absentType = "Buổi chiều";
                                break;
                            default:
                                absentType = "Hết ngày";
                                break;
                        }
                        data.Append("<tr>");
                        data.Append($"<td class=\"text-center\">{absentType}</td>");
                        data.Append($"<td class=\"text-center\">" +
                            $"{item.FromDate.Substring(6, 2).PadLeft(2, '0')}-" +
                            $"{item.FromDate.Substring(4, 2).PadLeft(2, '0')}-" +
                            $"{item.FromDate.Substring(0, 4).PadLeft(4, '0')}" +
                            $"</td>");
                        data.Append($"<td class=\"text-center\">" +
                            $"{item.ToDate.Substring(6, 2).PadLeft(2, '0')}-" +
                            $"{item.ToDate.Substring(4, 2).PadLeft(2, '0')}-" +
                            $"{item.ToDate.Substring(0, 4).PadLeft(4, '0')}" +
                            $"</td>");
                        data.Append("</tr>");
                        sbAbsentData.Append(data);
                    }
                    DivAbSentCalendar.InnerHtml = string.Format(AbsentCalendarHtmlTemplate,
                        sbAbsentData);
                }
            }
        }
        protected void ChangeUserAssignment(object sender, EventArgs e)
        {
            string policyList = string.Empty;
            if (ddlPolicySelected != null)
            {
                foreach (ListItem item in ddlPolicySelected.Items)
                {
                    policyList += (string.IsNullOrWhiteSpace(policyList) ? item.Value : $";{item.Value}");
                }
            }
            if (!UserID.Equals("-1") && !ApplicationTypeCode.Equals("-1") &&
                !PhaseCode.Equals("-1") && !string.IsNullOrWhiteSpace(policyList) &&
                !string.IsNullOrWhiteSpace(txtKpi.Text.Trim()) && 
                !string.IsNullOrWhiteSpace(txtRemark.Text.Trim()))
            {
                UserPhaseMappingData userDate = RequestType.Equals(ApplicationEnum.Add) ? GetUserData() : CacheBase.Receive<UserPhaseMappingData>(EditKeyID);
                string userPhaseMappingID = userDate == null ? "-1" : userDate.UserPhaseMappingID;
                Dictionary<string, string> dictionary = new Dictionary<string, string>
                {
                    { UserPhaseMappingTable.UserPhaseMappingID, userPhaseMappingID },
                    { UserPhaseMappingTable.UserID, UserID },
                    { UserPhaseMappingTable.ApplicationTypeCode, ApplicationTypeCode },
                    { UserPhaseMappingTable.PhaseCode, PhaseCode },
                    { UserPhaseMappingTable.PolicyCode, policyList },
                    { UserPhaseMappingTable.KPI, txtKpi.Text.Trim() },
                    { "Remark", txtRemark.Text.Trim() },
                    { UserPhaseMappingTable.ModifyUserID, UserInfo.UserID.ToString() },
                    { UserPhaseMappingTable.ModifyDateTime, DateTime.Now.ToString(PatternEnum.DateTime) }
                };

                string message;
                int id;
                if (UserPhaseMappingBusiness.InsertUserPhaseMappingData(dictionary,out id, out message))
                {
                    CacheBase.Remove<UserPhaseMappingData>(userPhaseMappingID);
                    CacheBase.Insert(id.ToString(),UserPhaseMappingBusiness.GetUserPhaseMapping(id.ToString()));
                    ShowMessage("Cập nhật thông tin thành công.", ModuleMessage.ModuleMessageType.GreenSuccess);
                }
                else
                {
                    ShowMessage(message, ModuleMessage.ModuleMessageType.RedError);
                }
            }
            else
            {
                ShowMessage("Vui lòng điền đủ các trường và thực hiện lại", ModuleMessage.ModuleMessageType.RedError);
            }
        }
        
        
        private void SetPermission()
        {
            btnAddUserProcess.Visible = IsConfigurationRole();
        }

        private string EditKeyID => hidKeyID.Value ?? "-1";

        private UserPhaseMappingData GetUserData()
        {
            if (RequestType.Equals(ApplicationEnum.Add))
            {
                List<UserPhaseMappingData> userPhaseList = CacheBase.Receive<UserPhaseMappingData>();
                if (userPhaseList != null && !UserID.Equals("-1") && !ApplicationTypeCode.Equals("-1") &&
                    !PhaseCode.Equals("-1"))
                {
                    return userPhaseList.FirstOrDefault(userPhase => userPhase.UserID.Equals(UserID) &&
                                                        userPhase.ApplicationTypeCode.Equals(ApplicationTypeCode) &&
                                                        userPhase.PhaseCode.Equals(PhaseCode));
                }
            }
            return null;
        }
        private UserPhaseMappingData UserData { get; set; }
        private string RequestType => hidRequestType.Value;
        private string UserID => RequestType.Equals(ApplicationEnum.Add) ? ddlUser.SelectedValue ?? "-1" : hidUserID.Value;
        private string ApplicationTypeCode => RequestType.Equals(ApplicationEnum.Add) || RequestType.Equals(ApplicationEnum.Edit) ?  
                    ddlApplicationType.SelectedValue : 
                    hidApplicationTypeCode.Value;
        private string PhaseCode => !RequestType.Equals(ApplicationEnum.EditPolicy) ? ddlPhase.SelectedValue : hidPhaseCode.Value;

        private const string AbsentCalendarHtmlTemplate = @"
            <div class=""row"">
                <div class=""col-md-3""></div>
                <div class=""col-md-6"">
                    <table class=""table c-margin-t-10"">
                        <colgroup>
                            <col width=""20%"" />
                            <col width=""40%"" />
                            <col width=""40%"" />
                        </colgroup>
                        <thead>
                            <tr>
                                <th class=""text-center"">Vắng</th>
                                <th class=""text-center"">Từ ngày</th>
                                <th class=""text-center"">Đến ngày</th>
                                </tr>
                        </thead>
                        <tbody>{0}</tbody>
                    </table>
                </div>
                <div class=""col-md-3""></div>
            </div>";
    }

}
