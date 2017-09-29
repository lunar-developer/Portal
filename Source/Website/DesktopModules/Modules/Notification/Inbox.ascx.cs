using System;
using DotNetNuke.Services.Social.Notifications;
using DotNetNuke.UI.Skins.Controls;
using Telerik.Web.UI;
using Website.Library.Global;

namespace DesktopModules.Modules.Notification
{
    public partial class Inbox : DesktopModuleBase
    {
        protected override void OnLoad(EventArgs e)
        {
            if (IsPostBack)
            {
                return;
            }

            gridData.DataBind();
        }

        protected void Refresh(object sender, EventArgs e)
        {
            ProcessOnGridNeedDataSource(null, null);
            gridData.DataBind();
        }

        protected void ProcessOnGridNeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            gridData.DataSource = NotificationsController.Instance.GetNotifications(UserInfo.UserID, PortalId, -1, 100);
        }

        protected void DeleteNotification(object sender, EventArgs eventArgs)
        {
            NotificationsController.Instance.DeleteUserNotifications(UserInfo);
            Refresh(null, null);
            ShowMessage("Xóa hộp thư thành công!", ModuleMessage.ModuleMessageType.GreenSuccess);
        }
    }
}