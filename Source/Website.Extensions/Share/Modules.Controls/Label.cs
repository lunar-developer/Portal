using System;
using DotNetNuke.UI.UserControls;

namespace Modules.Controls
{
    public class Label : LabelControl
    {
        public bool IsRequire { get; set; }


        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            Text = $"{GetHighlight()}{Text}";
        }

        private string GetHighlight()
        {
            return IsRequire ? "<span class='c-font-red-2'>*</span>&nbsp;" : string.Empty;
        }
    }
}