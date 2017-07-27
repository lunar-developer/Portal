using System;
using Modules.Application.Global;
using Telerik.Web.UI;

namespace DesktopModules.Modules.Application.Controls
{
    public partial class SectionCardInfo : ApplicationFormModuleBase
    {
        protected override void OnLoad(EventArgs e)
        {
            if (IsPostBack)
            {
            }
        }

        #region PUBLIC PROPERTY

        public RadComboBox ControlEmbossIndicator => ctrlEmbossIndicator;
        public RadComboBox ControlInstantEmbossIndicator => ctrlInstantEmbossIndicator;
        public RadComboBox ControlCardDeliveryMethod => ctrlCardDeliveryMethod;
        public RadComboBox ControlCardDespatchBranchCode => ctrlCardDespatchBranchCode;
        public RadComboBox ControlCardDeliveryAddress => ctrlCardDeliveryAddress;

        public RadComboBox ControlStatementDeliveryMethod => ctrlStatementDeliveryMethod;
        public RadComboBox ControlStatementType => ctrlStatementType;
        public RadComboBox ControlStatementDeliveryAddress => ctrlStatementDeliveryAddress;

        #endregion
    }
}