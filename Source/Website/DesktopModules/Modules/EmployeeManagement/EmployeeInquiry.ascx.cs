using Modules.EmployeeManagement.Global;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Modules.EmployeeManagement.Business;
using Modules.EmployeeManagement.Database;
using Modules.EmployeeManagement.Enum;
using Website.Library.DataTransfer;
using Website.Library.Global;

namespace DesktopModules.Modules.EmployeeManagement
{
    public partial class EmployeeInquiry : EmployeeManagementModuleBase
    {
        protected override void OnLoad(EventArgs e)
        {
            if (IsPostBack)
            {
                return;
            }
            AutoWire();
        }

        protected void SearchEmployee(object sender, EventArgs e)
        {
            hidEmployeeID.Value = txtEmployeeID.Text.Trim();
            hidFullName.Value = txtFullName.Text.Trim();
            hidEmail.Value = txtEmail.Text.Trim();
            hidBranch.Value = txtBranch.Text.Trim();
            hidArea.Value = txtArea.Text.Trim();

            BindToTable();
        }

        private DataTable GetData()
        {
            Dictionary<string, SQLParameterData> parameterDictionary = new Dictionary<string, SQLParameterData>
            {
                { EmployeeTable.EmployeeID, new SQLParameterData(hidEmployeeID.Value, SqlDbType.VarChar) },
                { EmployeeTable.FullName, new SQLParameterData(hidFullName.Value, SqlDbType.NVarChar) },
                { EmployeeTable.Email, new SQLParameterData(hidEmail.Value, SqlDbType.VarChar) },
                { EmployeeTable.Branch, new SQLParameterData(hidBranch.Value, SqlDbType.NVarChar) },
                { EmployeeTable.Area, new SQLParameterData(hidArea.Value, SqlDbType.NVarChar) }
            };
            return EmployeeBusiness.SearchEmployee(parameterDictionary);
        }

        private const string HTMLSearchResultTemplate = @"
            <h3 class='text-center'>KẾT QUẢ TÌM KIẾM</h3>
            <div class='form-horizontal divRecord'>
                {0}
            </div>
            ";

        protected void BindToTable()
        {
            StringBuilder listData = new StringBuilder();
            DataTable dtTable = GetData();
            Dictionary<string, string> employeeQRDictionary = new Dictionary<string, string>();

            if (dtTable.Rows.Count <= 0)
            {
                divResultSearch.InnerHtml = string.Format(HTMLSearchResultTemplate,
                    $"<div class='text-center c-font-bold'><i>{MessageDefinitionEnum.NoRecordFound}<i></div>");
                return;
            }

            foreach (DataRow row in dtTable.Rows)
            {
                string qrCode = row[EmployeeImageTable.ContactQRCode].ToString();
                if (string.IsNullOrWhiteSpace(qrCode))
                {
                    qrCode = GenerateQRCode(row[EmployeeTable.FullName].ToString(),
                        row[EmployeeTable.Role].ToString(),
                        row[EmployeeTable.PhoneExtendNumber].ToString(),
                        row[EmployeeTable.PhoneNumber].ToString(),
                        row[EmployeeTable.Office].ToString(),
                        row[EmployeeTable.Email].ToString());
                    employeeQRDictionary.Add(row[EmployeeTable.Email].ToString(), qrCode);
                }

                StringBuilder data = new StringBuilder();
                string image = row[EmployeeTable.Image].ToString();
                if (string.IsNullOrEmpty(image))
                {
                    image = row[EmployeeTable.Gender].ToString().ToLower() == "nam"
                        ? DefaultMaleImage
                        : DefaultFemaleImage;
                }
                string srcImage = "data:image/png;base64," + image;
                data.Append($@"
                    <div class='form-group'>
                        <div class='col-lg-2 col-md-6 col-sm-12 text-center'>
                            <div class='imageHeight'>
                                <img class='imageWidth'  src='{srcImage}'/>
                            </div>
                        </div>
                        <div class='col-lg-10 col-md-6 col-md-12'>
                            <div class='col-lg-6 col-md-12 col-sm-12'>
                                <ul class='c-content-list-1 c-separator-dot c-theme'>
                                    <li><b>Mã NV:</b> {row[EmployeeTable.EmployeeID]}</li>
                                    <li><b>Họ Tên:</b> {row[EmployeeTable.FullName]}</li>
                                    <li><b>Ngày sinh:</b> {
                                        FunctionBase.FormatDate(row[EmployeeTable.DateOfBirth].ToString())
                                    }</li>
                                    <li><b>Giới tính:</b> {row[EmployeeTable.Gender]}</li>
                                    <li><b>Chức vụ:</b> {row[EmployeeTable.Role]}</li>
                                    <li><b>Đơn vị:</b> {row[EmployeeTable.Branch]}</li>
                                </ul>
                                <div class='QRImage'>
                                    <img src='{"data:image/png;base64," + qrCode}'/>
                                </div>
                            </div>
                            <div class='col-lg-6 col-md-12 col-sm-12'>
                                <ul class='c-content-list-1 c-separator-dot c-theme'>
                                    <li><b>Địa chỉ:</b> {row[EmployeeTable.Office]}</li>
                                    <li><b>Email:</b>
                                        <a class='c-theme-color' href='mailto:{row[EmployeeTable.Email]}'>
                                            {row[EmployeeTable.Email]}
                                        </a>
                                    </li>
                                    <li><b>Số điện thoại:</b>
                                        <a class='c-theme-color' href='tel:{row[EmployeeTable.PhoneNumber]}'>
                                            {FunctionBase.FormatPhoneNumber(row[EmployeeTable.PhoneNumber].ToString())}
                                        </a>
                                    </li>
                                    <li><b>Điện thoại bàn:</b> {row[EmployeeTable.PhoneExtendNumber]}</li>
                                </ul>
                            </div>
                        </div>
                    </div>");
                listData.Append(data);
            }
            divResultSearch.InnerHtml = string.Format(HTMLSearchResultTemplate, listData);

            if (employeeQRDictionary.Count > 0)
            {
                UpdateContactQRCode(employeeQRDictionary);
            }
        }
    }
}