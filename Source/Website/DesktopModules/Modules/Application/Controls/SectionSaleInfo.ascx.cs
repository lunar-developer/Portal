using System;
using System.Web.UI.WebControls;
using Modules.Application.Global;
using Telerik.Web.UI;

namespace DesktopModules.Modules.Application.Controls
{
    public partial class SectionSaleInfo : ApplicationFormModuleBase
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
            BindBranchData(ctrlSourceBranchCode);
        }

        #region PUBLIC PROPERTY

        public RadComboBox ControlApplicationSourceCode => ctrlApplicationSourceCode;
        public RadComboBox ControlSaleMethod => ctrlSaleMethod;
        public RadComboBox ControlProgramCode => ctrlProgramCode;
        public RadComboBox ControlProcessingBranch => ctrlProcessingBranch;
        public RadComboBox ControlSourceBranchCode => ctrlSourceBranchCode;
        public RadComboBox ControlSaleChecker => ctrlSaleChecker;
        public RadComboBox ControlSaleOfficer => ctrlSaleOfficer;
        public TextBox ControlSaleStaffID => ctrlSaleStaffID;

        public RadComboBox ControlSaleSupporter => ctrlSaleSupporter;
        public TextBox ControlSaleID => ctrlSaleID;
        public TextBox ControlSaleAccount => ctrlSaleAccount;
        public TextBox ControlSaleMobile => ctrlSaleMobile;
        public TextBox ControlSaleEmail => ctrlSaleEmail;

        #endregion
    }
}