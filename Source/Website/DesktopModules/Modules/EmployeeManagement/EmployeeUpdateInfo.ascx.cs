using Modules.EmployeeManagement.Global;
using System;
using DotNetNuke.UI.Skins.Controls;
using System.Collections.Generic;
using System.Data;
using Modules.EmployeeManagement.Business;
using Modules.EmployeeManagement.Database;
using Modules.EmployeeManagement.Enum;
using Website.Library.DataTransfer;
using Website.Library.Global;
using Website.Library.Enum;

namespace DesktopModules.Modules.EmployeeManagement
{
    public partial class EmployeeUpdateInfo : EmployeeManagementModuleBase
    {
        protected override void OnLoad(EventArgs e)
        {
            if (IsPostBack)
            {
                return;
            }
            BindData();
        }

        public void BindData()
        {
            DataTable dataTable = EmployeeBusiness.LoadEmployeeInfo(UserInfo.Username);
            if (dataTable.Rows.Count <= 0)
            {
                ShowMessage(MessageDefinitionEnum.NoDataFound);
                DivForm.Visible = false;
                return;
            }

            DataRow row = dataTable.Rows[0];
            txtEmployeeID.Text = row[EmployeeTable.EmployeeID].ToString();
            txtOffice.Text = row[EmployeeTable.Office].ToString();
            txtFullName.Text = row[EmployeeTable.FullName].ToString();
            txtEmail.Text = row[EmployeeTable.Email].ToString();
            txtPhoneNumber.Text = row[EmployeeTable.PhoneNumber].ToString();
            calendarDateOfBirth.SelectedDate = FunctionBase.GetDate(row[EmployeeTable.DateOfBirth].ToString());
            cbxGender.SelectedValue = row[EmployeeTable.Gender].ToString();
            txtPhoneExtendNumber.Text = row[EmployeeTable.PhoneExtendNumber].ToString();
            txtRole.Text = row[EmployeeTable.Role].ToString();
            txtBranch.Text = row[EmployeeTable.Branch].ToString();
        }

        protected void UpdateEmployee(object sender, EventArgs e)
        {
            string dateOfBirth = "0";
            if (calendarDateOfBirth.SelectedDate != null)
            {
                dateOfBirth = calendarDateOfBirth.SelectedDate.Value.ToString(PatternEnum.Date);
            }
            string qrCode = GenerateQRCode(
                txtFullName.Text,
                txtRole.Text,
                txtPhoneExtendNumber.Text,
                txtPhoneNumber.Text,
                txtOffice.Text,
                txtEmail.Text);

            Dictionary<string, SQLParameterData> parameterDictionary = new Dictionary<string, SQLParameterData>
            {
                { EmployeeTable.Email, new SQLParameterData(txtEmail.Text) },
                { EmployeeTable.Office, new SQLParameterData(txtOffice.Text, SqlDbType.NVarChar) },
                { EmployeeTable.Gender, new SQLParameterData(cbxGender.Text, SqlDbType.NVarChar) },
                { EmployeeTable.DateOfBirth, new SQLParameterData(dateOfBirth, SqlDbType.Int) },
                { EmployeeTable.PhoneNumber, new SQLParameterData(txtPhoneNumber.Text) },
                { EmployeeTable.PhoneExtendNumber, new SQLParameterData(txtPhoneExtendNumber.Text) },
                { "QRCode", new SQLParameterData(qrCode, SqlDbType.NVarChar) }
            };

            bool result = EmployeeBusiness.UpdateEmployeeInfo(parameterDictionary);
            if(result == false)
            {
                ShowMessage(MessageDefinitionEnum.UpdateEmployeeFail, ModuleMessage.ModuleMessageType.RedError);
                return;
            }
            ShowMessage(MessageDefinitionEnum.UpdateEmployeeSuccess, ModuleMessage.ModuleMessageType.GreenSuccess);
        }
    }
}