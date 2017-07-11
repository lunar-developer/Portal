using System;
using System.Web.UI.WebControls;
using Modules.Application.Global;
using Telerik.Web.UI;

namespace DesktopModules.Modules.Application.Controls
{
    public partial class SectionPolicyInfo : ApplicationFormModuleBase
    {
        protected override void OnLoad(EventArgs e)
        {
            if (IsPostBack)
            {
            }
        }

        #region PUBLIC PROPERTY

        public RadComboBox ControlPolicyCode => ctrlPolicyCode;
        public TextBox ControlNumOfDocument => ctrlNumOfDocument;
        public TextBox ControlMembershipID => ctrlMembershipID;
        public TextBox ControlGuarantorName => ctrlGuarantorName;


        #endregion
    }
}