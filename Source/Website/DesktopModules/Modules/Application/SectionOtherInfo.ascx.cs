using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Modules.Applic.Global;

namespace DesktopModules.Modules.Applic
{
    public partial class SectionOtherInfo : ApplicationModuleBase
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
                //SetPermission();
            }
        }

        private void BindData()
        {
            BindApplicationType();
            BindPriority();
        }
        private void BindApplicationType()
        {
            ctApplicationType.Items.Add(new ListItem("Chưa chọn", "-1"));
            BindApplicationTypeData(ctApplicationType);
        }

        private void BindPriority()
        {
            ctPriority.Items.Add(new ListItem("Chưa chọn", "-1"));
            ctPriority.Items.Add(new ListItem("VIP ", "0"));
            ctPriority.Items.Add(new ListItem("Gấp", "1"));
        }
    }
}