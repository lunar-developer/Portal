using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.WebControls;
using DotNetNuke.UI.Skins.Controls;
using Modules.Disbursement.Business;
using Modules.Disbursement.Database;
using Modules.Disbursement.Enum;
using Modules.Disbursement.Global;
using Telerik.Web.UI;
using Website.Library.DataTransfer;
using Website.Library.Enum;
using Website.Library.Global;
using Modules.UserManagement.Global;

namespace DesktopModules.Modules.Disbursement
{
    public partial class DisbursementInquiry : DisbursementModuleBase
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
            BindBranchData(ddlBranch, UserInfo.UserID.ToString());
            BindStatusData(ddlStatus, new ListItem("Tất cả", "-1"));
            BindCurrencyData(ddlCurrencyCode, new ListItem("Tất cả", string.Empty));

            btnAdd.Visible = IsRoleInput();
            btnExport.Visible = false;
            DivProcessInfo.Visible = false;

            RegisterButton(btnPreapprove, DisbursementStatusEnum.Preapproved);
            RegisterButton(btnApprove, DisbursementStatusEnum.Approved);
            RegisterButton(btnCancel, DisbursementStatusEnum.Canceled);
        }

        protected void Search(object sender, EventArgs e)
        {
            hidCustomerID.Value = tbCustomerID.Text.Trim();
            hidCustomerName.Value = tbCustomerName.Text.Trim();
            hidBranchID.Value = ddlBranch.SelectedValue;
            hidStatus.Value = ddlStatus.SelectedValue;
            hidProcessDate.Value = calProcessDate.SelectedDate == null
                ? "-1"
                : calProcessDate.SelectedDate?.ToString(PatternEnum.Date);
            hidCurrencyCode.Value = ddlCurrencyCode.SelectedValue;
            
            BindGrid();
            SetPermission();
        }

        private void BindGrid(int pageIndex = 0)
        {
            gridData.Visible = true;
            gridData.DataSource = LoadData();
            gridData.CurrentPageIndex = pageIndex;
            gridData.DataBind();
        }

        private void SetPermission()
        {
            DivProcessInfo.Visible = true; // allow inner control can set attribute visible

            bool isRolePreapprove = IsRolePreapprove();
            btnExport.Visible = true;
            btnPreapprove.Visible = hidStatus.Value == DisbursementStatusEnum.Revised && isRolePreapprove;
            btnCancel.Visible = hidStatus.Value == DisbursementStatusEnum.RequestApproved && isRolePreapprove;
            btnApprove.Visible = hidStatus.Value == DisbursementStatusEnum.Preapproved && IsRoleApprove();

            DivProcessInfo.Visible = btnPreapprove.Visible || btnApprove.Visible || btnCancel.Visible;
        }

        private DataTable LoadData()
        {
            Dictionary<string, SQLParameterData> parameterDictionary = new Dictionary<string, SQLParameterData>
            {
                { DisbursementTable.CustomerID, new SQLParameterData(hidCustomerID.Value, SqlDbType.VarChar) },
                { DisbursementTable.CustomerName, new SQLParameterData(hidCustomerName.Value, SqlDbType.NVarChar) },
                { DisbursementTable.BranchID, new SQLParameterData(hidBranchID.Value, SqlDbType.Int) },
                { DisbursementTable.DisbursementStatus, new SQLParameterData(hidStatus.Value, SqlDbType.Int) },
                { DisbursementTable.ProcessDate, new SQLParameterData(hidProcessDate.Value, SqlDbType.Int) },
                { DisbursementTable.CurrencyCode, new SQLParameterData(hidCurrencyCode.Value, SqlDbType.VarChar) }
            };
            return DisbursementBusiness.QueryDisbursement(parameterDictionary);
        }

        protected void OnPageIndexChanging(object sender, GridPageChangedEventArgs e)
        {
            BindGrid(e.NewPageIndex);
        }

        protected void OnPageSizeChanging(object sender, GridPageSizeChangedEventArgs e)
        {
            BindGrid();
        }

        protected void Export(object sender, EventArgs e)
        {
            string[] columnName = {
                "TTKD",
                "Tên KH",
                "Mã KH",
                "Số tiền",
                "Loại tiền",
                "Lãi suất",
                "Thời hạn vay",
                "Mục đích vay",
                "Phương thức vay",
                "Ngày ĐK",
                "Ngày GN",
                "Hình thức GN",
                "Loại KH",
                "Trạng thái",
                "Ghi chú"
            };
            DataTable dt = new DataTable();
            dt.Clear();
            for (int i = 0; i < columnName.Length; i++) {
                dt.Columns.Add(columnName[i]);
            }

            DataTable dataTable = LoadData();
            
            foreach (DataRow row in dataTable.Rows)
            {
                DataRow newRow = dt.NewRow();

                newRow[columnName[0]] = UserManagementModuleBase.FormatBranchID(row[DisbursementTable.BranchID].ToString());
                newRow[columnName[1]] = row[DisbursementTable.CustomerName].ToString();
                newRow[columnName[2]] = row[DisbursementTable.CustomerID].ToString();
                newRow[columnName[3]] = row[DisbursementTable.Amount].ToString();
                newRow[columnName[4]] = "704".Equals(row[DisbursementTable.CurrencyCode].ToString()) ? "VND" : "USD"; 
                newRow[columnName[5]] = row[DisbursementTable.InterestRate].ToString();
                newRow[columnName[6]] = FunctionBase.FormatDate(row[DisbursementTable.LoanExpire].ToString());
                newRow[columnName[7]] = row[DisbursementTable.DisbursementPurpose].ToString();
                newRow[columnName[8]] = row[DisbursementTable.LoanMethod].ToString();
                newRow[columnName[9]] = FunctionBase.FormatDate(row[DisbursementTable.CreateDateTime].ToString());
                newRow[columnName[10]] = FunctionBase.FormatDate(row[DisbursementTable.DisbursementDate].ToString());
                newRow[columnName[11]] = "CK".Equals(row[DisbursementTable.DisbursementMethod].ToString()) ? "Chuyển khoản" : "Tiền mặt";
                newRow[columnName[12]] = "E".Equals(row[DisbursementTable.CustomerType].ToString()) ? "Mới": "Hiện hữu";
                newRow[columnName[13]] = GetStatusDescription(row[DisbursementTable.DisbursementStatus].ToString());
                newRow[columnName[14]] = "0".Equals(row[DisbursementTable.Note].ToString()) ? "GN & TN trong ngày" : "GN nội bộ";
                dt.Rows.Add(newRow);
            }
            ExportToExcel(dt, "Disbursement");
        }

        protected void Create(object sender, EventArgs e)
        {
            string script = EditUrl("Edit", 1024, 768, false);
            RegisterScript(script);
        }

        protected void Edit(object sender, EventArgs e)
        {
            LinkButton linkButton = sender as LinkButton;
            if (linkButton == null)
            {
                return;
            }

            string disbursementID = linkButton.CommandArgument;
            string script = EditUrl("Edit", 1024, 768, false, DisbursementTable.DisbursementID, disbursementID);
            RegisterScript(script);
        }

        protected void Submit(object sender, EventArgs e)
        {
            Button button = sender as Button;
            if (button == null)
            {
                return;
            }
            if (gridData.SelectedItems.Count == 0)
            {
                ShowAlertDialog("Vui lòng chọn ít nhất một record cần xử lý");
                return;
            }

            List<string> listID = new List<string>();
            foreach (GridDataItem item in gridData.SelectedItems)
            {
                listID.Add(item.GetDataKeyValue("DisbursementID").ToString());
            }
            Dictionary<string, SQLParameterData> parametrDictionary =
                GetProcessData(string.Join(",", listID), button.CommandArgument);
            int result = DisbursementBusiness.ProcessDisbursement(parametrDictionary);
            string message = GetAction(button.CommandArgument);
            ShowResult(result, message);
        }

        private Dictionary<string, SQLParameterData> GetProcessData(string listID, string nextStatus)
        {
            int isSensitiveInfo = IsSensitiveStatus(nextStatus) ? 1 : 0;
            return new Dictionary<string, SQLParameterData>
            {
                { DisbursementTable.DisbursementID, new SQLParameterData(listID, SqlDbType.VarChar) },
                { "CurrentStatus", new SQLParameterData(hidStatus.Value, SqlDbType.Int) },
                { DisbursementTable.DisbursementStatus, new SQLParameterData(nextStatus, SqlDbType.Int) },
                { DisbursementTable.Remark, new SQLParameterData(tbRemark.Text.Trim(), SqlDbType.NVarChar) },
                { DisbursementTable.IsSensitiveInfo, new SQLParameterData(isSensitiveInfo, SqlDbType.Bit) },
                { DisbursementTable.ModifyUserID, new SQLParameterData(UserInfo.UserID, SqlDbType.Int) },
                {
                    DisbursementTable.ModifyDateTime,
                    new SQLParameterData(DateTime.Now.ToString(PatternEnum.DateTime), SqlDbType.BigInt)
                },
            };
        }

        private void ShowResult(int result, string message)
        {
            if (result > 0)
            {
                BindGrid();
                ShowAlertDialog(message + " thành công");
            }
            else
            {
                ShowAlertDialog(message + " thất bại");
            }
        }
    }
}