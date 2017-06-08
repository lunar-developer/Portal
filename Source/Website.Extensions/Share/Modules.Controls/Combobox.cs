using System;

namespace Modules.Controls
{
    public sealed class Combobox : PickList
    {
        private const string Script = @"
            addPageLoaded(function()
            {{
                $('#{0}').combobox();
            }}, true);
        ";

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            string guid = Guid.NewGuid().ToString("N");
            string script = string.Format(Script, ClientID);
            Page.ClientScript.RegisterStartupScript(GetType(), guid, script, true);
        }
    }
}