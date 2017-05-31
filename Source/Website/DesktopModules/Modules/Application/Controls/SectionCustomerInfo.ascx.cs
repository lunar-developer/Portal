using System;
using System.Web.UI.WebControls;
using DotNetNuke.Web.UI.WebControls;
using Modules.Application.Global;

namespace DesktopModules.Modules.Application.Controls
{
    public partial class SectionCustomerInfo : ApplicationFormModuleBase
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
            BindIdentityTypeData(ctrlIdentityTypeCode);
            BindLanguageData(ctrlLanguage);
            BindCountryData(ctrlNationality);
            BindCustomerClassData(ctrlCustomerClass);
        }


        #region PUBLIC PROPERTY

        public TextBox ControlCustomerID => ctrlCustomerID;
        public DropDownList ControlIdentityTypeCode => ctrlIdentityTypeCode;
        public TextBox ControlCIFNo => ctrlCIFNo;
        public DropDownList ControlIsBasicCard => ctrlIsBasicCard;
        public TextBox ControlBasicCardNumber => ctrlBasicCardNumber;
        public TextBox ControlFullName => ctrlFullName;
        public TextBox ControlEmbossName => ctrlEmbossName;
        public TextBox ControlOldCustomerID => ctrlOldCustomerID;
        public TextBox ControlInsuranceNumber => ctrlInsuranceNumber;
        public DropDownList ControlGender => ctrlGender;
        public DropDownList ControlTitleOfAddress => ctrlTitleOfAddress;

        public DropDownList ControlLanguage => ctrlLanguage;
        public DropDownList ControlNationality => ctrlNationality;
        public DnnDatePicker ControlBirthDate => ctrlBirthDate;
        public TextBox ControlMobile01 => ctrlMobile01;
        public TextBox ControlMobile02 => ctrlMobile02;
        public TextBox ControlEmail01 => ctrlEmail01;
        public TextBox ControlEmail02 => ctrlEmail02;
        public DropDownList ControlIsCorporateCard => ctrlIsCorporateCard;
        public DropDownList ControlCustomerType => ctrlCustomerType;
        public DropDownList ControlCustomerClass => ctrlCustomerClass;

        #endregion
    }
}