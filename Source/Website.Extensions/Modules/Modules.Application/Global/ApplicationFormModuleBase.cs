using System;
using System.Web.UI.WebControls;

namespace Modules.Application.Global
{
    public class ApplicationFormModuleBase : ApplicationModuleBase
    {
        protected override void OnInit(EventArgs e)
        {
        }

        protected ListItem GetEmptyItem()
        {
            return new ListItem("CHƯA CHỌN", string.Empty);
        }
    }
}