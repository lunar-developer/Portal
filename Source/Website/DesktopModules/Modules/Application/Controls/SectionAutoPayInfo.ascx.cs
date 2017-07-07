using System;
using Modules.Application.Global;

namespace DesktopModules.Modules.Application.Controls
{
    public partial class SectionAutoDebitInfo : ApplicationFormModuleBase
    {
        protected override void OnLoad(EventArgs e)
        {
            if (IsPostBack)
            {
                return;
            }
        }
    }
}