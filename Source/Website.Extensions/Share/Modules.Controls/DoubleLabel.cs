using System;
using DotNetNuke.Services.Localization;
using DotNetNuke.UI.UserControls;

namespace Modules.Controls
{
    public class DoubleLabel : LabelControl
    {
        public bool IsRequire { get; set; }
        public string SubText { get; set; }


        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            Text = $"{GetHighlight()}{Text}<span class='help-block c-margin-0'>{GetSubText()}</span>";
        }

        private string GetHighlight()
        {
            return IsRequire ? "<span class='c-font-red-2'>*</span>&nbsp;" : string.Empty;
        }

        private string GetSubText()
        {
            if (string.IsNullOrWhiteSpace(SubText))
            {
                SubText = Localization.GetString(ResourceKey + ".SubText", this);
            }

            return SubText;
        }
    }
}