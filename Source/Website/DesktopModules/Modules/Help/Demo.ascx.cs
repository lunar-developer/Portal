using DotNetNuke.UI.Skins.Controls;
using Modules.UserManagement.Business;
using Modules.UserManagement.Database;
using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using DotNetNuke.Entities.Portals;
using DotNetNuke.Entities.Users;
using DotNetNuke.Security.Roles;
using DotNetNuke.Services.Social.Notifications;
using Website.Library.Global;

namespace DesktopModules.Modules.Help
{
    public partial class Demo : DesktopModuleBase
	{
        protected override void OnLoad(EventArgs e)
        {
            if (IsPostBack == false)
            {
                RegisterConfirmDialog(btnConfirm, "Are you sure?");
            }

            //var portalId =
            //    PortalController.GetEffectivePortalId(UserController.Instance.GetCurrentUserInfo().PortalID);
            //Notification notification = new Notification
            //{
            //    Subject = "Test",
            //    Body = "Your Message",
            //    From = UserInfo.Email,
            //    SenderUserID = UserInfo.UserID,
            //    NotificationTypeID = 1
            //};
            //try
            //{
            //    List<UserInfo> users = new List<UserInfo> { UserController.GetUserById(portalId, UserInfo.UserID) };
            //    NotificationsController.Instance.SendNotification(notification, portalId, new List<RoleInfo>(), users);
            //    ShowMessage("Send notification success!", ModuleMessage.ModuleMessageType.GreenSuccess);
            //}
            //catch(Exception ex)
            //{
            //    ShowMessage(ex.Message);
            //}
        }

        protected void Export(object sender, EventArgs e)
        {
            try
            {
                //Dictionary<string, string> dictionary = new Dictionary<string, string>
                //{
                //    { UserTable.UserName, "lunar.developer@gmail.com" },
                //    { UserTable.UserID, UserInfo.UserID.ToString() }
                //};
                //DataTable dtResult = UserBusiness.SearchUser(dictionary);
                //ExportToExcel(dtResult, "Demo");    // File name only
            }
            catch (Exception exception)
            {
                ShowMessage(exception.Message, ModuleMessage.ModuleMessageType.RedError);
            }
        }

        protected void Edit(object sender, EventArgs e)
        {
            try
            {
                string url = EditUrl(null, 1160, 100, false, null);
                RegisterScript(url);
            }
            catch (Exception exception)
            {
                ShowMessage(exception.Message, ModuleMessage.ModuleMessageType.RedError);
            }
        }

        protected void Search(object sender, EventArgs e)
	    {
            try
            {
                //Dictionary<string, string> dictionary = new Dictionary<string, string>
                //{
                //    { UserTable.UserName, "lunar.developer@gmail.com" },
                //    { UserTable.UserID, UserInfo.UserID.ToString() }
                //};
                //DataTable dtResult = UserBusiness.SearchUser(dictionary);
                //System.Threading.Thread.Sleep(2000);
                //tbUserName.Text = dtResult.Rows[0][UserTable.UserName].ToString();
                //tbDisplayName.Text = dtResult.Rows[0][UserTable.DisplayName].ToString();
                //tbBiography.Text = dtResult.Rows[0][UserTable.UserID].ToString();
                //chkState.Checked = true;
            }
            catch (Exception exception)
            {
                ShowMessage(exception.Message, ModuleMessage.ModuleMessageType.RedError);
            }
	    }

        protected void Alert(object sender, EventArgs e)
        {
            ShowAlertDialog("Welcome " + UserInfo.DisplayName);
        }

        protected void Confirm(object sender, EventArgs e)
        {
            ShowMessage("Confirm button clicked");
        }

        protected void ShowSuccess(object sender, EventArgs e)
        {
            ShowMessage("Success Message", ModuleMessage.ModuleMessageType.GreenSuccess);
        }

        protected void ShowInfo(object sender, EventArgs e)
        {
            ShowMessage("Information Message");
        }

        protected void ShowWarning(object sender, EventArgs e)
        {
            ShowMessage("Warning Message", ModuleMessage.ModuleMessageType.YellowWarning);
        }

        protected void ShowError(object sender, EventArgs e)
        {
            ShowMessage("Error Message", ModuleMessage.ModuleMessageType.RedError);
        }
    }
}