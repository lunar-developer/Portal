using Telerik.Web.UI;

namespace Modules.Controls
{
    public sealed class AutoComplete : RadComboBox
    {
        public AutoComplete()
        {
            RenderMode = RenderMode.Lightweight;
            Filter = RadComboBoxFilter.Contains;
            MaxHeight = 200;
            OnClientDropDownOpened = "onRadComboBoxOpened";
            OnClientLoad = "onRadComboBoxLoad";
        }
    }
}