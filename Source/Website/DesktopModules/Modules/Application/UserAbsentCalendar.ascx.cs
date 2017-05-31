using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DotNetNuke.Entities.Tabs;
using DotNetNuke.UI.Skins.Controls;
using Modules.Application.Business;
using Modules.Application.Database;
using Modules.Application.Enum;
using Modules.Application.Global;
using Telerik.Web.UI;
using Website.Library.Enum;
using Website.Library.Global;

namespace DesktopModules.Modules.Applic
{
    public partial class UserAbsentCalendar : ApplicationModuleBase
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

        protected void FromDateChange(object sender, EventArgs e)
        {
            //DateValidation();
        }

        protected void ToDateChange(object sender, EventArgs e)
        {
            //DateValidation();
        }

        protected void DeleteAbsentItem(object sender, EventArgs e)
        {
            LinkButton button = sender as LinkButton;
            if (button == null)
            {
                return;
            }

            Dictionary<string, string> dictionary = new Dictionary<string, string>()
            {
                {AbsentCalendarTable.ID,button.CommandArgument },
                {AbsentCalendarTable.UserID,button.CommandName },
                {AbsentCalendarTable.ModifyUserID, UserInfo.UserID.ToString() },
                {AbsentCalendarTable.ModifyDateTime , DateTime.Now.ToString(PatternEnum.DateTime) }
            };
            string message;
            if (AbsentCalendarBusiness.DeleteAbsentCalendar(dictionary, out message))
            {
                ShowMessage("Xóa ngày phép thành công.", ModuleMessage.ModuleMessageType.GreenSuccess);
                BindAbsentGrid();
            }
            else
            {
                ShowMessage(message, ModuleMessage.ModuleMessageType.RedError);
            }
        }

        protected void AddAbSentCalendar(object sender, EventArgs e)
        {
            if (ValidationForm())
            {
                Dictionary<string, string> dictionary = new Dictionary<string, string>()
                {
                    {AbsentCalendarTable.UserID, UserID },
                    {AbsentCalendarTable.AbsentType, AbsentTypeCode },
                    {AbsentCalendarTable.FromDate, FromDate },
                    {AbsentCalendarTable.ToDate, ToDate },
                    {AbsentCalendarTable.ModifyUserID, UserInfo.UserID.ToString() },
                    {AbsentCalendarTable.ModifyDateTime , DateTime.Now.ToString(PatternEnum.DateTime) }
                };
                string message;
                if (AbsentCalendarBusiness.InsertAbsentCalendar(dictionary, out message))
                {
                    ShowMessage("Thêm ngày phép thành công.", ModuleMessage.ModuleMessageType.GreenSuccess);
                    BindAbsentGrid();
                }
                else
                {
                    ShowMessage(message, ModuleMessage.ModuleMessageType.RedError);
                }
            }
        }

        protected void UserConfiguration(object sender, EventArgs e)
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>()
            {
                {ApplicationEnum.RequestType,ApplicationEnum.Edit },
                {UserPhaseMappingTable.UserID,UserID },
            };
            string url =
                TabController.Instance.GetTab(int.Parse(FunctionBase.GetConfiguration("APP_UserConfiguration_PageID")),
                    0).FullUrl;
            string script = GetWindowOpenScript(url, dictionary, false);
            RegisterScript(script);
        }
        private bool ValidationForm()
        {
            if (UserID.Equals("-1"))
            {
                ShowMessage("Vui lòng chọn user cần đăng ký nghỉ phép",
                        ModuleMessage.ModuleMessageType.YellowWarning);
                return false;
            }
            if (AbsentTypeCode.Equals("-1"))
            {
                ShowMessage("Vui lòng chọn loại đăng ký nghỉ phép: Buổi sáng, chiều hoặc cả ngày",
                       ModuleMessage.ModuleMessageType.YellowWarning);
                return false;
            }
            if (dpToDate.SelectedDate != null && dpFromDate.SelectedDate != null)
            {
                if (dpFromDate.SelectedDate > dpToDate.SelectedDate)
                {
                    ShowMessage("Ngày bắt đầu nghỉ phép không được phép lớn hơn ngày kết thúc nghỉ phép",
                        ModuleMessage.ModuleMessageType.RedError);
                    return false;
                }
            }
            if (dpToDate.SelectedDate == null)
            {
                ShowMessage("Vui lòng chọn ngày bắt kết thúc nghỉ phép",
                        ModuleMessage.ModuleMessageType.YellowWarning);
                return false;
            }
            if (dpFromDate.SelectedDate == null)
            {
                ShowMessage("Vui lòng chọn ngày bắt đầu nghỉ phép",
                        ModuleMessage.ModuleMessageType.YellowWarning);
                return false;
            }
            return true;
        }
        private void BindData()
        {
            if (IsConfigurationRole())
            {
                BindUserList();
                BindGrid();
            }
            BindAbsentData();
            BindAbsentGrid();
            dpFromDate.MinDate = DateTime.Today;
            dpToDate.MinDate = DateTime.Today;
        }

        private void BindUserList()
        {
            BindAllUserData(ddlApplicUser,UserID);
        }

        private void BindAbsentData()
        {
            ddlAbsentType.Items.Add(new ListItem("Buổi sáng", AbsentCalendarEnum.IsMorning.ToString()));
            ddlAbsentType.Items.Add(new ListItem("Buổi chiều", AbsentCalendarEnum.IsAfternoon.ToString()));
            ddlAbsentType.Items.Add(new ListItem("Cả ngày", AbsentCalendarEnum.IsAllDay.ToString()));
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
            DataTable dt = GetGridData();
            gridData.Visible = DivUserProcessContent.Visible = IsConfigurationRole() && dt != null && dt.Rows.Count > 0;
            gridData.CurrentPageIndex = pageIndex;
            gridData.DataSource = dt;
            gridData.DataBind();
        }
        private DataTable GetGridData()
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>
            {
                { UserPhaseMappingTable.UserID, UserID},
            };
            return UserPhaseMappingBusiness.GetUserPhaseMappingData(dictionary);
        }
        protected void OnPageAbsentIndexChanging(object sender, GridPageChangedEventArgs e)
        {
            BindAbsentGrid(e.NewPageIndex);
        }

        protected void OnPageAbsentSizeChanging(object sender, GridPageSizeChangedEventArgs e)
        {
            BindAbsentGrid();
        }


        private void BindAbsentGrid(int pageIndex = 0)
        {
            DataTable dt = AbsentCalendarBusiness.GetAbsentDataTable(UserID);
            gridUserAbsent.Visible = DivAbSentCalendar.Visible = dt != null && dt.Rows.Count > 0;
            gridUserAbsent.CurrentPageIndex = pageIndex;
            gridUserAbsent.DataSource = dt;
            gridUserAbsent.DataBind();
        }
        private void SetPermission()
        {
            DivUser.Visible = !IsConfigurationRole();
            DivUserList.Visible = IsConfigurationRole();
        }
        

        private string FromDate => dpFromDate.SelectedDate?.ToString(PatternEnum.Date);
        private string ToDate => dpToDate.SelectedDate?.ToString(PatternEnum.Date);
        private string AbsentTypeCode => ddlAbsentType.SelectedValue ?? "-1";
        private string UserID => IsConfigurationRole() ? ddlApplicUser.SelectedValue ?? UserInfo.UserID.ToString() : UserInfo.UserID.ToString();
        
    }
}
