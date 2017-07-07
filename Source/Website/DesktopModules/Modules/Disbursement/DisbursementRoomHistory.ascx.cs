using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.WebControls;
using DotNetNuke.UI.Skins.Controls;
using Modules.Disbursement.Business;
using Modules.Disbursement.Database;
using Modules.Disbursement.Enum;
using Modules.Disbursement.Global;
using Telerik.Web.UI;
using Website.Library.DataTransfer;
using Website.Library.Enum;

namespace DesktopModules.Modules.Disbursement
{
    public partial class DisbursementRoomHistory : DisbursementModuleBase
    {
        protected override void OnLoad(EventArgs e)
        {
            if (IsPostBack)
            {
                return;
            }
            BindGrid(0);
        }
        private void BindGrid(int pageIndex = 0)
        {
            gridData.Visible = true;
            gridData.DataSource = DisbursementRoomBusiness.GetTop500RecentChanges();
            gridData.CurrentPageIndex = pageIndex;
            gridData.DataBind();
        }

        protected void OnPageIndexChanging(object sender, GridPageChangedEventArgs e)
        {
            BindGrid(e.NewPageIndex);
        }

        protected void OnPageSizeChanging(object sender, GridPageSizeChangedEventArgs e)
        {
            BindGrid();
        }

    }
}