using System;
using System.Web.UI.WebControls;
using Modules.Application.Global;
using Telerik.Web.UI;

namespace DesktopModules.Modules.Application.Controls
{
    public partial class SectionApplicationInfo : ApplicationFormModuleBase
    {
        protected override void OnLoad(EventArgs e)
        {
            if (IsPostBack)
            {
                return;
            }
            BindData();
        }

        private void BindData()
        {
            BindApplicationTypeData(ctrlApplicationTypeID);
        }


        #region PUBLIC PROPERTY

        public Label ControlUniqueID => valUniqueID;
        public Label ControlApplicationID => valApplicationID;
        public RadComboBox ControlApplicationTypeID => ctrlApplicationTypeID;
        public RadComboBox ControlPriority => ctrlPriority;
        public Label ControlApplicationStatus => valApplicationStatus;
        public Label ControlDecisionCode => valDecisionCode;
        public Label ControlModifyUserID => valModifyUserID;
        public Label ControlModifyDateTime => valModifyDateTime;
        public Label ControlApplicationRemark => valApplicationRemark;

        #endregion
    }
}