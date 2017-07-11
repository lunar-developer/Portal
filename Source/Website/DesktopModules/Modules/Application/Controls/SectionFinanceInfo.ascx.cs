using System;
using System.Web.UI.WebControls;
using Modules.Application.Enum;
using Modules.Application.Global;
using Telerik.Web.UI;

namespace DesktopModules.Modules.Application.Controls
{
    public partial class SectionFinanceInfo : ApplicationFormModuleBase
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
            BindCountryData(ctrlCompanyCountry, GetEmptyItem());
            ctrlCompanyState.Items.Add(GetEmptyItem());
            ctrlCompanyCity.Items.Add(GetEmptyItem());
        }

        public void ProcessOnSelectCountry(object sender, EventArgs e)
        {
            RadComboBox dropDownList = sender as RadComboBox;
            if (dropDownList == null)
            {
                return;
            }

            RadComboBox dropDownListState = ctrlCompanyState;
            RadComboBox dropDownListCity = ctrlCompanyCity;
            BindEmptyItem(dropDownListCity);
            if (dropDownList.SelectedValue != CountryEnum.VietNam)
            {
                BindEmptyItem(dropDownListState);
            }
            else
            {
                BindStateData(dropDownListState, GetEmptyItem());
            }
        }

        public void ProcessOnSelectState(object sender, EventArgs e)
        {
            RadComboBox dropDownList = sender as RadComboBox;
            if (dropDownList == null)
            {
                return;
            }

            string value = dropDownList.SelectedValue;
            dropDownList = ctrlCompanyCity;
            if (string.IsNullOrWhiteSpace(value))
            {
                BindEmptyItem(dropDownList);
            }
            else
            {
                BindCityData(dropDownList, value, GetEmptyItem());
            }
        }

        private void BindEmptyItem(RadComboBox dropDownList)
        {
            dropDownList.Items.Clear();
            dropDownList.Items.Add(GetEmptyItem());
        }


        #region PUBLIC PROPERTY

        public RadComboBox ControlStaffIndicator => ctrlStaffIndicator;
        public TextBox ControlStaffID => ctrlStaffID;
        public TextBox ControlCompanyTaxNo => ctrlCompanyTaxNo;
        public TextBox ControlCompanyName => ctrlCompanyName;
        public TextBox ControlCompanyAddress01 => ctrlCompanyAddress01;
        public TextBox ControlCompanyAddress02 => ctrlCompanyAddress02;
        public RadComboBox ControlCompanyCountry => ctrlCompanyCountry;
        public RadComboBox ControlCompanyState => ctrlCompanyState;
        public RadComboBox ControlCompanyCity => ctrlCompanyCity;
        public TextBox ControlCompanyAddress03 => ctrlCompanyAddress03;
        public TextBox ControlCompanyPhone01 => ctrlCompanyPhone01;
        public TextBox ControlDepartmentName => ctrlDepartmentName;
        public TextBox ControlWorkingYear => ctrlWorkingYear;
        public TextBox ControlWorkingMonth => ctrlWorkingMonth;
        public RadComboBox ControlPosition => ctrlPosition;
        public RadComboBox ControlTitle => ctrlTitle;
        public TextBox ControlCompanyRemark => ctrlCompanyRemark;


        public RadComboBox ControlContractType => ctrlContractType;
        public RadComboBox ControlJobCategory => ctrlJobCategory;
        public RadComboBox ControlBusinessType => ctrlBusinessType;
        public RadComboBox ControlTotalStaff => ctrlTotalStaff;
        public RadComboBox ControlBusinessSize => ctrlBusinessSize;
        public RadComboBox ControlSIC => ctrlSIC;
        public TextBox ControlNetIncome => ctrlNetIncome;
        public TextBox ControlTotalExpense => ctrlTotalExpense;
        public TextBox ControlNumOfDependent => ctrlNumOfDependent;
        public RadComboBox ControlHasOtherBankCreditCard => ctrlHasOtherBankCreditCard;
        public TextBox ControlTotalBankHasCreditCard => ctrlTotalBankHasCreditCard;
        public RadComboBox ControlHomeOwnership => ctrlHomeOwnership;
        public RadComboBox ControlEducation => ctrlEducation;
        public TextBox ControlSecretPhrase => ctrlSecretPhrase;

        #endregion
    }
}