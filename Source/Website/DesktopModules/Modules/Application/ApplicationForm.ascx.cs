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
using Website.Library.Enum;
using Website.Library.Global;

namespace DesktopModules.Modules.Application
{
    public partial class ApplicationForm : ApplicationFormModuleBase
    {
        private bool IsEditMode;


        protected override void OnPreRender(EventArgs e)
        {
            if (IsPostBack)
            {
                return;
            }

            string applicationID = Request.Params[ApplicationTable.ApplicationID];
            LoadData(applicationID);
        }


        protected void InsertData(object sender, EventArgs e)
        {
            Dictionary<string, string> fieldDictionary = GetData();
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

        private Dictionary<string, string> GetData()
        {
            Dictionary<string, string> fieldDictionary = new Dictionary<string, string>
            {
                #region Application Info

                { ApplicationTable.ApplicationTypeID, SectionApplicationInfo.ControlApplicationTypeID.SelectedValue },
                { ApplicationTable.Priority, SectionApplicationInfo.ControlPriority.SelectedValue },
                { ApplicationTable.ModifyUserID, UserInfo.UserID.ToString() },
                { ApplicationTable.ModifyDateTime, DateTime.Now.ToString(PatternEnum.DateTime) },

                #endregion

                #region Customer Info

                { ApplicationTable.CustomerID, SectionCustomerInfo.ControlCustomerID.Text.Trim() },
                { ApplicationTable.IdentityTypeCode, SectionCustomerInfo.ControlIdentityTypeCode.SelectedValue },
                { ApplicationTable.CIFNo, SectionCustomerInfo.ControlCIFNo.Text.Trim() },
                { ApplicationTable.IsBasicCard, SectionCustomerInfo.ControlIsBasicCard.SelectedValue },
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
                { ApplicationTable.IsCorporateCard, SectionCustomerInfo.ControlIsCorporateCard.SelectedValue },
                { ApplicationTable.CustomerType, SectionCustomerInfo.ControlCustomerType.SelectedValue },
                { ApplicationTable.CustomerClass, SectionCustomerInfo.ControlCustomerClass.SelectedValue },

                #endregion

                #region Contact Info

                { ApplicationTable.HomeAddress01, SectionContactInfo.ControlHomeAddress01.Text.Trim() },
                { ApplicationTable.HomeAddress02, SectionContactInfo.ControlHomeAddress02.Text.Trim() },
                { ApplicationTable.HomeAddress03, SectionContactInfo.ControlHomeAddress03.Text.Trim() },
                { ApplicationTable.HomeCountry, SectionContactInfo.ControlHomeCountry.SelectedValue },
                { ApplicationTable.HomeState, SectionContactInfo.ControlHomeState.SelectedValue },
                { ApplicationTable.HomeCity, SectionContactInfo.ControlHomeCity.SelectedValue },
                { ApplicationTable.HomePhone01, SectionContactInfo.ControlHomePhone01.Text.Trim() },
                { ApplicationTable.HomeRemark, SectionContactInfo.ControlHomeRemark.Text.Trim() },

                { ApplicationTable.Alternative01Address01, SectionContactInfo.ControlAlternative01Address01.Text.Trim() },
                { ApplicationTable.Alternative01Address02, SectionContactInfo.ControlAlternative01Address02.Text.Trim() },
                { ApplicationTable.Alternative01Address03, SectionContactInfo.ControlAlternative01Address03.Text.Trim() },
                { ApplicationTable.Alternative01Country, SectionContactInfo.ControlAlternative01Country.SelectedValue },
                { ApplicationTable.Alternative01State, SectionContactInfo.ControlAlternative01State.SelectedValue },
                { ApplicationTable.Alternative01City, SectionContactInfo.ControlAlternative01City.SelectedValue },
                { ApplicationTable.Alternative01Phone01, SectionContactInfo.ControlAlternative01Phone01.Text.Trim() },
                { ApplicationTable.Alternative01Remark, SectionContactInfo.ControlAlternative01Remark.Text.Trim() },

                { ApplicationTable.Alternative02Address01, SectionContactInfo.ControlAlternative02Address01.Text.Trim() },
                { ApplicationTable.Alternative02Address02, SectionContactInfo.ControlAlternative02Address02.Text.Trim() },
                { ApplicationTable.Alternative02Address03, SectionContactInfo.ControlAlternative02Address03.Text.Trim() },
                { ApplicationTable.Alternative02Country, SectionContactInfo.ControlAlternative02Country.SelectedValue },
                { ApplicationTable.Alternative02State, SectionContactInfo.ControlAlternative02State.SelectedValue },
                { ApplicationTable.Alternative02City, SectionContactInfo.ControlAlternative02City.SelectedValue },
                { ApplicationTable.Alternative02Phone01, SectionContactInfo.ControlAlternative02Phone01.Text.Trim() },
                { ApplicationTable.Alternative02Remark, SectionContactInfo.ControlAlternative02Remark.Text.Trim() },

                #endregion
            };

            return fieldDictionary;
        }

        private void SetData(DataRow row)
        {
            // Hidden Fields
            ctrlApplicationID.Value = row[ApplicationTable.ApplicationID].ToString();


            #region Application Info

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

            #region Customer Info

            SectionCustomerInfo.ControlCustomerID.Text = row[ApplicationTable.CustomerID].ToString();
            SectionCustomerInfo.ControlIdentityTypeCode.SelectedValue = row[ApplicationTable.IdentityTypeCode].ToString();
            SectionCustomerInfo.ControlCIFNo.Text = row[ApplicationTable.CIFNo].ToString();
            SectionCustomerInfo.ControlIsBasicCard.SelectedValue = row[ApplicationTable.IsBasicCard].ToString();
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
            SectionCustomerInfo.ControlIsCorporateCard.SelectedValue = row[ApplicationTable.IsCorporateCard].ToString();
            SectionCustomerInfo.ControlCustomerType.SelectedValue = row[ApplicationTable.CustomerType].ToString();
            SectionCustomerInfo.ControlCustomerClass.SelectedValue = row[ApplicationTable.CustomerClass].ToString();

            #endregion

            #region Contact Info

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


















        protected void UpdateData(object sender, EventArgs e)
        {
            Dictionary<string, string> fieldDictionary = GetData();
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
                IsEditMode = true;
                SetData(dsResult.Tables[0].Rows[0]);

                // Load History
                string historyUrl = EditUrl(ApplicationTable.ApplicationID, applicationID, "History",
                    ApplicationLogTable.ApplicationLogID, "{0}");
                SectionHistoryInfo.BindData(applicationID, historyUrl, dsResult.Tables[1]);

                // Load Route
                ctrlRoute.Items.Clear();
                foreach (DataRow row in dsResult.Tables[2].Rows)
                {
                    string text = row["ActionName"].ToString();
                    string value = row["RouteID"].ToString();
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


        private void SetDefaltValue()
        {
            #region Application Info

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

            #region Customer Info

            SectionCustomerInfo.ControlCustomerID.Text = "";
            SectionCustomerInfo.ControlIdentityTypeCode.SelectedValue = "CM";
            SectionCustomerInfo.ControlCIFNo.Text = "";
            SectionCustomerInfo.ControlIsBasicCard.SelectedValue = "B";
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
            SectionCustomerInfo.ControlIsCorporateCard.SelectedValue = "0";
            SectionCustomerInfo.ControlCustomerType.SelectedValue = "CONSUMER";
            SectionCustomerInfo.ControlCustomerClass.SelectedValue = "THUONG";

            #endregion

            #region Contact Info

            SectionContactInfo.ControlHomeAddress01.Text = "";
            SectionContactInfo.ControlHomeAddress02.Text = "";
            SectionContactInfo.ControlHomeAddress03.Text = "";
            SectionContactInfo.ControlHomeCountry.SelectedIndex = 0;
            SectionContactInfo.ControlHomeState.SelectedIndex = 0;
            SectionContactInfo.ControlHomeCity.SelectedIndex = 0;
            SectionContactInfo.ControlHomePhone01.Text = "";
            SectionContactInfo.ControlHomeRemark.Text = "";

            SectionContactInfo.ControlAlternative01Address01.Text = "";
            SectionContactInfo.ControlAlternative01Address02.Text = "";
            SectionContactInfo.ControlAlternative01Address03.Text = "";
            SectionContactInfo.ControlAlternative01Country.SelectedIndex = 0;
            SectionContactInfo.ControlAlternative01State.SelectedIndex = 0;
            SectionContactInfo.ControlAlternative01City.SelectedIndex = 0;
            SectionContactInfo.ControlAlternative01Phone01.Text = "";
            SectionContactInfo.ControlAlternative01Remark.Text = "";

            SectionContactInfo.ControlAlternative02Address01.Text = "";
            SectionContactInfo.ControlAlternative02Address02.Text = "";
            SectionContactInfo.ControlAlternative02Address03.Text = "";
            SectionContactInfo.ControlAlternative02Country.SelectedIndex = 0;
            SectionContactInfo.ControlAlternative02State.SelectedIndex = 0;
            SectionContactInfo.ControlAlternative02City.SelectedIndex = 0;
            SectionContactInfo.ControlAlternative02Phone01.Text = "";
            SectionContactInfo.ControlAlternative02Remark.Text = "";

            #endregion
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