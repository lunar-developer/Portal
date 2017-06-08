using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using DotNetNuke.Entities.Tabs;
using DotNetNuke.UI.Skins.Controls;
using Modules.Application.Business;
using Modules.Application.Database;
using Modules.Application.DataTransfer;
using Modules.Application.Enum;
using Modules.Application.Global;
using Telerik.Web.UI;
using Website.Library.Enum;
using Website.Library.Global;
using DataTable = System.Data.DataTable;

namespace DesktopModules.Modules.Applic
{
    public partial class Assignment : ApplicationModuleBase
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
            BindApplicationType();
            BindPhaseData();
        }

        private void BindApplicationType()
        {
            ddlApplicationType.Items.Add(new ListItem("Tất cả","-1"));
            BindApplicationTypeData(ddlApplicationType);
        }
        private void BindPhaseData()
        {
            ddlPhase.Items.Clear();
            ddlPhase.Items.Add(new ListItem("Tất cả", "-1"));
            if (!ApplicationCode.Equals("-1"))
            {
                BindPhaseData(ddlPhase, ApplicationCode);
            }
        }

        protected void ApplicationTypeChange(object sender, EventArgs e)
        {
            BindPhaseData();
        }
        protected void PhaseChange(object sender, EventArgs e)
        {
            BindGrid();
        }
        protected void AddUser(object sender, EventArgs e)
        {
            Dictionary<string,string> dictionary = new Dictionary<string, string>()
            {
                {ApplicationEnum.RequestType,ApplicationEnum.Add },
                {UserPhaseMappingTable.ApplicationTypeCode, ddlApplicationType.SelectedValue },
                {UserPhaseMappingTable.PhaseCode, ddlPhase.SelectedValue }
            };
            string url =
                TabController.Instance.GetTab(int.Parse(FunctionBase.GetConfiguration("APP_UserConfiguration_PageID")),
                    0).FullUrl;
            string script = GetWindowOpenScript(url, dictionary, false);
            RegisterScript(script);
        }
        protected void DeleteAssignItem(object sender, EventArgs e)
        {
            LinkButton button = sender as LinkButton;
            if (button == null)
            {
                return;
            }
            string userPhaseMappingID = button.CommandArgument;
            Dictionary<string, string> dictionary = new Dictionary<string, string>()
            {
                {UserPhaseMappingTable.UserPhaseMappingID,userPhaseMappingID },
                {UserPhaseMappingTable.ModifyUserID, UserInfo.UserID.ToString() },
                {UserPhaseMappingTable.ModifyDateTime , DateTime.Now.ToString(PatternEnum.DateTime) }
            };
            string message;
            if (UserPhaseMappingBusiness.DeleteUserPhaseMappingData(dictionary,out message))
            {
                ShowMessage("Xóa phân công hồ sơ Thành công.", ModuleMessage.ModuleMessageType.GreenSuccess);
                CacheBase.Remove<UserPhaseMappingData>(userPhaseMappingID);
                BindGrid();
            }
            else
            {
                ShowMessage(message, ModuleMessage.ModuleMessageType.RedError);
            }
        }

        private void ActionEditItem(Dictionary<string, string> dictionary)
        {
            string url =
                TabController.Instance.GetTab(int.Parse(FunctionBase.GetConfiguration("APP_UserConfiguration_PageID")),
                    0).FullUrl;
            string script = GetWindowOpenScript(url, dictionary, false);
            RegisterScript(script);
        }
        protected void EditAssignItem(object sender, EventArgs e)
        {
            LinkButton button = sender as LinkButton;
            if (button == null)
            {
                return;
            }
            
            Dictionary<string, string> dictionary = new Dictionary<string, string>()
            {
                {ApplicationEnum.RequestType,ApplicationEnum.Edit },
                {UserPhaseMappingTable.UserPhaseMappingID,button.CommandArgument },
            };
            ActionEditItem(dictionary);
        }
        protected void EditAssignPhase(object sender, EventArgs e)
        {
            LinkButton button = sender as LinkButton;
            if (button == null)
            {
                return;
            }

            Dictionary<string, string> dictionary = new Dictionary<string, string>()
            {
                {ApplicationEnum.RequestType,ApplicationEnum.EditPhase },
                {UserPhaseMappingTable.UserPhaseMappingID,button.CommandArgument },
            };
            ActionEditItem(dictionary);
        }
        protected void EditAssignPolicy(object sender, EventArgs e)
        {
            LinkButton button = sender as LinkButton;
            if (button == null)
            {
                return;
            }

            Dictionary<string, string> dictionary = new Dictionary<string, string>()
            {
                {ApplicationEnum.RequestType,ApplicationEnum.EditPolicy },
                {UserPhaseMappingTable.UserPhaseMappingID,button.CommandArgument },
            };
            ActionEditItem(dictionary);
        }
        protected void OnPageIndexChanging(object sender, GridPageChangedEventArgs e)
        {
            BindGrid(e.NewPageIndex);
        }

        protected void OnPageSizeChanging(object sender, GridPageSizeChangedEventArgs e)
        {
            BindGrid();
        }
        private void BindGrid(int pageIndex = 0)
        {
            gridData.Visible = true;
            gridData.CurrentPageIndex = pageIndex;
            gridData.DataSource = GetGridData();
            gridData.DataBind();
        }
        private DataTable GetGridData()
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>
            {
                { UserPhaseMappingTable.ApplicationTypeCode, ApplicationCode},
                { UserPhaseMappingTable.PhaseCode, PhaseCode },
            };
            return UserPhaseMappingBusiness.GetUserPhaseMappingData(dictionary);
        }
        private void SetPermission()
        {
            btnAddUser.Visible = IsConfigurationRole();
        }

        protected bool EditItemVisble(string value)
        {
            return !string.IsNullOrWhiteSpace(value);
        }
        private string PhaseCode => ddlPhase.SelectedValue ?? "-1";
        private string ApplicationCode => ddlApplicationType.SelectedValue ?? "-1";
    }
}
