using System;
using System.Web.UI.WebControls;
using Modules.Application.Global;
using Telerik.Web.UI;

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

        #region PUBLIC PROPERTY

        public TextBox ControlCollateralID => ctrlCollateralID;
        public TextBox ControlCollateralValue => ctrlCollateralValue;
        public TextBox ControlCollateralRate => ctrlCollateralRate;

        public TextBox ControlCollateralPurpose => ctrlCollateralPurpose;
        public RadComboBox ControlCollateralType => ctrlCollateralType;
        public TextBox ControlCollateralDescription => ctrlCollateralDescription;

        #endregion
    }
}