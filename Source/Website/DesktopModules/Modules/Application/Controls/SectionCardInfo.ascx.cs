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
        public RadComboBox ControlDeliveryMethod => ctrlDeliveryMethod;
        public RadComboBox ControlDespatchBranchCode => ctrlDespatchBranchCode;
        public RadComboBox ControlDeliveryAddress => ctrlDeliveryAddress;

        public RadComboBox ControlStatementDeliveryType => ctrlStatementDeliveryType;
        public RadComboBox ControlStatementType => ctrlStatementType;
        public RadComboBox ControlStatementAddress => ctrlStatementAddress;

        #endregion
    }
}