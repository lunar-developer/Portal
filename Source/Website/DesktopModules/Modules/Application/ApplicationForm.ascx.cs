using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web.UI;
using DotNetNuke.UI.Skins.Controls;
using Modules.Application.Business;
using Modules.Application.Database;
using Modules.Application.DataTransfer;
using Modules.Application.Enum;
using Modules.Application.Global;
using Telerik.Web.UI;
using Website.Library.Database;
using Website.Library.DataTransfer;
using Website.Library.Enum;
using Website.Library.Extension;
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


        protected override void OnLoad(EventArgs e)
        {
            SectionCustomerInfo.Callback = SynchronizeCustomerInfo;
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

        protected void UpdateApplication(object sender, EventArgs e)
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

        protected void ProcessApplication(object sender, EventArgs e)
        {
            if (ctrlRoute.SelectedValue == string.Empty)
            {
                string script = $"focus(document.getElementByID('{ctrlRoute.ClientID}'));";
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

                // Load Routes
                BindRoute(dsResult.Tables[2].Rows);
            }
            catch (Exception exception)
            {
                FunctionBase.LogError(exception);
            }
            finally
            {
                // Set Permission
                SetPermission();
            }
        }

        private void Process()
        {
            // Identify Next Action User
            int nextUserID;
            string actionCode = ctrlRoute.SelectedItem.Attributes[ApplicationTable.ActionCode];
            switch (actionCode)
            {
                case "ACCEPT":
                case "LOCK":
                case "UNLOCK":
                    nextUserID = UserInfo.UserID;
                    break;

                case "RETURN":
                    int.TryParse(ctrlPreviousUserID.Value, out nextUserID);
                    break;

                default:
                    int.TryParse(ctrlUser.SelectedValue, out nextUserID);
                    break;
            }

            // Identify Log Sensitive
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
            long result = ApplicationBusiness.ProcessApplication(parameterDictionary);
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


        private void AddDefaultValues(IDictionary<string, string> fieldDictionary)
        {
            string applicationTypeID = SectionApplicationInfo.ControlApplicationTypeID.SelectedValue;
            ApplicationTypeProcessData cacheItem = CacheBase.Receive<ApplicationTypeProcessData>(applicationTypeID);
            string processID = cacheItem?.ProcessID ?? ProcessEnum.Personal;

            fieldDictionary.Add(ApplicationTable.UniqueID, string.Empty);
            fieldDictionary.Add(ApplicationTable.ProcessID, processID);
            fieldDictionary.Add(ApplicationTable.PhaseID, PhaseEnum.New);
            fieldDictionary.Add(ApplicationTable.ApplicationStatus, ApplicationStatusEnum.New00);
            fieldDictionary.Add(ApplicationTable.ApplicationRemark, string.Empty);
            fieldDictionary.Add(ApplicationTable.CurrentUserID, UserInfo.UserID.ToString());
            fieldDictionary.Add(ApplicationTable.PreviousUserID, "0");
            fieldDictionary.Add(ApplicationTable.CreateDateTime, fieldDictionary[ApplicationTable.ModifyDateTime]);
            fieldDictionary.Add(ApplicationTable.ExportDate, "0");
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
                { ApplicationTable.Title, SectionFinanceInfo.ControlTitle.Text.Trim() },
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

                #region SALE INFO

                { ApplicationTable.ApplicationSourceCode, SectionSaleInfo.ControlApplicationSourceCode.SelectedValue },
                { ApplicationTable.SaleMethod, SectionSaleInfo.ControlSaleMethod.SelectedValue },
                { ApplicationTable.ProgramCode, SectionSaleInfo.ControlProgramCode.SelectedValue },
                { ApplicationTable.ProcessingBranch, SectionSaleInfo.ControlProcessingBranch.SelectedValue },
                { ApplicationTable.SourceBranchCode, SectionSaleInfo.ControlSourceBranchCode.SelectedValue },
                { ApplicationTable.SaleChecker, SectionSaleInfo.ControlSaleChecker.SelectedValue },
                { ApplicationTable.SaleOfficer, SectionSaleInfo.ControlSaleOfficer.SelectedValue },
                { ApplicationTable.SaleStaffID, SectionSaleInfo.ControlSaleStaffID.Text.Trim() },

                { ApplicationTable.SaleSupporter, SectionSaleInfo.ControlSaleSupporter.SelectedValue },
                { ApplicationTable.SaleID, SectionSaleInfo.ControlSaleID.Text.Trim() },
                { ApplicationTable.SaleAccount, SectionSaleInfo.ControlSaleAccount.Text.Trim() },
                { ApplicationTable.SaleMobile, SectionSaleInfo.ControlSaleMobile.Text.Trim() },
                { ApplicationTable.SaleEmail, SectionSaleInfo.ControlSaleEmail.Text.Trim() },

                #endregion

                #region COLLATERAL INFO

                { ApplicationTable.CollateralID, SectionCollateralInfo.ControlCollateralID.Text.Trim() },
                { ApplicationTable.CollateralValue, SectionCollateralInfo.ControlCollateralValue.Text.Trim() },
                { ApplicationTable.CollateralCreditLimit, SectionCollateralInfo.ControlCollateralCreditLimit.Text.Trim() },

                { ApplicationTable.CollateralPurpose, SectionCollateralInfo.ControlCollateralPurpose.Text.Trim() },
                { ApplicationTable.CollateralType, SectionCollateralInfo.ControlCollateralType.Text.Trim() },
                { ApplicationTable.CollateralDescription, SectionCollateralInfo.ControlCollateralDescription.Text.Trim() },

                #endregion

                #region POLICY INFO

                { ApplicationTable.PolicyCode, SectionPolicyInfo.ControlPolicyCode.SelectedValue },
                { ApplicationTable.NumOfDocument, SectionPolicyInfo.ControlNumOfDocument.Text.Trim() },
                { ApplicationTable.MembershipID, SectionPolicyInfo.ControlMembershipID.Text.Trim() },
                { ApplicationTable.GuarantorName, SectionPolicyInfo.ControlGuarantorName.Text.Trim() },

                #endregion

                #region CARD INFO

                { ApplicationTable.EmbossIndicator, SectionCardInfo.ControlEmbossIndicator.SelectedValue },
                { ApplicationTable.InstantEmbossIndicator, SectionCardInfo.ControlInstantEmbossIndicator.SelectedValue },
                { ApplicationTable.CardDeliveryMethod, SectionCardInfo.ControlCardDeliveryMethod.SelectedValue },
                { ApplicationTable.CardDespatchBranchCode, SectionCardInfo.ControlCardDespatchBranchCode.SelectedValue },
                { ApplicationTable.CardDeliveryAddress, SectionCardInfo.ControlCardDeliveryAddress.SelectedValue  },

                { ApplicationTable.StatementDeliveryMethod, SectionCardInfo.ControlStatementDeliveryMethod.SelectedValue },
                { ApplicationTable.StatementType, SectionCardInfo.ControlStatementType.SelectedValue },
                { ApplicationTable.StatementDeliveryAddress, SectionCardInfo.ControlStatementDeliveryAddress.SelectedValue },

                #endregion

                #region ASSESSMENT INFO

                { ApplicationTable.DecisionCode, SectionAssessmentInfo.ControlDecisionCode.SelectedValue },
                { ApplicationTable.DecisionReason, GetRadComboBoxSelectedValues(SectionAssessmentInfo.ControlDecisionReason) },
                { ApplicationTable.AssessmentContent, SectionAssessmentInfo.ControlAssessmentContent.Text.Trim() },
                { ApplicationTable.AssessmentDisplayContent, SectionAssessmentInfo.ControlAssessmentDisplayContent.Text.Trim() },
                { ApplicationTable.ProposeLimit, SectionAssessmentInfo.ControlProposeLimit.Text.Trim() },
                { ApplicationTable.CreditLimit, SectionAssessmentInfo.ControlCreditLimit.Text.Trim() },
                { ApplicationTable.ProposeInstallmentLimit, SectionAssessmentInfo.ControlProposeInstallmentLimit.Text.Trim() },
                { ApplicationTable.InstallmentLimit, SectionAssessmentInfo.ControlInstallmentLimit.Text.Trim() },
                { ApplicationTable.AssessmentBranchCode, SectionAssessmentInfo.ControlAssessmentBranchCode.SelectedValue },
                { ApplicationTable.ReAssessmentDate, SectionAssessmentInfo.ControlReAssessmentDate.SelectedDate?.ToString(PatternEnum.Date) },
                { ApplicationTable.ReAssessmentReason, SectionAssessmentInfo.ControlReAssessmentReason.Text.Trim() }

                #endregion
            };

            return fieldDictionary;
        }

        private void SetData(DataRow row)
        {
            #region HIDDEN INFO

            ctrlApplicationID.Value = row[ApplicationTable.ApplicationID].ToString();
            ctrlProcessID.Value = row[ApplicationTable.ProcessID].ToString();
            ctrlPhaseID.Value = row[ApplicationTable.PhaseID].ToString();
            ctrlApplicationStatus.Value = row[ApplicationTable.ApplicationStatus].ToString();
            ctrlCurrentUserID.Value = row[ApplicationTable.CurrentUserID].ToString();
            ctrlPreviousUserID.Value = row[ApplicationTable.PreviousUserID].ToString();

            #endregion

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

            BindCurrentValue(SectionAutoPayInfo.ControlPaymentAccountNo, row[ApplicationTable.PaymentAccountNo].ToString());
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
            SectionFinanceInfo.ControlTitle.Text = row[ApplicationTable.Title].ToString();
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

            #region SALE INFO

            SectionSaleInfo.ControlApplicationSourceCode.SelectedValue = row[ApplicationTable.ApplicationSourceCode].ToString();
            SectionSaleInfo.ControlSaleMethod.SelectedValue = row[ApplicationTable.SaleMethod].ToString();
            SectionSaleInfo.ControlProgramCode.SelectedValue = row[ApplicationTable.ProgramCode].ToString();
            SectionSaleInfo.ControlProcessingBranch.SelectedValue = row[ApplicationTable.ProcessingBranch].ToString();
            SectionSaleInfo.ControlSourceBranchCode.SelectedValue = row[ApplicationTable.SourceBranchCode].ToString();
            SectionSaleInfo.ControlSaleChecker.SelectedValue = row[ApplicationTable.SaleChecker].ToString();
            SectionSaleInfo.ControlSaleOfficer.SelectedValue = row[ApplicationTable.SaleOfficer].ToString();
            SectionSaleInfo.ControlSaleStaffID.Text = row[ApplicationTable.SaleStaffID].ToString();

            SectionSaleInfo.ControlSaleSupporter.SelectedValue = row[ApplicationTable.SaleSupporter].ToString();
            SectionSaleInfo.ControlSaleID.Text = row[ApplicationTable.SaleID].ToString();
            SectionSaleInfo.ControlSaleAccount.Text = row[ApplicationTable.SaleAccount].ToString();
            SectionSaleInfo.ControlSaleMobile.Text = row[ApplicationTable.SaleMobile].ToString();
            SectionSaleInfo.ControlSaleEmail.Text = row[ApplicationTable.SaleEmail].ToString();

            #endregion

            #region COLLATERAL INFO

            SectionCollateralInfo.ControlCollateralID.Text = row[ApplicationTable.CollateralID].ToString();
            SectionCollateralInfo.ControlCollateralValue.Text = row[ApplicationTable.CollateralValue].ToString();
            SectionCollateralInfo.ControlCollateralCreditLimit.Text = row[ApplicationTable.CollateralCreditLimit].ToString();

            SectionCollateralInfo.ControlCollateralPurpose.Text = row[ApplicationTable.CollateralPurpose].ToString();
            SectionCollateralInfo.ControlCollateralType.Text = row[ApplicationTable.CollateralType].ToString();
            SectionCollateralInfo.ControlCollateralDescription.Text = row[ApplicationTable.CollateralDescription].ToString();

            #endregion

            #region POLICY INFO

            SectionPolicyInfo.ControlPolicyCode.SelectedValue = row[ApplicationTable.PolicyCode].ToString();
            SectionPolicyInfo.ControlNumOfDocument.Text = row[ApplicationTable.NumOfDocument].ToString();
            SectionPolicyInfo.ControlMembershipID.Text = row[ApplicationTable.MembershipID].ToString();
            SectionPolicyInfo.ControlGuarantorName.Text = row[ApplicationTable.GuarantorName].ToString();

            #endregion

            #region CARD INFO

            SectionCardInfo.ControlEmbossIndicator.SelectedValue = row[ApplicationTable.EmbossIndicator].ToString();
            SectionCardInfo.ControlInstantEmbossIndicator.SelectedValue = row[ApplicationTable.InstantEmbossIndicator].ToString();
            SectionCardInfo.ControlCardDeliveryMethod.SelectedValue = row[ApplicationTable.CardDeliveryMethod].ToString();
            SectionCardInfo.ControlCardDespatchBranchCode.SelectedValue = row[ApplicationTable.CardDespatchBranchCode].ToString();
            SectionCardInfo.ControlCardDeliveryAddress.SelectedValue = row[ApplicationTable.CardDeliveryAddress].ToString();

            SectionCardInfo.ControlStatementDeliveryMethod.SelectedValue = row[ApplicationTable.StatementDeliveryMethod].ToString();
            SectionCardInfo.ControlStatementType.SelectedValue = row[ApplicationTable.StatementType].ToString();
            SectionCardInfo.ControlStatementDeliveryAddress.SelectedValue = row[ApplicationTable.StatementDeliveryAddress].ToString();

            #endregion

            #region ASSESSMENT INFO

            string decisionCode = row[ApplicationTable.DecisionCode].ToString();
            string decisionReason = row[ApplicationTable.DecisionReason].ToString();
            SectionAssessmentInfo.ControlDecisionCode.SelectedValue = decisionCode;
            BindDecisionReason(decisionReason);

            SectionAssessmentInfo.ControlAssessmentContent.Text = row[ApplicationTable.AssessmentContent].ToString();
            SectionAssessmentInfo.ControlAssessmentDisplayContent.Text = row[ApplicationTable.AssessmentDisplayContent].ToString();
            SectionAssessmentInfo.ControlProposeLimit.Text = row[ApplicationTable.ProposeLimit].ToString();
            SectionAssessmentInfo.ControlCreditLimit.Text = row[ApplicationTable.CreditLimit].ToString();
            SectionAssessmentInfo.ControlProposeInstallmentLimit.Text = row[ApplicationTable.ProposeInstallmentLimit].ToString();
            SectionAssessmentInfo.ControlInstallmentLimit.Text = row[ApplicationTable.InstallmentLimit].ToString();

            SectionAssessmentInfo.ControlAssessmentBranchCode.SelectedValue = row[ApplicationTable.AssessmentBranchCode].ToString();
            DateTime reassessmentDate;
            if (DateTime.TryParseExact(row[ApplicationTable.ReAssessmentDate].ToString(), PatternEnum.Date,
                CultureInfo.CurrentCulture, DateTimeStyles.None, out reassessmentDate))
            {
                SectionAssessmentInfo.ControlReAssessmentDate.SelectedDate = reassessmentDate;
            }
            SectionAssessmentInfo.ControlReAssessmentReason.Text = row[ApplicationTable.ReAssessmentReason].ToString();

            #endregion
        }

        private void SetDefaltValue()
        {
            #region HIDDEN INFO

            ctrlApplicationID.Value = "";
            ctrlProcessID.Value = "";
            ctrlPhaseID.Value = "";
            ctrlApplicationStatus.Value = "";
            ctrlCurrentUserID.Value = "";
            ctrlPreviousUserID.Value = "";
            ctrlIsRequireUpdate.Value = "0";

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
            SectionCustomerInfo.ControlBirthDate.SelectedDate = new DateTime(DateTime.Now.Year - 20, 1, 1);
            SectionCustomerInfo.ControlMobile01.Text = "";
            SectionCustomerInfo.ControlMobile02.Text = "";
            SectionCustomerInfo.ControlEmail01.Text = "";
            SectionCustomerInfo.ControlEmail02.Text = "";
            SectionCustomerInfo.ControlCorporateCardIndicator.SelectedValue = "I";
            SectionCustomerInfo.ControlCustomerType.SelectedValue = "CONSUMER";
            SectionCustomerInfo.ControlCustomerClass.SelectedValue = "B";

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
            SectionFinanceInfo.ControlTitle.Text = "";
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

            #region SALE INFO

            SectionSaleInfo.ControlApplicationSourceCode.SelectedIndex = 0;
            SectionSaleInfo.ControlSaleMethod.SelectedIndex = 0;
            SectionSaleInfo.ControlProgramCode.SelectedIndex = 0;
            SectionSaleInfo.ControlProcessingBranch.SelectedIndex = 0;
            SectionSaleInfo.ControlSourceBranchCode.SelectedIndex = 0;
            SectionSaleInfo.ControlSaleChecker.SelectedIndex = 0;
            SectionSaleInfo.ControlSaleOfficer.SelectedIndex = 0;
            SectionSaleInfo.ControlSaleStaffID.Text = "";

            SectionSaleInfo.ControlSaleSupporter.SelectedIndex = 0;
            SectionSaleInfo.ControlSaleID.Text = "";
            SectionSaleInfo.ControlSaleAccount.Text = "";
            SectionSaleInfo.ControlSaleMobile.Text = "";
            SectionSaleInfo.ControlSaleEmail.Text = "";

            #endregion

            #region COLLATERAL INFO

            SectionCollateralInfo.ControlCollateralID.Text = "";
            SectionCollateralInfo.ControlCollateralValue.Text = "";
            SectionCollateralInfo.ControlCollateralCreditLimit.Text = "";

            SectionCollateralInfo.ControlCollateralPurpose.Text = "";
            SectionCollateralInfo.ControlCollateralType.Text = "";
            SectionCollateralInfo.ControlCollateralDescription.Text = "";

            #endregion

            #region POLICY INFO

            SectionPolicyInfo.ControlPolicyCode.SelectedIndex = 0;
            SectionPolicyInfo.ControlNumOfDocument.Text = "";
            SectionPolicyInfo.ControlMembershipID.Text = "";
            SectionPolicyInfo.ControlGuarantorName.Text = "";

            #endregion

            #region CARD INFO

            SectionCardInfo.ControlEmbossIndicator.SelectedIndex = 0;
            SectionCardInfo.ControlInstantEmbossIndicator.SelectedIndex = 0;
            SectionCardInfo.ControlCardDeliveryMethod.SelectedIndex = 0;
            SectionCardInfo.ControlCardDespatchBranchCode.SelectedIndex = 0;
            SectionCardInfo.ControlCardDeliveryAddress.SelectedIndex = 0;

            SectionCardInfo.ControlStatementDeliveryMethod.SelectedIndex = 0;
            SectionCardInfo.ControlStatementType.SelectedIndex = 0;
            SectionCardInfo.ControlStatementDeliveryAddress.SelectedIndex = 0;

            #endregion

            #region ASSESSMENT INFO

            SectionAssessmentInfo.ControlDecisionCode.SelectedIndex = -1;
            SectionAssessmentInfo.ControlDecisionReason.ClearCheckedItems();
            SectionAssessmentInfo.ControlAssessmentContent.Text = "";
            SectionAssessmentInfo.ControlAssessmentDisplayContent.Text = "";
            SectionAssessmentInfo.ControlProposeLimit.Text = "";
            SectionAssessmentInfo.ControlCreditLimit.Text = "";
            SectionAssessmentInfo.ControlProposeInstallmentLimit.Text = "";
            SectionAssessmentInfo.ControlInstallmentLimit.Text = "";

            SectionAssessmentInfo.ControlAssessmentBranchCode.SelectedIndex = 0;
            SectionAssessmentInfo.ControlReAssessmentDate.SelectedDate = DateTime.Now;
            SectionAssessmentInfo.ControlReAssessmentReason.Text = "";

            #endregion

            #region HISTORY INFO

            SectionHistoryInfo.Reset();

            #endregion

            #region PROCESS INFO

            ctrlRoute.Items.Clear();
            ctrlRoute.ClearSelection();
            ctrlRoute.SelectedIndex = 0;

            ctrlUser.Items.Clear();
            ctrlUser.ClearSelection();
            ctrlUser.SelectedIndex = 0;

            ctrlRemark.Text = string.Empty;

            #endregion
        }


        private void SynchronizeCustomerInfo(InsensitiveDictionary<string> customerInfo)
        {
            // RESET VALUE
            #region CUSTOMER INFO

            SectionCustomerInfo.ControlIdentityTypeCode.SelectedIndex = 0;
            SectionCustomerInfo.ControlCIFNo.Text = "";
            SectionCustomerInfo.ControlFullName.Text = "";
            SectionCustomerInfo.ControlEmbossName.Text = "";
            SectionCustomerInfo.ControlOldCustomerID.Text = "";
            SectionCustomerInfo.ControlGender.SelectedValue = "M";
            SectionCustomerInfo.ControlTitleOfAddress.SelectedValue = "MR";

            SectionCustomerInfo.ControlNationality.SelectedValue = CountryEnum.VietNam;
            SectionCustomerInfo.ControlBirthDate.SelectedDate = new DateTime(DateTime.Now.Year - 20, 1, 1);
            SectionCustomerInfo.ControlMobile01.Text = "";
            SectionCustomerInfo.ControlEmail01.Text = "";
            SectionCustomerInfo.ControlCustomerClass.SelectedIndex = 0;

            #endregion

            #region CONTACT INFO

            SectionContactInfo.ControlHomeAddress01.Text = "";
            SectionContactInfo.ControlHomeAddress02.Text = "";
            SectionContactInfo.ControlHomeCountry.SelectedIndex = 0;
            SectionContactInfo.ControlHomeState.SelectedIndex = 0;
            SectionContactInfo.ControlHomeCity.SelectedIndex = 0;
            SectionContactInfo.ControlHomeAddress03.Text = "";
            SectionContactInfo.ControlHomePhone01.Text = "";

            #endregion

            #region AUTO PAY

            SectionAutoPayInfo.ControlPaymentCIFNo.Text = "";
            SectionAutoPayInfo.ControlPaymentAccountNo.ClearSelection();
            SectionAutoPayInfo.ControlPaymentAccountNo.Items.Clear();
            SectionAutoPayInfo.ControlPaymentAccountName.Text = "";
            SectionAutoPayInfo.ControlPaymentBankCode.Text = "";

            #endregion


            // SYNC DATA
            #region CUSTOMER INFO

            SectionCustomerInfo.ControlIdentityTypeCode.SelectedValue =
                customerInfo.GetValue(ApplicationTable.IdentityTypeCode)?.Trim();
            SectionCustomerInfo.ControlCIFNo.Text = customerInfo.GetValue(ApplicationTable.CIFNo)?.Trim();
            SectionCustomerInfo.ControlFullName.Text = customerInfo.GetValue(ApplicationTable.FullName)?.Trim();
            SectionCustomerInfo.ControlEmbossName.Text =
                ApplicationBusiness.GetEmbossingName(SectionCustomerInfo.ControlFullName.Text);
            SectionCustomerInfo.ControlOldCustomerID.Text =
                customerInfo.GetValue(ApplicationTable.OldCustomerID)?.Trim();
            SectionCustomerInfo.ControlGender.SelectedValue = customerInfo.GetValue(ApplicationTable.Gender)?.Trim();
            SectionCustomerInfo.ControlTitleOfAddress.SelectedValue =
                SectionCustomerInfo.ControlGender.SelectedValue == "M" ? "MR" : "MS";

            SectionCustomerInfo.ControlNationality.SelectedValue =
                customerInfo.GetValue(ApplicationTable.Nationality)?.Trim();

            DateTime birthdate;
            if (DateTime.TryParseExact(
                customerInfo.GetValue(ApplicationTable.BirthDate)?.Trim(), PatternEnum.Date,
                CultureInfo.CurrentCulture, DateTimeStyles.None,
                out birthdate))
            {
                SectionCustomerInfo.ControlBirthDate.SelectedDate = birthdate;
            }

            SectionCustomerInfo.ControlMobile01.Text = customerInfo.GetValue(ApplicationTable.Mobile01)?.Trim();
            SectionCustomerInfo.ControlEmail01.Text = customerInfo.GetValue(ApplicationTable.Email01)?.Trim();
            SectionCustomerInfo.ControlCustomerClass.SelectedValue =
                customerInfo.GetValue(ApplicationTable.CustomerClass)?.Trim();

            #endregion

            #region CONTACT INFO

            SectionContactInfo.ControlHomeAddress01.Text =
                customerInfo.GetValue(ApplicationTable.HomeAddress01)?.Trim();
            SectionContactInfo.ControlHomeAddress02.Text =
                customerInfo.GetValue(ApplicationTable.HomeAddress02)?.Trim();

            string countryCode = customerInfo.GetValue(ApplicationTable.HomeCountry)?.Trim();
            SectionContactInfo.ControlHomeCountry.SelectedValue = countryCode;
            BindState(SectionContactInfo.ControlHomeCountry);

            SectionContactInfo.ControlHomeState.SelectedValue =
                customerInfo.GetValue(ApplicationTable.HomeState)?.Trim();
            BindCity(countryCode, SectionContactInfo.ControlHomeState);

            SectionContactInfo.ControlHomeCity.SelectedValue =
                customerInfo.GetValue(ApplicationTable.HomeCity)?.Trim();

            SectionContactInfo.ControlHomeAddress03.Text =
                customerInfo.GetValue(ApplicationTable.HomeAddress03)?.Trim();
            SectionContactInfo.ControlHomePhone01.Text = customerInfo.GetValue(ApplicationTable.HomePhone01)?.Trim();

            UpdateContent(SectionContactInfo);
            #endregion

            #region AUTO PAY

            SectionAutoPayInfo.ControlPaymentCIFNo.Text = SectionCustomerInfo.ControlCIFNo.Text;
            UpdateContent(SectionAutoPayInfo);

            #endregion
        }


















        private void SetPermission()
        {
            bool isEditMode = IsEditMode;
            bool isCreditUser = IsCreditUser();
            bool isRoleInput = IsRoleInput();
            bool isRoleAssessment = isCreditUser && IsRoleAssessment();
            bool isRoleApproval = isCreditUser && IsRoleApproval();
            bool isRouteAvailable = isEditMode && ctrlRoute.Items.Count > 0;
            bool isOwner = ctrlCurrentUserID.Value == UserInfo.UserID.ToString();
            string status = ctrlApplicationStatus.Value;

            // Sections
            AssessmentInfo.Visible = isEditMode && (isRoleAssessment || isRoleApproval);
            HistoryInfo.Visible = isEditMode;
            ProcessInfo.Visible = isEditMode && status != ApplicationStatusEnum.Approved10 && isRouteAvailable;

            // Rows
            DivProcessUser.Visible = status == ApplicationStatusEnum.Assessing08;
            if (status == ApplicationStatusEnum.Assessing08)
            {
                BindUser();
            }

            // Fields
            SectionApplicationInfo.ControlApplicationTypeID.Enabled = isEditMode == false;

            // Query Buttons
            SectionCustomerInfo.ControlQueryCustomer.Visible = isCreditUser && status != ApplicationStatusEnum.Approved10;
            SectionAutoPayInfo.ControlQueryAccount.Visible = isCreditUser && status != ApplicationStatusEnum.Approved10;
            SectionCollateralInfo.ControlQueryCollateral.Visible = isCreditUser && status != ApplicationStatusEnum.Approved10;

            // Process Buttons
            btnInsert.Visible = isEditMode == false && isRoleInput;
            btnUpdate.Visible = isEditMode && isOwner;
            btnProcess.Visible = isRouteAvailable;
        }


        private void BindRoute(IEnumerable rows)
        {
            ctrlRoute.ClearSelection();
            ctrlRoute.Items.Clear();
            foreach (DataRow row in rows)
            {
                string text = row[ApplicationTable.ActionName].ToString();
                string value = row[ApplicationTable.RouteID].ToString();
                RadComboBoxItem item = new RadComboBoxItem(text, value);
                item.Attributes.Add(ApplicationTable.ActionCode, row[ApplicationTable.ActionCode].ToString());
                ctrlRoute.Items.Add(item);
            }
        }

        private void BindUser(params RadComboBoxItem[] additionalItems)
        {
            BindUserApproval(ctrlUser, additionalItems);
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

        private static void BindCurrentValue(RadComboBox comboBox, string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return;
            }
            comboBox.Items.Add(new RadComboBoxItem(value, value));
        }

        private void BindDecisionReason(string selectedValues)
        {
            List<string> listSelectedValues = selectedValues.Split(',').ToList();
            SectionAssessmentInfo.LoadDecisionReason(listSelectedValues);
        }

        private static void UpdateContent(Control control)
        {
            (control.Parent.Parent as UpdatePanel)?.Update();
        }
    }
}