using System;
using System.Web.UI.WebControls;
using DotNetNuke.Web.UI.WebControls;
using Modules.Application.Business;
using Modules.Application.Global;
using Telerik.Web.UI;
using Website.Library.Extension;

namespace DesktopModules.Modules.Application.Controls
{
    public partial class SectionCustomerInfo : ApplicationFormModuleBase
    {
        public Action<InsensitiveDictionary<string>> Callback;


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

        protected void QueryCustomer(object sender, EventArgs e)
        {
            string customerID = ctrlCustomerID.Text.Trim();
            string message;
            InsensitiveDictionary<string> customerInfo = ApplicationBusiness.QueryCustomerByIDNo(customerID, out message);
            if (customerInfo == null || customerInfo.Count == 0)
            {
                ShowAlertDialog(message);
            }
            else
            {
                Callback(customerInfo);
            }
        }


        #region PUBLIC PROPERTY

        public Button ControlQueryCustomer => ctrlQueryCustomer;

        public TextBox ControlCustomerID => ctrlCustomerID;
        public RadComboBox ControlIdentityTypeCode => ctrlIdentityTypeCode;
        public TextBox ControlCIFNo => ctrlCIFNo;
        public RadComboBox ControlCardTypeIndicator => ctrlCardTypeIndicator;
        public TextBox ControlBasicCardNumber => ctrlBasicCardNumber;
        public TextBox ControlFullName => ctrlFullName;
        public TextBox ControlEmbossName => ctrlEmbossName;
        public TextBox ControlOldCustomerID => ctrlOldCustomerID;
        public TextBox ControlInsuranceNumber => ctrlInsuranceNumber;
        public RadComboBox ControlGender => ctrlGender;
        public RadComboBox ControlTitleOfAddress => ctrlTitleOfAddress;

        public RadComboBox ControlLanguage => ctrlLanguage;
        public RadComboBox ControlNationality => ctrlNationality;
        public DnnDatePicker ControlBirthDate => ctrlBirthDate;
        public TextBox ControlMobile01 => ctrlMobile01;
        public TextBox ControlMobile02 => ctrlMobile02;
        public TextBox ControlEmail01 => ctrlEmail01;
        public TextBox ControlEmail02 => ctrlEmail02;
        public RadComboBox ControlCorporateCardIndicator => ctrlCorporateCardIndicator;
        public RadComboBox ControlCustomerType => ctrlCustomerType;
        public RadComboBox ControlCustomerClass => ctrlCustomerClass;

        #endregion
    }
}