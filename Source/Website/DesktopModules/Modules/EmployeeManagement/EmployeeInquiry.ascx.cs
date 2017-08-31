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
using Modules.EmployeeManagement.DataTransfer;
using Telerik.Web.UI;

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
            BindData();
        }

        public void BindData()
        {
            cboBranch.ClearSelection();
            foreach(EmployeeBranchData branch in CacheBase.Receive<EmployeeBranchData>())
            {
                cboBranch.Items.Add(new RadComboBoxItem(branch.Branch, branch.Branch));
            }
            hidPageIndex.Value = "0";
        }

        protected void ProcessOnShowMore(object sender, EventArgs e)
        {
            hidPageIndex.Value = (int.Parse(hidPageIndex.Value) + 1).ToString();
            BindToTable();
        }

        protected void SearchEmployee(object sender, EventArgs e)
        {
            hidEmployeeID.Value = txtEmployeeID.Text.Trim();
            hidFullName.Value = txtFullName.Text.Trim();
            hidEmail.Value = txtEmail.Text.Trim();
            hidBranch.Value = cboBranch.SelectedValue;
            hidArea.Value = txtArea.Text.Trim();
            hidPageIndex.Value = "0";

            BindToTable();
        }

        private DataSet GetData()
        {
            Dictionary<string, SQLParameterData> parameterDictionary = new Dictionary<string, SQLParameterData>
            {
                { "PageIndex", new SQLParameterData(hidPageIndex.Value, SqlDbType.Int) },
                { EmployeeTable.EmployeeID, new SQLParameterData(hidEmployeeID.Value, SqlDbType.VarChar) },
                { EmployeeTable.FullName, new SQLParameterData(hidFullName.Value, SqlDbType.NVarChar) },
                { EmployeeTable.Email, new SQLParameterData(hidEmail.Value, SqlDbType.VarChar) },
                { EmployeeTable.Branch, new SQLParameterData(hidBranch.Value, SqlDbType.NVarChar) },
                { EmployeeTable.Area, new SQLParameterData(hidArea.Value, SqlDbType.NVarChar) }
            };
            return EmployeeBusiness.SearchEmployee(parameterDictionary);
        }

        private const string HTMLSearchResultTemplate = @"
            <div class='form-horizontal divRecord'>
                {0}
            </div>
            ";

        private const string HTMLSearchResultStatistic = @"<h3 class='text-center' style='line-height: 30px'>KẾT QUẢ TÌM KIẾM{0}</h3>";

        protected void BindToTable()
        {
            StringBuilder listData = new StringBuilder();
            DataSet dtTable = GetData();
            Dictionary<string, string> employeeQRDictionary = new Dictionary<string, string>();

            if (dtTable.Tables.Count <= 0 || dtTable.Tables[0].Rows.Count == 0)
            {
                divResultText.InnerHtml = string.Format(HTMLSearchResultStatistic, string.Empty);
                divResultHidden.InnerHtml = string.Format(HTMLSearchResultTemplate,
                    $"<div class='text-center c-font-bold'><i>{MessageDefinitionEnum.NoRecordFound}<i></div>");
                btnShowMore.Visible = false;
                return;
            }
            if(dtTable.Tables.Count > 1)
            {
                hidTotalRecord.Value = dtTable.Tables[1].Rows[0]["TotalRecord"].ToString();
            }         
            btnShowMore.Visible = int.Parse(hidPageIndex.Value) * 10 + 10 < int.Parse(hidTotalRecord.Value);

            
            foreach (DataRow row in dtTable.Tables[0].Rows)
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

            int currentRecord = Math.Min(int.Parse(hidPageIndex.Value) * 10 + 10, int.Parse(hidTotalRecord.Value));
            divResultText.InnerHtml = string.Format(HTMLSearchResultStatistic, $"<br>{currentRecord} / {hidTotalRecord.Value}");
            divEndStatistic.InnerHtml = $"<hr>{currentRecord} / {hidTotalRecord.Value}";
            divResultHidden.InnerHtml = string.Format(HTMLSearchResultTemplate, listData);

            if (employeeQRDictionary.Count > 0)
            {
                UpdateContactQRCode(employeeQRDictionary);
            }
        }
    }
}