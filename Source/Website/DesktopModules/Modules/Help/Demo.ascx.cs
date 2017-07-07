using DotNetNuke.UI.Skins.Controls;
using Modules.UserManagement.Business;
using Modules.UserManagement.Database;
using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Web.UI.WebControls;
using DotNetNuke.Entities.Portals;
using DotNetNuke.Entities.Users;
using DotNetNuke.Security.Roles;
using DotNetNuke.Services.Social.Notifications;
using Telerik.Web.UI;
using Website.Library.DataTransfer;
using Website.Library.Global;

namespace DesktopModules.Modules.Help
{
    public partial class Demo : DesktopModuleBase
    {
        protected void RadGrid1_ItemCreated(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridPagerItem)
            {
                RadNumericTextBox goToPageText = (RadNumericTextBox)e.Item.FindControl("GoToPageTextBox");



                goToPageText.Width = Unit.Pixel(50);
                goToPageText.ShowSpinButtons = true;
                //goToPageText.Style.Add("color", "gray");

                

                RadNumericTextBox ChangePageSizeTextBox = (RadNumericTextBox)e.Item.FindControl("ChangePageSizeTextBox");
                ChangePageSizeTextBox.IncrementSettings.Step = 10;



                ChangePageSizeTextBox.Width = Unit.Pixel(50);
                ChangePageSizeTextBox.ShowSpinButtons = true;
                //ChangePageSizeTextBox.Style.Add("color", "gray");
            }
        }

        protected void OnItemRequest(object sender, RadComboBoxItemsRequestedEventArgs e)
        {
            Dictionary<string, SQLParameterData> dictionary = new Dictionary<string, SQLParameterData>
            {
                { "UserName", new SQLParameterData(e.Text, SqlDbType.VarChar)},
                { "UserID", new SQLParameterData(1, SqlDbType.Int)}
            };

            DataTable data = UserBusiness.SearchUser(dictionary);

            if (data.Rows.Count == 0)
            {
                e.EndOfItems = false;
                e.NumberOfItems = 0;
                return;
            }

            int itemOffset = e.NumberOfItems;
            int endOffset = Math.Min(itemOffset + ddlUser.ItemsPerRequest, data.Rows.Count);
            e.EndOfItems = endOffset == data.Rows.Count;

            for (int i = itemOffset; i < endOffset; i++)
            {
                string text = data.Rows[i]["UserName"].ToString();
                string value = data.Rows[i]["UserID"].ToString();
                ddlUser.Items.Add(new RadComboBoxItem(text, value));
            }
            //    ddlUser.ShowMoreResultsBox = !e.EndOfItems;

            //   ddlUser.DataSource = data;
            //ddlUser.DataTextField = "DisplayName";
            //ddlUser.DataValueField = "UserID";
            //   ddlUser.DataBind();
        }

        protected override void OnLoad(EventArgs e)
        {
            if (IsPostBack == false)
            {
                RegisterConfirmDialog(btnConfirm, "Are you sure?");
                //Dictionary<string, SQLParameterData> dictionary = new Dictionary<string, SQLParameterData>
                //{
                //    { "UserID", new SQLParameterData(1, SqlDbType.Int)}
                //};
                //DataTable data = UserBusiness.SearchUser(dictionary);

                //ddlUser.DataSource = data;
                //ddlUser.DataTextField = "UserName";
                //ddlUser.DataValueField = "UserID";
                //ddlUser.DataBind();

                GridData.DataSource = BranchBusiness.GetAllBranchInfo();
                GridData.DataBind();
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