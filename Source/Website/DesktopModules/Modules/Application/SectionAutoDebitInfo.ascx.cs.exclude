using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Modules.Applic.Global;

namespace DesktopModules.Modules.Applic
{
    public partial class SectionAutoDebitInfo : ApplicationModuleBase
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

        }

    }
}