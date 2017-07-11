using System;
using System.Web.UI.WebControls;
using Modules.Application.Global;
using Telerik.Web.UI;

namespace DesktopModules.Modules.Application.Controls
{
    public partial class SectionReferenceInfo : ApplicationFormModuleBase
    {
        protected override void OnLoad(EventArgs e)
        {
            if (IsPostBack)
            {
            }
        }

        #region PUBLIC PROPERTY

        public RadComboBox ControlMarialStatus => ctrlMarialStatus;
        public RadComboBox ControlContactSpouseType => ctrlContactSpouseType;
        public TextBox ControlContactSpouseName => ctrlContactSpouseName;
        public TextBox ControlContactSpouseID => ctrlContactSpouseID;
        public TextBox ControlContactSpouseMobile => ctrlContactSpouseMobile;
        public TextBox ControlContactSpouseCompanyName => ctrlContactSpouseCompanyName;
        public TextBox ControlContactSpouseRemark => ctrlContactSpouseRemark;

        public RadComboBox ControlContact01Type => ctrlContact01Type;
        public TextBox ControlContact01Name => ctrlContact01Name;
        public TextBox ControlContact01ID => ctrlContact01ID;
        public TextBox ControlContact01Mobile => ctrlContact01Mobile;
        public TextBox ControlContact01CompanyName => ctrlContact01CompanyName;
        public TextBox ControlContact01Remark => ctrlContact01Remark;

        #endregion
    }
}