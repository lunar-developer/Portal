using System;
using System.Web.UI.WebControls;
using Modules.Application.Business;
using Modules.Application.Database;
using Modules.Application.Global;
using Telerik.Web.UI;
using Website.Library.Extension;
using Website.Library.Global;

namespace DesktopModules.Modules.Application.Controls
{
    public partial class SectionCollateralInfo : ApplicationFormModuleBase
    {
        protected override void OnLoad(EventArgs e)
        {
            if (IsPostBack)
            {
            }
        }

        protected void QueryCollateral(object sender, EventArgs e)
        {
            string collateralID = ctrlCollateralID.Text.Trim();
            ctrlCollateralValue.Text = string.Empty;
            ctrlCollateralCreditLimit.Text = string.Empty;
            ctrlCollateralPurpose.Text = string.Empty;
            ctrlCollateralDescription.Text = string.Empty;

            string message;
            InsensitiveDictionary<string> collateralInfo = ApplicationBusiness.QueryCollateral(collateralID, out message);
            if (collateralInfo == null || collateralInfo.Count == 0)
            {
                ShowAlertDialog(message);
            }
            else
            {
                ctrlCollateralValue.Text =
                    FunctionBase.FormatDecimal(collateralInfo.GetValue(ApplicationTable.CollateralValue));
                ctrlCollateralCreditLimit.Text =
                    FunctionBase.FormatDecimal(collateralInfo.GetValue(ApplicationTable.CollateralCreditLimit));
                ctrlCollateralPurpose.Text = collateralInfo.GetValue(ApplicationTable.CollateralPurpose);
                ctrlCollateralDescription.Text = collateralInfo.GetValue(ApplicationTable.CollateralDescription);
            }
        }


        #region PUBLIC PROPERTY

        public Button ControlQueryCollateral => ctrlQueryCollateral;

        public TextBox ControlCollateralID => ctrlCollateralID;
        public TextBox ControlCollateralValue => ctrlCollateralValue;
        public TextBox ControlCollateralCreditLimit => ctrlCollateralCreditLimit;

        public TextBox ControlCollateralPurpose => ctrlCollateralPurpose;
        public RadComboBox ControlCollateralType => ctrlCollateralType;
        public TextBox ControlCollateralDescription => ctrlCollateralDescription;

        #endregion
    }
}