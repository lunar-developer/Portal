using System;
using System.Web.UI.WebControls;
using Modules.Application.Global;
using Telerik.Web.UI;

namespace DesktopModules.Modules.Application.Controls
{
    public partial class SectionAutoPayInfo : ApplicationFormModuleBase
    {
        protected override void OnLoad(EventArgs e)
        {
            if (IsPostBack)
            {
            }
        }

        #region PUBLIC PROPERTY

        public Button ControlQueryAccount => ctrlQueryAccount;

        public RadComboBox ControlPaymentMethod => ctrlPaymentMethod;
        public RadComboBox ControlPaymentSource => ctrlPaymentSource;
        public TextBox ControlPaymentCIFNo => ctrlPaymentCIFNo;
        public TextBox ControlPaymentAccountName => ctrlPaymentAccountName;

        public RadComboBox ControlPaymentAccountNo => ctrlPaymentAccountNo;
        public TextBox ControlPaymentBankCode => ctrlPaymentBankCode;
        public RadComboBox ControlAutoPayIndicator => ctrlAutoPayIndicator;

        #endregion
    }
}