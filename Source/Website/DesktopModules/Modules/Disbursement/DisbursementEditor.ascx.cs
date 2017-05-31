using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Web.UI.WebControls;
using DotNetNuke.UI.Skins.Controls;
using Modules.Disbursement.Business;
using Modules.Disbursement.Database;
using Modules.Disbursement.DataTransfer;
using Modules.Disbursement.Enum;
using Modules.Disbursement.Global;
using Modules.UserManagement.Business;
using Modules.UserManagement.DataTransfer;
using Modules.UserManagement.Global;
using Website.Library.DataTransfer;
using Website.Library.Enum;
using Website.Library.Global;

namespace DesktopModules.Modules.Disbursement
{
    public partial class DisbursementEditor : DisbursementModuleBase
    {
        private bool IsEditMode;

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
            RegisterConfirmDialog(btnDelete, "Bạn có chắc muốn xóa yêu cầu này?");
            RegisterButton(btnSubmit, DisbursementStatusEnum.Submited);
            RegisterButton(btnRevise, DisbursementStatusEnum.Revised);
            RegisterButton(btnPreapprove, DisbursementStatusEnum.Preapproved);
            RegisterButton(btnApprove, DisbursementStatusEnum.Approved);
            RegisterButton(btnReject, DisbursementStatusEnum.Rejected);

            RegisterButton(btnRequestCancel, DisbursementStatusEnum.RequestCancel);
            RegisterButton(btnRequestApprove, DisbursementStatusEnum.RequestApproved);
            RegisterButton(btnCancel, DisbursementStatusEnum.Canceled);

            BindCurrencyData(ddlCurrencyCode);
            LoadData(Request[DisbursementTable.DisbursementID]);
        }
        

        private void CleanData()
        {
            tbIdentifier.Text = string.Empty;
            tbCustomerName.Text = string.Empty;

            hidBranchID.Value = string.Empty;
            lblBranchID.Text = string.Empty;
            hidStatus.Value = string.Empty;
            lblStatus.Text = string.Empty;

            ddlCurrencyCode.SelectedIndex = 0;
            tbAmount.Text = string.Empty;
            tbDisbursementDate.SelectedDate = DateTime.Now;
            ddlDisbursementMethod.SelectedIndex = 0;
            tbDisbursementPurpose.Text = string.Empty;

            tbLoanMethod.Text = string.Empty;
            tbCollectAmount.Text = string.Empty;

            hidDisbursementID.Value = string.Empty;
            tbRemark.Text = string.Empty;

            IsEditMode = false;
            DivRemark.Visible = false;
            TabHistory.Visible = false;
            DivHistory.Visible = false;
        }

        private void LoadData(string id)
        {
            try
            {
                CleanData();

                if (string.IsNullOrWhiteSpace(id))
                {
                    return;
                }
                DataSet dsResult = DisbursementBusiness.GetDisbursementByID(id);
                if (dsResult.Tables.Count == 0 || dsResult.Tables[0].Rows.Count == 0)
                {
                    ShowMessage("Không tìm thấy thông tin mà bạn quan tâm.");
                    DivEditor.Visible = false;
                    return;
                }

                DataRow data = dsResult.Tables[0].Rows[0];
                if (IsAdministrator() == false &&
                    UserBusiness.IsUserOfBranch(
                        UserInfo.UserID.ToString(), int.Parse(data[DisbursementTable.BranchID].ToString())) == false)
                {
                    ShowMessage("Bạn không có quyền xem thông tin này.");
                    DivEditor.Visible = false;
                    return;
                }


                BindData(data);
                BindLogData(dsResult.Tables[1]);
                IsEditMode = true;
                TabHistory.Visible = DivHistory.Visible = true;
                DivRemark.Visible = IsEditMode
                    && hidStatus.Value != DisbursementStatusEnum.Approved
                    && hidStatus.Value != DisbursementStatusEnum.Canceled;
            }
            catch (Exception exception)
            {
                FunctionBase.LogError(exception);
            }
            finally
            {
                SetPermission();
            }
        }

        private void BindData(DataRow data)
        {
            hidDisbursementID.Value = data[DisbursementTable.DisbursementID].ToString();
            hidStatus.Value = data[DisbursementTable.DisbursementStatus].ToString();
            lblStatus.Text = GetStatusDescription(hidStatus.Value);

            tbIdentifier.Text = data[DisbursementTable.CustomerID].ToString();
            hidBranchID.Value = data[DisbursementTable.BranchID].ToString();
            lblBranchID.Text = UserManagementModuleBase.FormatBranchID(hidBranchID.Value);

            tbCustomerName.Text = data[DisbursementTable.CustomerName].ToString();
            ddlCurrencyCode.SelectedValue = data[DisbursementTable.CurrencyCode].ToString();
            tbAmount.Text = FunctionBase.FormatDecimal(data[DisbursementTable.Amount].ToString());
            tbDisbursementDate.SelectedDate = FunctionBase.GetDate(data[DisbursementTable.DisbursementDate].ToString());
            ddlDisbursementMethod.SelectedValue = data[DisbursementTable.DisbursementMethod].ToString();
            tbDisbursementPurpose.Text = data[DisbursementTable.DisbursementPurpose].ToString();
            tbLoanMethod.Text = data[DisbursementTable.LoanMethod].ToString();
            tbCollectAmount.Text = FunctionBase.FormatDecimal(data[DisbursementTable.CollectAmount].ToString());
        }

        private const string TableLog = @"
            <div class=""table-responsive"">
                <table class=""table"">
                    <colgroup>
                        <col width=""15%"" />
                        <col width=""30%"" />
                        <col width=""20%"" />
                        <col width=""35%"" />
                    </colgroup>
                    <thead>
                        <tr>
                            <th>Thời Gian</th>
                            <th>Người thực hiện</th>
                            <th>Hành động</th>
                            <th>Ghi chú</th>
                        </tr>
                    </thead>
                    <tbody>{0}</tbody>
                </table>
            </div>";

        private void BindLogData(DataTable logTable)
        {
            bool isRoleAdmin = IsAdministrator();
            StringBuilder html = new StringBuilder();
            foreach (DataRow row in logTable.Rows)
            {
                string action = GetAction(row[DisbursementTable.DisbursementStatus].ToString());
                string actionUser = FunctionBase.FormatUserID(row[DisbursementTable.ModifyUserID].ToString());
                string remark = row[DisbursementTable.Remark].ToString();
                bool isSensitiveInfo = bool.Parse(row[DisbursementTable.IsSensitiveInfo].ToString());
                if (isSensitiveInfo && isRoleAdmin == false)
                {
                    actionUser = string.Empty;
                    remark = string.Empty;
                }

                html.Append($@"
                    <tr>
                        <td>{FunctionBase.FormatDate(row[DisbursementTable.ModifyDateTime].ToString())}</td>
                        <td>{actionUser}</td>
                        <td>{action}</td>
                        <td>{remark}</td>
                    </tr>
                ");
            }
            DivLog.InnerHtml = string.Format(TableLog, html);
        }

        private void SetPermission()
        {
            if (DivEditor.Visible == false)
            {
                return;
            }

            btnCreate.Visible = btnUpdate.Visible = btnDelete.Visible = false;
            btnSubmit.Visible = btnRevise.Visible = btnPreapprove.Visible = btnApprove.Visible = false;
            btnRequestCancel.Visible = btnRequestApprove.Visible = btnCancel.Visible = false;
            btnReject.Visible = false;

            bool isRoleInput = IsRoleInput();
            bool isRoleRevise = IsRoleRevise();
            bool isRolePreapprove = IsRolePreapprove();
            bool isRoleApprove = IsRoleApprove();
            string status = hidStatus.Value;


            if (IsEditMode == false)
            {
                btnCreate.Visible = isRoleInput;
            }
            else
            {
                switch (status)
                {
                    case DisbursementStatusEnum.New:
                        btnUpdate.Visible = isRoleInput;
                        btnDelete.Visible = isRoleInput;
                        btnSubmit.Visible = isRoleInput;
                        break;

                    case DisbursementStatusEnum.Submited:
                        btnRequestCancel.Visible = isRoleInput;
                        btnUpdate.Visible = isRoleRevise;
                        btnRevise.Visible = isRoleRevise;
                        btnReject.Visible = isRoleRevise;
                        break;

                    case DisbursementStatusEnum.Revised:
                        btnRequestCancel.Visible = isRoleInput;
                        btnPreapprove.Visible = isRolePreapprove;
                        btnReject.Visible = isRolePreapprove;
                        break;

                    case DisbursementStatusEnum.Preapproved:
                        btnRequestCancel.Visible = isRoleInput;
                        btnApprove.Visible = isRoleApprove;
                        btnReject.Visible = isRoleApprove;
                        break;

                    case DisbursementStatusEnum.Approved:
                        btnRequestCancel.Visible = isRoleInput;
                        break;

                    case DisbursementStatusEnum.RequestCancel:
                        btnRequestApprove.Visible = isRoleRevise;
                        break;

                    case DisbursementStatusEnum.RequestApproved:
                        btnCancel.Visible = isRolePreapprove;
                        break;
                }
            }
        }


        private bool Validate()
        {
            if (string.IsNullOrWhiteSpace(tbIdentifier.Text))
            {
                ShowMessage("Vui lòng nhập vào <b>Mã khách hàng</b>", ModuleMessage.ModuleMessageType.YellowWarning);
                return false;
            }

            if (string.IsNullOrWhiteSpace(tbCustomerName.Text))
            {
                ShowMessage("Vui lòng nhập vào <b>Tên khách hàng</b>", ModuleMessage.ModuleMessageType.YellowWarning);
                return false;
            }

            if (string.IsNullOrWhiteSpace(tbAmount.Text))
            {
                ShowMessage("Vui lòng nhập vào <b>Số tiền</b> cần giải ngân",
                    ModuleMessage.ModuleMessageType.YellowWarning);
                return false;
            }

            if (string.IsNullOrWhiteSpace(tbDisbursementPurpose.Text))
            {
                ShowMessage("Vui lòng nhập vào <b>Mục đích vay vốn</b>", ModuleMessage.ModuleMessageType.YellowWarning);
                return false;
            }

            return true;
        }

        private DisbursementData GetData()
        {
            UserData userData = CacheBase.Receive<UserData>(UserId.ToString());
            if (userData == null || userData.BranchID == "-1")
            {
                ShowMessage("Không tìm thấy thông tin Chi nhánh của User",
                    ModuleMessage.ModuleMessageType.YellowWarning);
                return null;
            }

            DisbursementData data = new DisbursementData
            {
                CustomerID = tbIdentifier.Text.Trim(),
                CustomerName = tbCustomerName.Text.Trim(),
                BranchID = userData.BranchID,
                DisbursementStatus = DisbursementStatusEnum.New,
                Amount = tbAmount.Text.Trim().Replace(",", string.Empty),
                CurrencyCode = ddlCurrencyCode.SelectedValue,
                DisbursementDate = tbDisbursementDate.SelectedDate?.ToString(PatternEnum.Date),
                DisbursementMethod = ddlDisbursementMethod.SelectedValue,
                LoanMethod = tbLoanMethod.Text.Trim(),
                CollectAmount =
                    FunctionBase.GetCoalesceString(tbCollectAmount.Text.Trim().Replace(",", string.Empty), "0"),
                DisbursementPurpose = tbDisbursementPurpose.Text.Trim()
            };
            if (hidDisbursementID.Value == string.Empty)
            {
                data.CreateUserID = UserId.ToString();
                data.CreateDateTime = DateTime.Now.ToString(PatternEnum.DateTime);
            }
            else
            {
                data.DisbursementID = hidDisbursementID.Value;
                data.ModifyUserID = UserId.ToString();
                data.ModifyDateTime = DateTime.Now.ToString(PatternEnum.DateTime);
            }

            return data;
        }

        protected void Create(object sender, EventArgs e)
        {
            if (Validate() == false)
            {
                return;
            }

            DisbursementData data = GetData();
            if (data == null)
            {
                return;
            }

            long result = DisbursementBusiness.InsertNewRecord(data);
            if (result > 0)
            {
                hidDisbursementID.Value = result.ToString();
                LoadData(hidDisbursementID.Value);
                ShowMessage("Thêm yêu cầu thành công", ModuleMessage.ModuleMessageType.GreenSuccess);
            }
            else
            {
                ShowMessage("Thêm yêu cầu thất bại", ModuleMessage.ModuleMessageType.RedError);
            }
        }

        protected void Update(object sender, EventArgs e)
        {
            if (Validate() == false)
            {
                return;
            }

            DisbursementData data = GetData();
            if (data == null)
            {
                return;
            }

            bool result = DisbursementBusiness.UpdateRecord(data);
            if (result)
            {
                LoadData(data.DisbursementID);
                ShowMessage("Cập nhật yêu cầu thành công", ModuleMessage.ModuleMessageType.GreenSuccess);
            }
            else
            {
                ShowMessage("Cập nhật yêu cầu thất bại", ModuleMessage.ModuleMessageType.RedError);
            }
        }

        protected void Delete(object sender, EventArgs e)
        {
            bool isPopup = Request.QueryString["popUp"] == "true";
            bool result = DisbursementBusiness.DeleteRecord(hidDisbursementID.Value);
            if (result)
            {
                if (isPopup)
                {
                    RegisterScript(GetCloseScript());
                }
                else
                {
                    ShowMessage("Xóa yêu cầu thành công", ModuleMessage.ModuleMessageType.GreenSuccess);
                    DivEditor.Visible = false;
                }
            }
            else
            {
                ShowMessage("Xóa yêu cầu thất bại", ModuleMessage.ModuleMessageType.RedError);
            }
        }

        protected void Submit(object sender, EventArgs e)
        {
            Button button = sender as Button;
            if (button == null)
            {
                return;
            }

            Dictionary<string, SQLParameterData> parametrDictionary = GetProcessData(button.CommandArgument);
            int result = DisbursementBusiness.ProcessDisbursement(parametrDictionary);
            string message = GetAction(button.CommandArgument);
            ShowResult(result, message);
        }

        private Dictionary<string, SQLParameterData> GetProcessData(string nextStatus)
        {
            int isSensitiveInfo = IsSensitiveStatus(nextStatus) ? 1 : 0;
            return new Dictionary<string, SQLParameterData>
            {
                { DisbursementTable.DisbursementID, new SQLParameterData(hidDisbursementID.Value, SqlDbType.VarChar) },
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
                LoadData(hidDisbursementID.Value);
                ShowMessage(message + " thành công", ModuleMessage.ModuleMessageType.GreenSuccess);
            }
            else
            {
                ShowMessage(message + " thất bại", ModuleMessage.ModuleMessageType.RedError);
            }
        }

        protected void Cancel(object sender, EventArgs e)
        {
            Button button = sender as Button;
            if (button == null)
            {
                return;
            }

            Dictionary<string, SQLParameterData> parametrDictionary = GetProcessCancelData(button.CommandArgument);
            int result = DisbursementBusiness.ProcessCancel(parametrDictionary);
            string message = GetAction(button.CommandArgument);
            ShowResult(result, message);
        }

        private Dictionary<string, SQLParameterData> GetProcessCancelData(string nextStatus)
        {
            return new Dictionary<string, SQLParameterData>
            {
                { DisbursementTable.DisbursementID, new SQLParameterData(hidDisbursementID.Value, SqlDbType.BigInt) },
                { DisbursementTable.DisbursementStatus, new SQLParameterData(nextStatus, SqlDbType.Int) },
                { DisbursementTable.Remark, new SQLParameterData(tbRemark.Text.Trim(), SqlDbType.NVarChar) },
                { DisbursementTable.ModifyUserID, new SQLParameterData(UserInfo.UserID, SqlDbType.Int) },
                {
                    DisbursementTable.ModifyDateTime,
                    new SQLParameterData(DateTime.Now.ToString(PatternEnum.DateTime), SqlDbType.BigInt)
                },
            };
        }
    }
}