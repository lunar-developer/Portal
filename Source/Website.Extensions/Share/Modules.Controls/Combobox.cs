using System;
using System.Web.UI;

namespace Modules.Controls
{
    public sealed class Combobox : PickList
    {
        private const string Script = @"
            $(document).ready(function()
            {{
                $('#{0}').combobox();
            }});
        ";

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            string guid = Guid.NewGuid().ToString("N");
            string script = string.Format(Script, ClientID);
            if (Page.IsPostBack)
            {
                ScriptManager.RegisterClientScriptBlock(this, GetType(), guid, script, true);
            }
            else
            {
                Page.ClientScript.RegisterStartupScript(GetType(), guid, script, true);
            }
        }
    }
}