using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Web;
using Modules.Disbursement.Business;
using Modules.Disbursement.Database;
using Modules.Disbursement.DataTransfer;
using Modules.Disbursement.Global;
using Modules.UserManagement.Global;
using Website.Library.DataTransfer;
using Website.Library.Enum;
using OfficeOpenXml;
using System.Linq;
using Telerik.Web.UI;

namespace DesktopModules.Modules.Disbursement
{
    class ImportedData {

    }
    public partial class DisbursementCompare : DisbursementModuleBase
    {
        private const string MATCHED_SESSION_KEY_TABLE = "MATCHED_SESSION_KEY_TABLE";
        private const string UN_MATCHED_SESSION_KEY_TABLE = "UN_MATCHED_SESSION_KEY_TABLE";
        private const string ONLY_PORTAL_SESSION_KEY_TABLE = "ONLY_PORTAL_SESSION_KEY_TABLE";
        protected override void OnLoad(EventArgs e)
        {
            if (IsPostBack)
            {
                return;
            }
            InitView();

        }

        private void InitView()
        {
            calProcessDate.SelectedDate = DateTime.Now;
            HttpContext.Current.Session[ONLY_PORTAL_SESSION_KEY_TABLE] = null;
            HttpContext.Current.Session[MATCHED_SESSION_KEY_TABLE] = null;
            HttpContext.Current.Session[UN_MATCHED_SESSION_KEY_TABLE] = null;
        }

        protected void Upload(object sender, EventArgs e)
        {
            try
            {
                if (inputFile.HasFile)
                {
                    if (Path.GetExtension(inputFile.FileName) == ".xlsx" || Path.GetExtension(inputFile.FileName) == ".xls")
                    {
                        int limitFileSize = 10 * 1024 * 1024;

                        if (inputFile.FileBytes.Length > limitFileSize)
                        {
                            ShowAlertDialog("Kích cỡ tập tin tải lên lớn hơn giới hạn cho phép 10 Mb");
                            return;
                        }
                        ExcelPackage package = new ExcelPackage(inputFile.FileContent);
                        GetExcelTable(package);
                    }
                }
            }
            catch (Exception ex)
            {
                ShowAlertDialog(ex.Message, "Lỗi");
            }
        }

        protected void Grid_OnNeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            portalGrid.DataSource = HttpContext.Current.Session[ONLY_PORTAL_SESSION_KEY_TABLE];
            Grid.DataSource = HttpContext.Current.Session[MATCHED_SESSION_KEY_TABLE];
            unmatchedGrid.DataSource = HttpContext.Current.Session[UN_MATCHED_SESSION_KEY_TABLE];
        }

        private bool GetExcelTable(ExcelPackage package)
        {
            try
            {
                ExcelWorksheet workSheet = package.Workbook.Worksheets.First();
                string[] columnName = {
                    "branch",
                    "name",
                    "PsnNbr",
                    "OrgNbr",
                    "chovay",
                    "currencycd"
                };
                string selectedDate = calProcessDate.SelectedDate?.ToString(PatternEnum.Date);
                DataTable portal = DisbursementBusiness.GetDisbursementByDate(selectedDate);
                
                int cntColumn = 0;

                /*get Header*/
                foreach (var firstRowCell in workSheet.Cells[1, 1, 1, workSheet.Dimension.End.Column])
                {
                    if (firstRowCell.Text != null && firstRowCell.Text != "")
                    {
                        cntColumn++;
                    }
                    else
                    {
                        break;
                    }
                }

                List<CoreBankData> corebankData = new List<CoreBankData>();
                /*get Body*/
                for (var rowNumber = 2; rowNumber <= workSheet.Dimension.End.Row; rowNumber++)
                {
                    string date = workSheet.Cells[rowNumber, 4].Text.Trim();
                    string dataDate = DateTime.ParseExact(date, "dd-MMM-yy", null).ToString(PatternEnum.Date);
                    if (!selectedDate.Equals(dataDate))
                    {
                        ShowAlertDialog("Ngày bạn chọn và ngày trong file không giống nhau.", "Sai ngày");
                        return false;
                    }

                    string disburseCorebank = workSheet.Cells[rowNumber, 8].Text.Trim();

                    if (long.Parse(disburseCorebank) != 0) {
                        string branch = workSheet.Cells[rowNumber, 1].Text.Trim();
                        string ccCorbank = workSheet.Cells[rowNumber, 3].Text.Trim();
                        string orgNbr = workSheet.Cells[rowNumber, 5].Text.Trim();
                        string psnNbr = workSheet.Cells[rowNumber, 6].Text.Trim();
                        string customerName = workSheet.Cells[rowNumber, 7].Text.Trim();
                        corebankData.Add(new CoreBankData()
                        {
                            branchID = branch,
                            accountID = workSheet.Cells[rowNumber, 2].Text.Trim(),
                            currencyCode = "USD".Equals(ccCorbank) ? "840" : "704",
                            dateProcessed = dataDate,
                            organizationNumber = orgNbr,
                            personalNumber = psnNbr,
                            customerName = customerName,
                            amount = disburseCorebank,
                            modifyUserID = UserId.ToString(),
                            modifyDateTime = DateTime.Now.ToString(PatternEnum.DateTime)
                        });
                    }
                }

                DisbursementBusiness.InsertResult(corebankData, selectedDate);
                Compare();
                //var result =
                //from pt in portal.AsEnumerable()
                //from cb in table.AsEnumerable()
                //.Where(y =>
                //{
                //    if (y != null)
                //    {
                //        return
                //        pt.Field<String>("CustomerID").Equals(y.Field<String>("PsnNbr"))
                //        && pt.Field<String>("OrganizationID").Equals(y.Field<String>("OrgNbr"))
                //        && pt.Field<Decimal>("Amount").ToString().Equals(y.Field<String>("chovay"));
                //    }
                //    else
                //    {
                //        // return the left row
                //        return true;
                //    }
                //})
                //.DefaultIfEmpty()
                //select new
                //{
                //    DisbursementID = pt["DisbursementID"],
                //    BranchName = UserManagementModuleBase.FormatBranchID(pt["BranchID"].ToString()),
                //    CustomerName = (String)pt["CustomerName"],
                //    PsnNbr = ((null != cb) ? (String)cb["PsnNbr"] : (String)pt["CustomerID"]),
                //    OrgNbr = ((null != cb) ? (String)cb["OrgNbr"] : (String)pt["OrganizationID"]),
                //    CbAmount = (null != cb ? FunctionBase.FormatDecimal((String)cb["chovay"]) : ""),
                //    CbCurrencyCode = ((null != cb) ? (String)cb["currencycd"] : ""),
                //    PtAmount = (null == pt ? "" : FunctionBase.FormatDecimal((Decimal)pt["Amount"])),
                //    PtCurrencyCode = (null == pt ? "" : ("840".Equals((String)pt["CurrencyCode"]) ? "USD" : "VND")),
                //    IsFired = ((null != pt) ? (null != cb ? (
                //    FunctionBase.FormatDecimal((Decimal)pt["Amount"]).Equals(FunctionBase.FormatDecimal((String)cb["chovay"])) ? (
                //        (("840".Equals((String)pt["CurrencyCode"]) ? "USD" : "VND")).Equals((String)cb["currencycd"]) ? ("NO") : ("YES")) : ("YES")) : ("YES")) : ("")
                //    )
                //};


                return true;
            }
            catch (Exception ex)
            {
                ShowAlertDialog(ex.Message, "Lỗi");
                return false;
            }
        }

        private void Compare() {
            var result = DisbursementBusiness.GetComparedResult(calProcessDate.SelectedDate?.ToString(PatternEnum.Date));
            HttpContext.Current.Session.Add(ONLY_PORTAL_SESSION_KEY_TABLE, result.Tables[0]);
            portalGrid.DataSource = result.Tables[0];
            portalGrid.DataBind();

            HttpContext.Current.Session.Add(MATCHED_SESSION_KEY_TABLE, result.Tables[1]);
            Grid.DataSource = result.Tables[1];
            Grid.DataBind();

            HttpContext.Current.Session.Add(UN_MATCHED_SESSION_KEY_TABLE, result.Tables[2]);
            unmatchedGrid.DataSource = result.Tables[2];
            unmatchedGrid.DataBind();

            btnOnlyPortalAppy.Visible = true;
            btnOnlyPortalDownload.Visible = true;

            btnApply.Visible = true;
            btnDownLoadMatched.Visible = true;

            btnOnlyCorebankDownload.Visible = true;
        }

        protected void btnApply_Click(object sender, EventArgs e)
        {
            DataTable dataTable = (DataTable)HttpContext.Current.Session[MATCHED_SESSION_KEY_TABLE];
            List<string> listID = new List<string>();
            List<string> listStatus = new List<string>();
            List<string> listFires = new List<string>();

            foreach (DataRow row in dataTable.Rows) {
                listID.Add(row["DisbursementID"].ToString());
                listStatus.Add(row["DisbursementStatus"].ToString());
                string isFire = row["IsFired"].ToString();
                listFires.Add("YES".Equals(isFire)?"Y": "LATE".Equals(isFire) ? "L": "N");
            }
            string ids = string.Join(",", listID);
            string status = string.Join(",", listStatus);
            string fires = string.Join(",", listFires);
            Dictionary < string, SQLParameterData > parameters = new Dictionary<string, SQLParameterData>
            {
                { DisbursementTable.DisbursementID, new SQLParameterData(ids, SqlDbType.VarChar) },
                { "DisbursementStatus", new SQLParameterData(status, SqlDbType.VarChar) },
                { "IsAlmFires", new SQLParameterData(fires, SqlDbType.VarChar) },
                { DisbursementTable.ModifyUserID, new SQLParameterData(UserInfo.UserID, SqlDbType.Int) }
            };
            string errmsg;
            int result = DisbursementBusiness.ApplyResult(parameters, out errmsg);
            ShowAlertDialog(errmsg);
            if (result > 0) {
                Compare();
            }
        }

        protected void btnDownLoadMatched_Click(object sender, EventArgs e)
        {
            string[] columnName = {
                "TTKD",
                "Tên KH",
                "Mã KHCN",
                "Mã KHDN",
                "Số tiền GN",
                "Loại tiền GN",
                "Số tiền ĐK",
                "Loại tiền ĐK",
                "Áp phạt"
            };
            DataTable dt = new DataTable();
            dt.Clear();
            for (int i = 0; i < columnName.Length; i++)
            {
                dt.Columns.Add(columnName[i]);
            }

            DataTable dataTable = (DataTable) HttpContext.Current.Session[MATCHED_SESSION_KEY_TABLE];

            foreach (DataRow row in dataTable.Rows)
            {
                DataRow newRow = dt.NewRow();

                newRow[columnName[0]] = UserManagementModuleBase.FormatBranchID(row[DisbursementTable.BranchID].ToString());
                newRow[columnName[1]] = row[DisbursementTable.CustomerName].ToString();
                newRow[columnName[2]] = row["PsnNbr"].ToString();
                newRow[columnName[3]] = row["OrgNbr"].ToString();
                newRow[columnName[4]] = row["CbAmount"].ToString();
                newRow[columnName[5]] = "704".Equals(row["CbCurrencyCode"].ToString()) ? "VND" : "USD";
                newRow[columnName[6]] = row["PtAmount"].ToString();
                newRow[columnName[7]] = "704".Equals(row["PtCurrencyCode"].ToString()) ? "VND" : "USD";
                newRow[columnName[8]] = row["IsFired"].ToString();
                dt.Rows.Add(newRow);
            }
            ExportToExcel(dt, DateTime.Now.ToString(PatternEnum.Date)+ "_Matched_Disbursement");
        }

        protected void btnOnlyCorebankDownload_Click(object sender, EventArgs e)
        {
            string[] columnName = {
                "TTKD",
                "Tên KH",
                "Số TK",
                "Mã KHCN",
                "Mã KHDN",
                "Số tiền GN",
                "Loại tiền GN"
            };
            DataTable dt = new DataTable();
            dt.Clear();
            for (int i = 0; i < columnName.Length; i++)
            {
                dt.Columns.Add(columnName[i]);
            }

            DataTable dataTable = (DataTable)HttpContext.Current.Session[UN_MATCHED_SESSION_KEY_TABLE];

            foreach (DataRow row in dataTable.Rows)
            {
                DataRow newRow = dt.NewRow();

                newRow[columnName[0]] = UserManagementModuleBase.FormatBranchID(row[DisbursementTable.BranchID].ToString());
                newRow[columnName[1]] = row[DisbursementTable.CustomerName].ToString();
                newRow[columnName[2]] = row["accountID"].ToString();
                newRow[columnName[3]] = row["PsnNbr"].ToString();
                newRow[columnName[4]] = row["OrgNbr"].ToString();
                newRow[columnName[5]] = row["amount"].ToString();
                newRow[columnName[6]] = "704".Equals(row["CurrencyCode"].ToString()) ? "VND" : "USD";
                dt.Rows.Add(newRow);
            }
            ExportToExcel(dt, DateTime.Now.ToString(PatternEnum.Date) + "_Only_Corebank_Disbursement");
        }

        protected void btnOnlyPortalAppy_Click(object sender, EventArgs e)
        {
            DataTable dataTable = (DataTable)HttpContext.Current.Session[ONLY_PORTAL_SESSION_KEY_TABLE];
            List<string> listID = new List<string>();
            List<string> listStatus = new List<string>();
            List<string> listFires = new List<string>();

            foreach (DataRow row in dataTable.Rows)
            {
                listID.Add(row["DisbursementID"].ToString());
                listStatus.Add(row["DisbursementStatus"].ToString());
                string isFire = row["IsFired"].ToString();
                listFires.Add("YES".Equals(isFire) ? "Y" : "LATE".Equals(isFire) ? "L" : "N");
            }
            string ids = string.Join(",", listID);
            string status = string.Join(",", listStatus);
            string fires = string.Join(",", listFires);
            Dictionary<string, SQLParameterData> parameters = new Dictionary<string, SQLParameterData>
            {
                { DisbursementTable.DisbursementID, new SQLParameterData(ids, SqlDbType.VarChar) },
                { "DisbursementStatus", new SQLParameterData(status, SqlDbType.VarChar) },
                { "IsAlmFires", new SQLParameterData(fires, SqlDbType.VarChar) },
                { DisbursementTable.ModifyUserID, new SQLParameterData(UserInfo.UserID, SqlDbType.Int) }
            };
            string errmsg;
            int result = DisbursementBusiness.ApplyResult(parameters, out errmsg);
            ShowAlertDialog(errmsg);
            if (result > 0)
            {
                Compare();
            }
        }

        protected void btnOnlyPortalDownload_Click(object sender, EventArgs e)
        {
            string[] columnName = {
                "TTKD",
                "Tên KH",
                "Mã KHCN",
                "Mã KHDN",
                "Số tiền ĐK",
                "Loại tiền ĐK",
                "Áp phạt"
            };
            DataTable dt = new DataTable();
            dt.Clear();
            for (int i = 0; i < columnName.Length; i++)
            {
                dt.Columns.Add(columnName[i]);
            }

            DataTable dataTable = (DataTable)HttpContext.Current.Session[ONLY_PORTAL_SESSION_KEY_TABLE];

            foreach (DataRow row in dataTable.Rows)
            {
                DataRow newRow = dt.NewRow();

                newRow[columnName[0]] = UserManagementModuleBase.FormatBranchID(row[DisbursementTable.BranchID].ToString());
                newRow[columnName[1]] = row[DisbursementTable.CustomerName].ToString();
                newRow[columnName[2]] = row["PsnNbr"].ToString();
                newRow[columnName[3]] = row["OrgNbr"].ToString();
                newRow[columnName[4]] = row["CbAmount"].ToString();
                newRow[columnName[5]] = "704".Equals(row["PtCurrencyCode"].ToString()) ? "VND" : "USD";
                newRow[columnName[6]] = row["IsFired"].ToString();
                dt.Rows.Add(newRow);
            }
            ExportToExcel(dt, DateTime.Now.ToString(PatternEnum.Date) + "_Only_Portal_Disbursement");
        }
    }
}