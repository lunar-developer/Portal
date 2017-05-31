using System;
using System.Web.UI.WebControls;

namespace Modules.Controls
{
    public sealed class Combobox : DropDownList
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

        protected override object SaveViewState()
        {
            object[] allStates = new object[Items.Count + 1];
            object baseState = base.SaveViewState();
            allStates[0] = baseState;

            int i = 1;
            foreach (ListItem item in Items)
            {
                int j = 0;
                string[][] attributes = new string[item.Attributes.Count][];
                foreach (string attribute in item.Attributes.Keys)
                {
                    attributes[j++] = new[] { attribute, item.Attributes[attribute] };
                }
                allStates[i++] = attributes;
            }
            return allStates;
        }

        protected override void LoadViewState(object savedState)
        {
            if (savedState == null)
            {
                return;
            }

            object[] myState = (object[]) savedState;
            if (myState[0] != null)
            {
                base.LoadViewState(myState[0]);
            }

            int i = 1;
            foreach (ListItem item in Items)
            {
                foreach (string[] attribute in (string[][])myState[i++])
                {
                    item.Attributes[attribute[0]] = attribute[1];
                }
            }
        }
    }
}