using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI.WebControls;
using DotNetNuke.Web.UI.WebControls;
using Modules.Application.DataTransfer;
using Modules.Application.Global;
using Telerik.Web.UI;
using Website.Library.Global;

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


        public void ProcessOnSelectDecisionCode(object sender, EventArgs e)
        {
            LoadDecisionReason();
        }

        public void ProcessOnSelectDecisionReason(object sender, EventArgs e)
        {
            IList<RadComboBoxItem> reasonList = ctrlDecisionReason.CheckedItems;
            StringBuilder hiddenRemarkBuilder = new StringBuilder();
            StringBuilder remarkBuilder = new StringBuilder();
            foreach (RadComboBoxItem item in reasonList)
            {
                DecisionReasonData decisionReason = CacheBase.Receive<DecisionReasonData>(item.Value);
                if (decisionReason == null)
                {
                    continue;
                }
                hiddenRemarkBuilder.Append(decisionReason.HiddenRemark + Environment.NewLine);
                remarkBuilder.Append(decisionReason.Remark + Environment.NewLine);
            }
            ctrlAssessmentContent.Text = hiddenRemarkBuilder.ToString();
            ctrlAssessmentDisplayContent.Text = remarkBuilder.ToString();
        }

        public void LoadDecisionReason(List<string> listSelectedValues = null)
        {
            CleanUp();

            // Rebind
            string decisionCode = ctrlDecisionCode.SelectedValue;
            if (string.IsNullOrWhiteSpace(decisionCode))
            {
                return;
            }
            BindDecisionReasonData(ctrlDecisionReason, decisionCode, listSelectedValues);
        }

        private void CleanUp()
        {
            ctrlDecisionReason.ClearSelection();
            ctrlDecisionReason.ClearCheckedItems();
            ctrlDecisionReason.Items.Clear();
            ctrlAssessmentContent.Text = string.Empty;
            ctrlAssessmentDisplayContent.Text = string.Empty;
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