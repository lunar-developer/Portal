using System;
using System.Web.UI.WebControls;
using DotNetNuke.Web.UI.WebControls;
using Modules.Application.Global;
using Telerik.Web.UI;

namespace DesktopModules.Modules.Application.Controls
{
    public partial class SectionAssessmentInfo : ApplicationFormModuleBase
    {
        protected override void OnLoad(EventArgs e)
        {
            if (IsPostBack)
            {
            }
        }

        #region PUBLIC PROPERTY

        public RadComboBox ControlDecisionCode => ctrlDecisionCode;
        public RadComboBox ControlDecisionReason => ctrlDecisionReason;
        public TextBox ControlAssessmentContent => ctrlAssessmentContent;
        public TextBox ControlAssessmentDisplayContent => ctrlAssessmentDisplayContent;
        public TextBox ControlProposeLimit => ctrlProposeLimit;
        public TextBox ControlCreditLimit => ctrlCreditLimit;
        public TextBox ControlProposeInstallmentLimit => ctrlProposeInstallmentLimit;
        public TextBox ControlInstallmentLimit => ctrlInstallmentLimit;

        public RadComboBox ControlAssessmentBranchCode => ctrlAssessmentBranchCode;
        public DnnDatePicker ControlReAssessmentDate => ctrlReAssessmentDate;
        public TextBox ControlReAssessmentReason => ctrlReAssessmentReason;

        #endregion
    }
}