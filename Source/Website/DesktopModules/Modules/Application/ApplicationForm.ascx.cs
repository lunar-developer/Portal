using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using DotNetNuke.UI.Skins.Controls;
using Modules.Application.Business;
using Modules.Application.Database;
using Modules.Application.Enum;
using Modules.Application.Global;
using Telerik.Web.UI;
using Website.Library.Database;
using Website.Library.DataTransfer;
using Website.Library.Enum;
using Website.Library.Global;

namespace DesktopModules.Modules.Application
{
    public partial class ApplicationForm : ApplicationFormModuleBase
    {
        private string CurrentMode;
        private bool IsEditMode
        {
            get
            {
                if (string.IsNullOrWhiteSpace(CurrentMode))
                {
                    int id;
                    CurrentMode = int.TryParse(ctrlApplicationID.Value, out id) && id > 0
                        ? "EDIT" : "ADD";
                }
                return CurrentMode == "EDIT";
            }
        }


        protected override void OnPreRender(EventArgs e)
        {
            if (IsPostBack)
            {
                return;
            }

            string applicationID = Request.Params[ApplicationTable.ApplicationID];
            LoadData(applicationID);
        }

        protected void InsertApplication(object sender, EventArgs e)
        {
            Dictionary<string, string> fieldDictionary = GetInputData();
            AddDefaultValues(fieldDictionary);

            long result = ApplicationBusiness.InsertApplication(UserInfo.UserID, fieldDictionary);
            if (result > 0)
            {
                string url = $"{ApplicationEditUrl}/{ApplicationTable.ApplicationID}/{result}";
                string script = $"location.href = '{url}';";
                ShowAlertDialog("Lưu hồ sơ thành công.", "Thông báo", false, script);
            }
            else
            {
                ShowMessage("Lưu hồ sơ thất bại.", ModuleMessage.ModuleMessageType.RedError);
            }
        }

        protected void ProcessApplication(object sender, EventArgs e)
        {
            if (ctrlRoute.SelectedValue == string.Empty)
            {
                string script = $"focus(document.getElementByID('{ctrlRoute.ClientID}'))";
                ShowAlertDialog("Vui lòng chọn <b>Thao tác</b> trước khi xử lý.", null, false, script);
                return;
            }
            Process();
        }


        private void LoadData(string applicationID)
        {
            try
            {
                // Reset control state|value
                SetDefaltValue();

                // Insert or Update
                long id;
                if (long.TryParse(applicationID, out id) == false)
                {
                    return;
                }

                DataSet dsResult = ApplicationBusiness.LoadApplication(applicationID, UserInfo.UserID);
                if (dsResult.Tables.Count == 0 || dsResult.Tables[0].Rows.Count == 0)
                {
                    ShowMessage("Không tìm thấy thông tin hồ sơ.");
                    return;
                }

                // Load Data
                ctrlApplicationID.Value = applicationID;
                SetData(dsResult.Tables[0].Rows[0]);

                // Load History
                string historyUrl = EditUrl(ApplicationTable.ApplicationID, applicationID, "History",
                    ApplicationLogTable.ApplicationLogID, "{0}");
                SectionHistoryInfo.BindData(applicationID, historyUrl, dsResult.Tables[1]);

                // Load Route
                ctrlRoute.Items.Clear();
                foreach (DataRow row in dsResult.Tables[2].Rows)
                {
                    string text = row[ApplicationTable.ActionName].ToString();
                    string value = row[ApplicationTable.RouteID].ToString();
                    ctrlRoute.Items.Add(new RadComboBoxItem(text, value));
                }
            }
            finally
            {
                // Set State & Permission
                SetState();
                SetPermission();
            }
        }

        private void Process()
        {
            string nextUserID = "0";
            bool isSensitiveInfo = IsSensitiveInfo(ctrlApplicationStatus.Value);
            Dictionary<string, SQLParameterData> parameterDictionary = new Dictionary<string, SQLParameterData>
            {
                { ApplicationTable.ApplicationID, new SQLParameterData(ctrlApplicationID.Value, SqlDbType.BigInt) },
                { ApplicationTable.ProcessID, new SQLParameterData(ctrlProcessID.Value, SqlDbType.TinyInt) },
                { ApplicationTable.PhaseID, new SQLParameterData(ctrlPhaseID.Value, SqlDbType.TinyInt) },
                { ApplicationTable.ApplicationStatus, new SQLParameterData(ctrlApplicationStatus.Value, SqlDbType.TinyInt) },
                { ApplicationTable.RouteID, new SQLParameterData(ctrlRoute.SelectedValue, SqlDbType.Int) },
                { ApplicationTable.CurrentUserID, new SQLParameterData(UserInfo.UserID, SqlDbType.Int) },
                { "NextUserID", new SQLParameterData(nextUserID, SqlDbType.Int) },
                { "Remark", new SQLParameterData(ctrlRemark.Text.Trim(), SqlDbType.NVarChar) },
                { "IsSensitiveInfo", new SQLParameterData(isSensitiveInfo ? 1 : 0, SqlDbType.TinyInt) },
                { ApplicationTable.ModifyUserID, new SQLParameterData(UserInfo.UserID, SqlDbType.Int) },
                { ApplicationTable.ModifyDateTime, new SQLParameterData(DateTime.Now.ToString(PatternEnum.DateTime), SqlDbType.BigInt) }
            };

            string action = ctrlRoute.SelectedItem.Text;
            int result = ApplicationBusiness.ProcessApplication(parameterDictionary);
            if (result < 0)
            {
                ShowMessage($"{action} thất bại.", ModuleMessage.ModuleMessageType.RedError);
            }
            else if (result == 0)
            {
                ShowMessage("Trạng thái của hồ sơ đã bị thay đổi, vui lòng kiểm tra lại.");
            }
            else
            {
                ShowMessage($"{action} thành công.", ModuleMessage.ModuleMessageType.GreenSuccess);
                LoadData(ctrlApplicationID.Value);
            }
        }


        private Dictionary<string, string> GetInputData()
        {
            Dictionary<string, string> fieldDictionary = new Dictionary<string, string>
            {
                #region APPLICATION INFO

                { ApplicationTable.ApplicationTypeID, SectionApplicationInfo.ControlApplicationTypeID.SelectedValue },
                { ApplicationTable.Priority, SectionApplicationInfo.ControlPriority.SelectedValue },
                { ApplicationTable.ModifyUserID, UserInfo.UserID.ToString() },
                { ApplicationTable.ModifyDateTime, DateTime.Now.ToString(PatternEnum.DateTime) },

                #endregion

                #region CUSTOMER INFO

                { ApplicationTable.CustomerID, SectionCustomerInfo.ControlCustomerID.Text.Trim() },
                { ApplicationTable.IdentityTypeCode, SectionCustomerInfo.ControlIdentityTypeCode.SelectedValue },
                { ApplicationTable.CIFNo, SectionCustomerInfo.ControlCIFNo.Text.Trim() },
                { ApplicationTable.CardTypeIndicator, SectionCustomerInfo.ControlCardTypeIndicator.SelectedValue },
                { ApplicationTable.BasicCardNumber, SectionCustomerInfo.ControlBasicCardNumber.Text.Trim() },
                { ApplicationTable.FullName, SectionCustomerInfo.ControlFullName.Text.Trim() },
                { ApplicationTable.EmbossName, SectionCustomerInfo.ControlEmbossName.Text.Trim() },
                { ApplicationTable.OldCustomerID, SectionCustomerInfo.ControlOldCustomerID.Text.Trim() },
                { ApplicationTable.InsuranceNumber, SectionCustomerInfo.ControlInsuranceNumber.Text.Trim() },
                { ApplicationTable.Gender, SectionCustomerInfo.ControlGender.SelectedValue },
                { ApplicationTable.TitleOfAddress, SectionCustomerInfo.ControlTitleOfAddress.SelectedValue },

                { ApplicationTable.Language, SectionCustomerInfo.ControlLanguage.SelectedValue },
                { ApplicationTable.Nationality, SectionCustomerInfo.ControlNationality.SelectedValue },
                {
                    ApplicationTable.BirthDate,
                    SectionCustomerInfo.ControlBirthDate.SelectedDate?.ToString(PatternEnum.Date)
                },
                { ApplicationTable.Mobile01, SectionCustomerInfo.ControlMobile01.Text.Trim() },
                { ApplicationTable.Mobile02, SectionCustomerInfo.ControlMobile02.Text.Trim() },
                { ApplicationTable.Email01, SectionCustomerInfo.ControlEmail01.Text.Trim() },
                { ApplicationTable.Email02, SectionCustomerInfo.ControlEmail02.Text.Trim() },
                { ApplicationTable.CorporateCardIndicator, SectionCustomerInfo.ControlCorporateCardIndicator.SelectedValue },
                { ApplicationTable.CustomerType, SectionCustomerInfo.ControlCustomerType.SelectedValue },
                { ApplicationTable.CustomerClass, SectionCustomerInfo.ControlCustomerClass.SelectedValue },

                #endregion

                #region CONTACT INFO

                { ApplicationTable.HomeAddress01, SectionContactInfo.ControlHomeAddress01.Text.Trim() },
                { ApplicationTable.HomeAddress02, SectionContactInfo.ControlHomeAddress02.Text.Trim() },
                { ApplicationTable.HomeCountry, SectionContactInfo.ControlHomeCountry.SelectedValue },
                { ApplicationTable.HomeState, SectionContactInfo.ControlHomeState.SelectedValue },
                { ApplicationTable.HomeCity, SectionContactInfo.ControlHomeCity.SelectedValue },
                { ApplicationTable.HomeAddress03, SectionContactInfo.ControlHomeAddress03.Text.Trim() },
                { ApplicationTable.HomePhone01, SectionContactInfo.ControlHomePhone01.Text.Trim() },
                { ApplicationTable.HomeRemark, SectionContactInfo.ControlHomeRemark.Text.Trim() },

                { ApplicationTable.Alternative01Address01, SectionContactInfo.ControlAlternative01Address01.Text.Trim() },
                { ApplicationTable.Alternative01Address02, SectionContactInfo.ControlAlternative01Address02.Text.Trim() },
                { ApplicationTable.Alternative01Country, SectionContactInfo.ControlAlternative01Country.SelectedValue },
                { ApplicationTable.Alternative01State, SectionContactInfo.ControlAlternative01State.SelectedValue },
                { ApplicationTable.Alternative01City, SectionContactInfo.ControlAlternative01City.SelectedValue },
                { ApplicationTable.Alternative01Address03, SectionContactInfo.ControlAlternative01Address03.Text.Trim() },
                { ApplicationTable.Alternative01Phone01, SectionContactInfo.ControlAlternative01Phone01.Text.Trim() },
                { ApplicationTable.Alternative01Remark, SectionContactInfo.ControlAlternative01Remark.Text.Trim() },

                { ApplicationTable.Alternative02Address01, SectionContactInfo.ControlAlternative02Address01.Text.Trim() },
                { ApplicationTable.Alternative02Address02, SectionContactInfo.ControlAlternative02Address02.Text.Trim() },
                { ApplicationTable.Alternative02Country, SectionContactInfo.ControlAlternative02Country.SelectedValue },
                { ApplicationTable.Alternative02State, SectionContactInfo.ControlAlternative02State.SelectedValue },
                { ApplicationTable.Alternative02City, SectionContactInfo.ControlAlternative02City.SelectedValue },
                { ApplicationTable.Alternative02Address03, SectionContactInfo.ControlAlternative02Address03.Text.Trim() },
                { ApplicationTable.Alternative02Phone01, SectionContactInfo.ControlAlternative02Phone01.Text.Trim() },
                { ApplicationTable.Alternative02Remark, SectionContactInfo.ControlAlternative02Remark.Text.Trim() },

                #endregion

                #region AUTO PAY

                { ApplicationTable.PaymentMethod, SectionAutoPayInfo.ControlPaymentMethod.SelectedValue },
                { ApplicationTable.PaymentSource, SectionAutoPayInfo.ControlPaymentSource.SelectedValue },
                { ApplicationTable.PaymentCIFNo, SectionAutoPayInfo.ControlPaymentCIFNo.Text.Trim() },
                { ApplicationTable.PaymentAccountName, SectionAutoPayInfo.ControlPaymentAccountName.Text.Trim() },

                { ApplicationTable.PaymentAccountNo, SectionAutoPayInfo.ControlPaymentAccountNo.SelectedValue },
                { ApplicationTable.PaymentBankCode, SectionAutoPayInfo.ControlPaymentBankCode.Text.Trim() },
                { ApplicationTable.AutoPayIndicator, SectionAutoPayInfo.ControlAutoPayIndicator.SelectedValue },

                #endregion

                #region FINANCE INFO

                { ApplicationTable.StaffIndicator, SectionFinanceInfo.ControlStaffIndicator.SelectedValue },
                { ApplicationTable.StaffID, SectionFinanceInfo.ControlStaffID.Text.Trim() },
                { ApplicationTable.CompanyName, SectionFinanceInfo.ControlCompanyName.Text.Trim() },
                { ApplicationTable.CompanyTaxNo, SectionFinanceInfo.ControlCompanyTaxNo.Text.Trim() },
                { ApplicationTable.CompanyAddress01, SectionFinanceInfo.ControlCompanyAddress01.Text.Trim() },
                { ApplicationTable.CompanyAddress02, SectionFinanceInfo.ControlCompanyAddress02.Text.Trim() },
                { ApplicationTable.CompanyCountry, SectionFinanceInfo.ControlCompanyCountry.SelectedValue },
                { ApplicationTable.CompanyState, SectionFinanceInfo.ControlCompanyState.SelectedValue },
                { ApplicationTable.CompanyCity, SectionFinanceInfo.ControlCompanyCity.SelectedValue },
                { ApplicationTable.CompanyAddress03, SectionFinanceInfo.ControlCompanyAddress03.Text.Trim() },
                { ApplicationTable.CompanyPhone01, SectionFinanceInfo.ControlCompanyPhone01.Text.Trim() },
                { ApplicationTable.DepartmentName, SectionFinanceInfo.ControlDepartmentName.Text.Trim() },
                { ApplicationTable.WorkingYear, SectionFinanceInfo.ControlWorkingYear.Text.Trim() },
                { ApplicationTable.WorkingMonth, SectionFinanceInfo.ControlWorkingMonth.Text.Trim() },
                { ApplicationTable.Position, SectionFinanceInfo.ControlPosition.SelectedValue },
                { ApplicationTable.Title, SectionFinanceInfo.ControlTitle.SelectedValue },
                { ApplicationTable.CompanyRemark, SectionFinanceInfo.ControlCompanyRemark.Text.Trim() },

                { ApplicationTable.ContractType, SectionFinanceInfo.ControlContractType.SelectedValue },
                { ApplicationTable.JobCategory, SectionFinanceInfo.ControlJobCategory.SelectedValue },
                { ApplicationTable.BusinessType, SectionFinanceInfo.ControlBusinessType.SelectedValue },
                { ApplicationTable.TotalStaff, SectionFinanceInfo.ControlTotalStaff.SelectedValue },
                { ApplicationTable.BusinessSize, SectionFinanceInfo.ControlBusinessSize.SelectedValue },
                { ApplicationTable.SIC, SectionFinanceInfo.ControlSIC.SelectedValue },
                { ApplicationTable.NetIncome, SectionFinanceInfo.ControlNetIncome.Text.Trim() },
                { ApplicationTable.TotalExpense, SectionFinanceInfo.ControlTotalExpense.Text.Trim() },
                { ApplicationTable.NumOfDependent, SectionFinanceInfo.ControlNumOfDependent.Text.Trim() },
                { ApplicationTable.HasOtherBankCreditCard, SectionFinanceInfo.ControlHasOtherBankCreditCard.SelectedValue },
                { ApplicationTable.TotalBankHasCreditCard, SectionFinanceInfo.ControlTotalBankHasCreditCard.Text.Trim() },
                { ApplicationTable.HomeOwnership, SectionFinanceInfo.ControlHomeOwnership.SelectedValue },
                { ApplicationTable.Education, SectionFinanceInfo.ControlEducation.SelectedValue },
                { ApplicationTable.SecretPhrase, SectionFinanceInfo.ControlSecretPhrase.Text.Trim() },

            #endregion

            #region REFERENCE INFO

            { ApplicationTable.MarialStatus, SectionReferenceInfo.ControlMarialStatus.SelectedValue },
            { ApplicationTable.ContactSpouseType, SectionReferenceInfo.ControlContactSpouseType.SelectedValue },
            { ApplicationTable.ContactSpouseName, SectionReferenceInfo.ControlContactSpouseName.Text.Trim() },
            { ApplicationTable.ContactSpouseID, SectionReferenceInfo.ControlContactSpouseID.Text.Trim() },
            { ApplicationTable.ContactSpouseMobile, SectionReferenceInfo.ControlContactSpouseMobile.Text.Trim() },
            { ApplicationTable.ContactSpouseCompanyName, SectionReferenceInfo.ControlContactSpouseCompanyName.Text.Trim() },
            { ApplicationTable.ContactSpouseRemark, SectionReferenceInfo.ControlContactSpouseRemark.Text.Trim() },

            { ApplicationTable.Contact01Type, SectionReferenceInfo.ControlContact01Type.SelectedValue },
            { ApplicationTable.Contact01Name, SectionReferenceInfo.ControlContact01Name.Text.Trim() },
            { ApplicationTable.Contact01ID, SectionReferenceInfo.ControlContact01ID.Text.Trim() },
            { ApplicationTable.Contact01Mobile, SectionReferenceInfo.ControlContact01Mobile.Text.Trim() },
            { ApplicationTable.Contact01CompanyName, SectionReferenceInfo.ControlContact01CompanyName.Text.Trim() },
            { ApplicationTable.Contact01Remark, SectionReferenceInfo.ControlContact01Remark.Text.Trim() },

            #endregion
        };

            return fieldDictionary;
        }

        private void SetData(DataRow row)
        {
            // Hidden Fields
            ctrlApplicationID.Value = row[ApplicationTable.ApplicationID].ToString();


            #region APPLICATION INFO

            SectionApplicationInfo.ControlUniqueID.Text = row[ApplicationTable.UniqueID].ToString();
            SectionApplicationInfo.ControlApplicationID.Text = ctrlApplicationID.Value;
            SectionApplicationInfo.ControlApplicationTypeID.SelectedValue = row[ApplicationTable.ApplicationTypeID].ToString();
            SectionApplicationInfo.ControlPriority.SelectedValue = row[ApplicationTable.Priority].ToString();

            SectionApplicationInfo.ControlApplicationStatus.Text = FormatStatus(row[ApplicationTable.ApplicationStatus].ToString());
            SectionApplicationInfo.ControlDecisionCode.Text = row[ApplicationTable.DecisionCode].ToString();
            SectionApplicationInfo.ControlModifyUserID.Text = FunctionBase.FormatUserID(row[BaseTable.ModifyUserID].ToString());
            SectionApplicationInfo.ControlModifyDateTime.Text = FunctionBase.FormatDate(row[BaseTable.ModifyDateTime].ToString());

            SectionApplicationInfo.ControlApplicationRemark.Text = row[ApplicationTable.ApplicationRemark].ToString();

            #endregion

            #region CUSTOMER INFO

            SectionCustomerInfo.ControlCustomerID.Text = row[ApplicationTable.CustomerID].ToString();
            SectionCustomerInfo.ControlIdentityTypeCode.SelectedValue = row[ApplicationTable.IdentityTypeCode].ToString();
            SectionCustomerInfo.ControlCIFNo.Text = row[ApplicationTable.CIFNo].ToString();
            SectionCustomerInfo.ControlCardTypeIndicator.SelectedValue = row[ApplicationTable.CardTypeIndicator].ToString();
            SectionCustomerInfo.ControlBasicCardNumber.Text = row[ApplicationTable.BasicCardNumber].ToString();
            SectionCustomerInfo.ControlFullName.Text = row[ApplicationTable.FullName].ToString();
            SectionCustomerInfo.ControlEmbossName.Text = row[ApplicationTable.EmbossName].ToString();
            SectionCustomerInfo.ControlOldCustomerID.Text = row[ApplicationTable.OldCustomerID].ToString();
            SectionCustomerInfo.ControlInsuranceNumber.Text = row[ApplicationTable.InsuranceNumber].ToString();
            SectionCustomerInfo.ControlGender.SelectedValue = row[ApplicationTable.Gender].ToString();
            SectionCustomerInfo.ControlTitleOfAddress.SelectedValue = row[ApplicationTable.TitleOfAddress].ToString();


            SectionCustomerInfo.ControlLanguage.SelectedValue = row[ApplicationTable.Language].ToString();
            SectionCustomerInfo.ControlNationality.SelectedValue = row[ApplicationTable.Nationality].ToString();

            DateTime birthdate;
            DateTime.TryParseExact(row[ApplicationTable.BirthDate].ToString(), PatternEnum.Date,
                CultureInfo.CurrentCulture, DateTimeStyles.None, out birthdate);
            SectionCustomerInfo.ControlBirthDate.SelectedDate = birthdate;

            SectionCustomerInfo.ControlMobile01.Text = row[ApplicationTable.Mobile01].ToString();
            SectionCustomerInfo.ControlMobile02.Text = row[ApplicationTable.Mobile02].ToString();
            SectionCustomerInfo.ControlEmail01.Text = row[ApplicationTable.Email01].ToString();
            SectionCustomerInfo.ControlEmail02.Text = row[ApplicationTable.Email02].ToString();
            SectionCustomerInfo.ControlCorporateCardIndicator.SelectedValue = row[ApplicationTable.CorporateCardIndicator].ToString();
            SectionCustomerInfo.ControlCustomerType.SelectedValue = row[ApplicationTable.CustomerType].ToString();
            SectionCustomerInfo.ControlCustomerClass.SelectedValue = row[ApplicationTable.CustomerClass].ToString();

            #endregion

            #region CONTACT INFO

            // HOME ADDRESS
            SectionContactInfo.ControlHomeAddress01.Text = row[ApplicationTable.HomeAddress01].ToString();
            SectionContactInfo.ControlHomeAddress02.Text = row[ApplicationTable.HomeAddress02].ToString();
            SectionContactInfo.ControlHomeAddress03.Text = row[ApplicationTable.HomeAddress03].ToString();

            string value = row[ApplicationTable.HomeCountry].ToString();
            SectionContactInfo.ControlHomeCountry.SelectedValue = value;
            BindState(SectionContactInfo.ControlHomeCountry);
            SectionContactInfo.ControlHomeState.SelectedValue = row[ApplicationTable.HomeState].ToString();
            BindCity(value, SectionContactInfo.ControlHomeState);

            SectionContactInfo.ControlHomeCity.SelectedValue = row[ApplicationTable.HomeCity].ToString();
            SectionContactInfo.ControlHomePhone01.Text = row[ApplicationTable.HomePhone01].ToString();
            SectionContactInfo.ControlHomeRemark.Text = row[ApplicationTable.HomeRemark].ToString();


            // ALTERNATIVE 01 ADDRESS
            SectionContactInfo.ControlAlternative01Address01.Text = row[ApplicationTable.Alternative01Address01].ToString();
            SectionContactInfo.ControlAlternative01Address02.Text = row[ApplicationTable.Alternative01Address02].ToString();
            SectionContactInfo.ControlAlternative01Address03.Text = row[ApplicationTable.Alternative01Address03].ToString();

            value = row[ApplicationTable.Alternative01Country].ToString();
            SectionContactInfo.ControlAlternative01Country.SelectedValue = value;
            BindState(SectionContactInfo.ControlAlternative01Country);
            SectionContactInfo.ControlAlternative01State.SelectedValue = row[ApplicationTable.Alternative01State].ToString();
            BindCity(value, SectionContactInfo.ControlAlternative01State);

            SectionContactInfo.ControlAlternative01City.SelectedValue = row[ApplicationTable.Alternative01City].ToString();
            SectionContactInfo.ControlAlternative01Phone01.Text = row[ApplicationTable.Alternative01Phone01].ToString();
            SectionContactInfo.ControlAlternative01Remark.Text = row[ApplicationTable.Alternative01Remark].ToString();


            // ALTERNATIVE 02 ADDRESS
            SectionContactInfo.ControlAlternative02Address01.Text = row[ApplicationTable.Alternative02Address01].ToString();
            SectionContactInfo.ControlAlternative02Address02.Text = row[ApplicationTable.Alternative02Address02].ToString();
            SectionContactInfo.ControlAlternative02Address03.Text = row[ApplicationTable.Alternative02Address03].ToString();

            value = row[ApplicationTable.Alternative02Country].ToString();
            SectionContactInfo.ControlAlternative02Country.SelectedValue = value;
            BindState(SectionContactInfo.ControlAlternative02Country);
            SectionContactInfo.ControlAlternative02State.SelectedValue = row[ApplicationTable.Alternative02State].ToString();
            BindCity(value, SectionContactInfo.ControlAlternative02State);

            SectionContactInfo.ControlAlternative02City.SelectedValue = row[ApplicationTable.Alternative02City].ToString();
            SectionContactInfo.ControlAlternative02Phone01.Text = row[ApplicationTable.Alternative02Phone01].ToString();
            SectionContactInfo.ControlAlternative02Remark.Text = row[ApplicationTable.Alternative02Remark].ToString();

            #endregion

            #region AUTO PAY INFO

            SectionAutoPayInfo.ControlPaymentMethod.SelectedValue = row[ApplicationTable.PaymentMethod].ToString();
            SectionAutoPayInfo.ControlPaymentSource.SelectedValue = row[ApplicationTable.PaymentSource].ToString();
            SectionAutoPayInfo.ControlPaymentCIFNo.Text = row[ApplicationTable.PaymentCIFNo].ToString();
            SectionAutoPayInfo.ControlPaymentAccountName.Text = row[ApplicationTable.PaymentAccountName].ToString();

            SectionAutoPayInfo.ControlPaymentAccountNo.SelectedValue = row[ApplicationTable.PaymentAccountNo].ToString();
            SectionAutoPayInfo.ControlPaymentBankCode.Text = row[ApplicationTable.PaymentBankCode].ToString();
            SectionAutoPayInfo.ControlAutoPayIndicator.SelectedValue = row[ApplicationTable.AutoPayIndicator].ToString();

            #endregion

            #region FINANCE INFO

            SectionFinanceInfo.ControlStaffIndicator.SelectedValue = row[ApplicationTable.StaffIndicator].ToString();
            SectionFinanceInfo.ControlStaffID.Text = row[ApplicationTable.StaffID].ToString();
            SectionFinanceInfo.ControlCompanyName.Text = row[ApplicationTable.CompanyName].ToString();
            SectionFinanceInfo.ControlCompanyTaxNo.Text = row[ApplicationTable.CompanyTaxNo].ToString();
            SectionFinanceInfo.ControlCompanyAddress01.Text = row[ApplicationTable.CompanyAddress01].ToString();
            SectionFinanceInfo.ControlCompanyAddress02.Text = row[ApplicationTable.CompanyAddress02].ToString();
            SectionFinanceInfo.ControlCompanyCountry.SelectedValue = row[ApplicationTable.CompanyCountry].ToString();
            SectionFinanceInfo.ControlCompanyState.SelectedValue = row[ApplicationTable.CompanyState].ToString();
            SectionFinanceInfo.ControlCompanyCity.SelectedValue = row[ApplicationTable.CompanyCity].ToString();
            SectionFinanceInfo.ControlCompanyAddress03.Text = row[ApplicationTable.CompanyAddress03].ToString();
            SectionFinanceInfo.ControlCompanyPhone01.Text = row[ApplicationTable.CompanyPhone01].ToString();
            SectionFinanceInfo.ControlDepartmentName.Text = row[ApplicationTable.DepartmentName].ToString();
            SectionFinanceInfo.ControlWorkingYear.Text = row[ApplicationTable.WorkingYear].ToString();
            SectionFinanceInfo.ControlWorkingMonth.Text = row[ApplicationTable.WorkingMonth].ToString();
            SectionFinanceInfo.ControlPosition.SelectedValue = row[ApplicationTable.Position].ToString();
            SectionFinanceInfo.ControlTitle.SelectedValue = row[ApplicationTable.Title].ToString();
            SectionFinanceInfo.ControlCompanyRemark.Text = row[ApplicationTable.CompanyRemark].ToString();

            SectionFinanceInfo.ControlContractType.SelectedValue = row[ApplicationTable.ContractType].ToString();
            SectionFinanceInfo.ControlJobCategory.SelectedValue = row[ApplicationTable.JobCategory].ToString();
            SectionFinanceInfo.ControlBusinessType.SelectedValue = row[ApplicationTable.BusinessType].ToString();
            SectionFinanceInfo.ControlTotalStaff.SelectedValue = row[ApplicationTable.TotalStaff].ToString();
            SectionFinanceInfo.ControlBusinessSize.SelectedValue = row[ApplicationTable.BusinessSize].ToString();
            SectionFinanceInfo.ControlSIC.SelectedValue = row[ApplicationTable.SIC].ToString();
            SectionFinanceInfo.ControlNetIncome.Text = row[ApplicationTable.NetIncome].ToString();
            SectionFinanceInfo.ControlTotalExpense.Text = row[ApplicationTable.TotalExpense].ToString();
            SectionFinanceInfo.ControlNumOfDependent.Text = row[ApplicationTable.NumOfDependent].ToString();
            SectionFinanceInfo.ControlHasOtherBankCreditCard.SelectedValue = row[ApplicationTable.HasOtherBankCreditCard].ToString();
            SectionFinanceInfo.ControlTotalBankHasCreditCard.Text = row[ApplicationTable.TotalBankHasCreditCard].ToString();
            SectionFinanceInfo.ControlHomeOwnership.SelectedValue = row[ApplicationTable.HomeOwnership].ToString();
            SectionFinanceInfo.ControlEducation.SelectedValue = row[ApplicationTable.Education].ToString();
            SectionFinanceInfo.ControlSecretPhrase.Text = row[ApplicationTable.SecretPhrase].ToString();

            #endregion

            #region REFERENCE INFO

            SectionReferenceInfo.ControlMarialStatus.SelectedValue = row[ApplicationTable.MarialStatus].ToString();
            SectionReferenceInfo.ControlContactSpouseType.SelectedValue = row[ApplicationTable.ContactSpouseType].ToString();
            SectionReferenceInfo.ControlContactSpouseName.Text = row[ApplicationTable.ContactSpouseName].ToString();
            SectionReferenceInfo.ControlContactSpouseID.Text = row[ApplicationTable.ContactSpouseID].ToString();
            SectionReferenceInfo.ControlContactSpouseMobile.Text = row[ApplicationTable.ContactSpouseMobile].ToString();
            SectionReferenceInfo.ControlContactSpouseCompanyName.Text = row[ApplicationTable.ContactSpouseCompanyName].ToString();
            SectionReferenceInfo.ControlContactSpouseRemark.Text = row[ApplicationTable.ContactSpouseRemark].ToString();

            SectionReferenceInfo.ControlContact01Type.SelectedValue = row[ApplicationTable.Contact01Type].ToString();
            SectionReferenceInfo.ControlContact01Name.Text = row[ApplicationTable.Contact01Name].ToString();
            SectionReferenceInfo.ControlContact01ID.Text = row[ApplicationTable.Contact01ID].ToString();
            SectionReferenceInfo.ControlContact01Mobile.Text = row[ApplicationTable.Contact01Mobile].ToString();
            SectionReferenceInfo.ControlContact01CompanyName.Text = row[ApplicationTable.Contact01CompanyName].ToString();
            SectionReferenceInfo.ControlContact01Remark.Text = row[ApplicationTable.Contact01Remark].ToString();

            #endregion
        }

        private void AddDefaultValues(IDictionary<string, string> fieldDictionary)
        {
            string applicationTypeID = SectionApplicationInfo.ControlApplicationTypeID.SelectedValue;
            string processID = applicationTypeID == ApplicationTypeEnum.Personal
                ? ProcessEnum.Personal
                : ProcessEnum.PreApprove;
            fieldDictionary.Add(ApplicationTable.UniqueID, string.Empty);
            fieldDictionary.Add(ApplicationTable.ProcessID, processID);
            fieldDictionary.Add(ApplicationTable.PhaseID, PhaseEnum.New);
            fieldDictionary.Add(ApplicationTable.ApplicationStatus, ApplicationStatusEnum.New);
            fieldDictionary.Add(ApplicationTable.ApplicationRemark, string.Empty);
            fieldDictionary.Add(ApplicationTable.CurrentUserID, UserInfo.UserID.ToString());
            fieldDictionary.Add(ApplicationTable.PreviousUserID, "0");
            fieldDictionary.Add(ApplicationTable.CreateDateTime, fieldDictionary[ApplicationTable.ModifyDateTime]);
            fieldDictionary.Add(ApplicationTable.ExportDate, "0");
        }

        private void SetDefaltValue()
        {
            #region HIDDEN INFO

            ctrlApplicationID.Value = "";
            ctrlProcessID.Value = "";
            ctrlPhaseID.Value = "";
            ctrlApplicationStatus.Value = "";

            #endregion

            #region APPLICATION INFO

            SectionApplicationInfo.ControlUniqueID.Text = "";
            SectionApplicationInfo.ControlApplicationID.Text = "";
            SectionApplicationInfo.ControlApplicationTypeID.SelectedValue = "1";
            SectionApplicationInfo.ControlPriority.SelectedValue = "NORMAL";

            SectionApplicationInfo.ControlApplicationStatus.Text = "";
            SectionApplicationInfo.ControlDecisionCode.Text = "";
            SectionApplicationInfo.ControlModifyUserID.Text = "";
            SectionApplicationInfo.ControlModifyDateTime.Text = "";

            SectionApplicationInfo.ControlApplicationRemark.Text = "";

            #endregion

            #region CUSTOMER INFO

            SectionCustomerInfo.ControlCustomerID.Text = "";
            SectionCustomerInfo.ControlIdentityTypeCode.SelectedValue = "CM";
            SectionCustomerInfo.ControlCIFNo.Text = "";
            SectionCustomerInfo.ControlCardTypeIndicator.SelectedValue = "B";
            SectionCustomerInfo.ControlBasicCardNumber.Text = "";
            SectionCustomerInfo.ControlFullName.Text = "";
            SectionCustomerInfo.ControlEmbossName.Text = "";
            SectionCustomerInfo.ControlOldCustomerID.Text = "";
            SectionCustomerInfo.ControlInsuranceNumber.Text = "";
            SectionCustomerInfo.ControlGender.SelectedValue = "M";
            SectionCustomerInfo.ControlTitleOfAddress.SelectedValue = "MR";

            SectionCustomerInfo.ControlLanguage.SelectedValue = "1";
            SectionCustomerInfo.ControlNationality.SelectedValue = "VN";
            SectionCustomerInfo.ControlBirthDate.SelectedDate = new DateTime(DateTime.Now.Year - 30, 1, 1);
            SectionCustomerInfo.ControlMobile01.Text = "";
            SectionCustomerInfo.ControlMobile02.Text = "";
            SectionCustomerInfo.ControlEmail01.Text = "";
            SectionCustomerInfo.ControlEmail02.Text = "";
            SectionCustomerInfo.ControlCorporateCardIndicator.SelectedValue = "I";
            SectionCustomerInfo.ControlCustomerType.SelectedValue = "CONSUMER";
            SectionCustomerInfo.ControlCustomerClass.SelectedValue = "THUONG";

            #endregion

            #region CONTACT INFO

            SectionContactInfo.ControlHomeAddress01.Text = "";
            SectionContactInfo.ControlHomeAddress02.Text = "";
            SectionContactInfo.ControlHomeCountry.SelectedIndex = 0;
            SectionContactInfo.ControlHomeState.SelectedIndex = 0;
            SectionContactInfo.ControlHomeCity.SelectedIndex = 0;
            SectionContactInfo.ControlHomeAddress03.Text = "";
            SectionContactInfo.ControlHomePhone01.Text = "";
            SectionContactInfo.ControlHomeRemark.Text = "";

            SectionContactInfo.ControlAlternative01Address01.Text = "";
            SectionContactInfo.ControlAlternative01Address02.Text = "";
            SectionContactInfo.ControlAlternative01Country.SelectedIndex = 0;
            SectionContactInfo.ControlAlternative01State.SelectedIndex = 0;
            SectionContactInfo.ControlAlternative01City.SelectedIndex = 0;
            SectionContactInfo.ControlAlternative01Address03.Text = "";
            SectionContactInfo.ControlAlternative01Phone01.Text = "";
            SectionContactInfo.ControlAlternative01Remark.Text = "";

            SectionContactInfo.ControlAlternative02Address01.Text = "";
            SectionContactInfo.ControlAlternative02Address02.Text = "";
            SectionContactInfo.ControlAlternative02Country.SelectedIndex = 0;
            SectionContactInfo.ControlAlternative02State.SelectedIndex = 0;
            SectionContactInfo.ControlAlternative02City.SelectedIndex = 0;
            SectionContactInfo.ControlAlternative02Address03.Text = "";
            SectionContactInfo.ControlAlternative02Phone01.Text = "";
            SectionContactInfo.ControlAlternative02Remark.Text = "";

            #endregion

            #region AUTO PAY INFO

            SectionAutoPayInfo.ControlPaymentMethod.SelectedValue = "O";
            SectionAutoPayInfo.ControlPaymentSource.SelectedValue = "ACCOUNT";
            SectionAutoPayInfo.ControlPaymentCIFNo.Text = "";
            SectionAutoPayInfo.ControlPaymentAccountName.Text = "";

            SectionAutoPayInfo.ControlPaymentAccountNo.SelectedIndex = 0;
            SectionAutoPayInfo.ControlPaymentBankCode.Text = "";
            SectionAutoPayInfo.ControlAutoPayIndicator.SelectedIndex = 0;

            #endregion

            #region FINANCE INFO

            SectionFinanceInfo.ControlStaffIndicator.SelectedValue = "N";
            SectionFinanceInfo.ControlStaffID.Text = "";
            SectionFinanceInfo.ControlCompanyName.Text = "";
            SectionFinanceInfo.ControlCompanyTaxNo.Text = "";
            SectionFinanceInfo.ControlCompanyAddress01.Text = "";
            SectionFinanceInfo.ControlCompanyAddress02.Text = "";
            SectionFinanceInfo.ControlCompanyCountry.SelectedIndex = 0;
            SectionFinanceInfo.ControlCompanyState.SelectedIndex = 0;
            SectionFinanceInfo.ControlCompanyCity.SelectedIndex = 0;
            SectionFinanceInfo.ControlCompanyAddress03.Text = "";
            SectionFinanceInfo.ControlCompanyPhone01.Text = "";
            SectionFinanceInfo.ControlDepartmentName.Text = "";
            SectionFinanceInfo.ControlWorkingYear.Text = "";
            SectionFinanceInfo.ControlWorkingMonth.Text = "";
            SectionFinanceInfo.ControlPosition.SelectedIndex = 0;
            SectionFinanceInfo.ControlTitle.SelectedIndex = 0;
            SectionFinanceInfo.ControlCompanyRemark.Text = "";

            SectionFinanceInfo.ControlContractType.SelectedIndex = 0;
            SectionFinanceInfo.ControlJobCategory.SelectedIndex = 0;
            SectionFinanceInfo.ControlBusinessType.SelectedIndex = 0;
            SectionFinanceInfo.ControlTotalStaff.SelectedIndex = 0;
            SectionFinanceInfo.ControlBusinessSize.SelectedIndex = 0;
            SectionFinanceInfo.ControlSIC.SelectedIndex = 0;
            SectionFinanceInfo.ControlNetIncome.Text = "";
            SectionFinanceInfo.ControlTotalExpense.Text = "";
            SectionFinanceInfo.ControlNumOfDependent.Text = "";
            SectionFinanceInfo.ControlHasOtherBankCreditCard.SelectedValue = "N";
            SectionFinanceInfo.ControlTotalBankHasCreditCard.Text = "";
            SectionFinanceInfo.ControlHomeOwnership.SelectedIndex = 0;
            SectionFinanceInfo.ControlEducation.SelectedIndex = 0;
            SectionFinanceInfo.ControlSecretPhrase.Text = "";

            #endregion

            #region REFERENCE INFO

            SectionReferenceInfo.ControlMarialStatus.SelectedIndex = 0;
            SectionReferenceInfo.ControlContactSpouseType.SelectedIndex = 0;
            SectionReferenceInfo.ControlContactSpouseName.Text = "";
            SectionReferenceInfo.ControlContactSpouseID.Text = "";
            SectionReferenceInfo.ControlContactSpouseMobile.Text = "";
            SectionReferenceInfo.ControlContactSpouseCompanyName.Text = "";
            SectionReferenceInfo.ControlContactSpouseRemark.Text = "";

            SectionReferenceInfo.ControlContact01Type.SelectedIndex = 0;
            SectionReferenceInfo.ControlContact01Name.Text = "";
            SectionReferenceInfo.ControlContact01ID.Text = "";
            SectionReferenceInfo.ControlContact01Mobile.Text = "";
            SectionReferenceInfo.ControlContact01CompanyName.Text = "";
            SectionReferenceInfo.ControlContact01Remark.Text = "";

            #endregion
        }
















        protected void UpdateData(object sender, EventArgs e)
        {
            Dictionary<string, string> fieldDictionary = GetInputData();
            long result = ApplicationBusiness.UpdateApplication(
                UserInfo.UserID, ctrlApplicationID.Value, fieldDictionary);
            if (result > 0)
            {
                LoadData(ctrlApplicationID.Value);
                ShowMessage("Cập nhật hồ sơ thành công.", ModuleMessage.ModuleMessageType.GreenSuccess);
            }
            else
            {
                ShowMessage("Cập nhật hồ sơ thất bại.", ModuleMessage.ModuleMessageType.RedError);
            }
        }






        private void SetPermission()
        {
            bool isRoleInput = IsRoleInput();
            btnInsert.Visible = IsEditMode == false && isRoleInput;
            btnUpdate.Visible = IsEditMode && isRoleInput;
        }

        private void SetState()
        {
            HistoryInfo.Visible = ProcessInfo.Visible = IsEditMode;
        }



        private void BindState(RadComboBox comboboxHome)
        {
            if (comboboxHome.SelectedValue == CountryEnum.VietNam)
            {
                SectionContactInfo.ProcessOnSelectCountry(comboboxHome, null);
            }
        }

        private void BindCity(string countryCode, RadComboBox comboboxState)
        {
            if (countryCode == CountryEnum.VietNam
                && string.IsNullOrWhiteSpace(comboboxState.SelectedValue) == false)
            {
                SectionContactInfo.ProcessOnSelectState(comboboxState, null);
            }
        }
    }
}