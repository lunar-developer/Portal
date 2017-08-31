using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using DotNetNuke.UI.Skins.Controls;
using Modules.Application.Business;
using Modules.Application.DataTransfer;
using Modules.UserManagement.Global;
using Modules.VSaleKit.Business;
using Modules.VSaleKit.Database;
using Modules.VSaleKit.Enum;
using Modules.VSaleKit.Global;
using Telerik.Web.UI;
using Website.Library.DataTransfer;
using Website.Library.Global;
using PermissionEnum = Modules.VSaleKit.Enum.PermissionEnum;
using UserBusiness = Modules.UserManagement.Business.UserBusiness;

namespace DesktopModules.Modules.VSaleKit
{
    public partial class ApplicationDetail : VSaleKitModuleBase
    {
        protected bool IsEditMode => string.IsNullOrWhiteSpace(hidUniqueID.Value) == false;


        protected override void OnLoad(EventArgs e)
        {
            if (IsPostBack)
            {
                return;
            }

            AutoWire();
            BindData();
            LoadData(Request[ApplicationFormTable.UniqueID]);
        }

        private void BindData()
        {
            BindIdentityTypeData(ddlIdentityType);
            LoadPolicyData(ddlPolicy);
            LoadApplicationTypeData(ddlApplicationType);
            hidRoleName.Value = GetRoleName();
        }

        private void LoadData(string uniqueID)
        {
            ResetData();

            if (string.IsNullOrWhiteSpace(uniqueID) == false)
            {
                hidUniqueID.Value = uniqueID;
                DataSet dsResult =
                    ApplicationFormBusiness.LoadApplication(uniqueID, UserInfo.UserID, hidRoleName.Value);
                if (dsResult.Tables.Count > 0
                    && dsResult.Tables[0].Rows.Count == 1)
                {
                    DataRow row = dsResult.Tables[0].Rows[0];

                    // General Info                
                    txtCustomerName.Text = row[ApplicationFormTable.CustomerName].ToString().Trim();
                    txtMobileNumber.Text = row[ApplicationFormTable.MobileNumber].ToString().Trim();
                    ddlIdentityType.SelectedValue = row[ApplicationFormTable.IdentityTypeCode].ToString().Trim();
                    txtCustomerID.Text = row[ApplicationFormTable.CustomerID].ToString().Trim();

                    ddlApplicationType.SelectedValue = row[ApplicationFormTable.ApplicationTypeID].ToString().Trim();
                    ddlPolicy.SelectedValue = row[ApplicationFormTable.PolicyID].ToString().Trim();
                    txtProposalLimit.Text = FunctionBase.FormatDecimal(
                        row[ApplicationFormTable.ProposalLimit].ToString());
                    ddlPriority.SelectedValue = row[ApplicationFormTable.Priority].ToString().Trim();

                    string status = row[ApplicationFormTable.Status].ToString().Trim();
                    string branchCode = row[ApplicationFormTable.BranchCode].ToString().Trim();
                    lblBranch.Text = UserManagementModuleBase.FormatBranchCode(branchCode);
                    lblUniqueID.Text = uniqueID;
                    lblStatus.Text = ApplicStatusEnum.GetDescription(status);

                    // Hidden Info
                    hidBranchCode.Value = branchCode;
                    hidUniqueID.Value = uniqueID;
                    hidStatus.Value = status;
                    hidCreateUserName.Value = row["CreateUserName"].ToString().Trim().ToUpper();
                    hidProcessUserName.Value = row["ProcessUserName"].ToString().Trim().ToUpper();
                    hidNextProcessUserName.Value = row["NextProcessUserName"].ToString().Trim().ToUpper();
                    hidIsLinkApplication.Value = row["IsLinkApplication"].ToString().Trim().ToUpper();

                    // Load Documents
                    LoadDocuments(dsResult.Tables[1]);

                    // Log Info
                    gridLogData.DataSource = dsResult.Tables[2];
                    gridLogData.Visible = true;
                    gridLogData.LocalResourceFile = LocalResourceFile;
                    gridLogData.DataBind();
                }
            }

            SetPermission();
        }

        private void ResetData()
        {
            // Tab General
            txtCustomerName.Text = string.Empty;
            txtMobileNumber.Text = string.Empty;
            ddlIdentityType.SelectedIndex = 0;
            txtCustomerID.Text = string.Empty;

            ddlApplicationType.SelectedIndex = 0;
            ddlPolicy.SelectedIndex = 0;
            txtProposalLimit.Text = string.Empty;
            ddlPriority.SelectedIndex = 0;

            lblBranch.Text = string.Empty;
            lblUniqueID.Text = string.Empty;
            lblStatus.Text = string.Empty;

            // Tab Document
            TabDocumentContainer.InnerHtml = string.Empty;

            // Tab Log
            gridLogData.DataSource = null;
            gridLogData.Visible = false;

            // Process
            txtRemark.Text = string.Empty;

            // Hidden
            hidBranchCode.Value = string.Empty;
            hidUniqueID.Value = string.Empty;
            hidStatus.Value = string.Empty;
            hidCreateUserName.Value = string.Empty;
            hidProcessUserName.Value = string.Empty;
            hidNextProcessUserName.Value = string.Empty;
            hidIsLinkApplication.Value = string.Empty;

            // Buttons
            btnSave.Visible = btnUpdate.Visible = false;
            btnApprove.Visible = btnReturn.Visible = false;
            btnRecall.Visible = btnClose.Visible = false;
            btnUploadFile.Visible = btnSortFile.Visible = btnEditFile.Visible = btnDeleteFile.Visible = false;
        }

        private void SetPermission()
        {
            string roleName = GetRoleName();
            bool isEditMode = IsEditMode;
            bool isHasPermissionAdd = IsHasPermission(PermissionEnum.Add);
            bool isHasPermissionEdit = IsHasPermission(PermissionEnum.Edit);
            bool isHasPermissionApprove = IsHasPermission(PermissionEnum.Approve);
            bool isHasPermissionRecall = IsHasPermission(PermissionEnum.Recall);
            bool isHasPermissionClose = IsHasPermission(PermissionEnum.Close);
            bool isCreater = hidCreateUserName.Value == UserInfo.Username.ToUpper();
            bool isProcesser = hidProcessUserName.Value == UserInfo.Username.ToUpper();
            bool isOwner = hidNextProcessUserName.Value == UserInfo.Username.ToUpper()
                || hidNextProcessUserName.Value == roleName;
            bool isNoOwner = string.IsNullOrWhiteSpace(hidNextProcessUserName.Value);
            string status = hidStatus.Value;

            TabDocumentInfo.Visible = TabHistoryInfo.Visible = isEditMode;
            TabDocumentContainer.Visible = TabHistoryContainer.Visible = isEditMode;

            // Fields
            ddlApplicationType.Enabled = isEditMode == false;
            ddlPolicy.Enabled = isEditMode == false;

            // Buttons
            btnSave.Visible = isEditMode == false && isHasPermissionAdd;
            btnUpdate.Visible = isEditMode && isHasPermissionEdit && (
                FunctionBase.IsInArray(status,
                    ApplicStatusEnum.Init,
                    ApplicStatusEnum.Editing) && isCreater && isNoOwner
                ||
                FunctionBase.IsInArray(status,
                    ApplicStatusEnum.WaitForApprove,
                    ApplicStatusEnum.Return,
                    ApplicStatusEnum.Editing,
                    ApplicStatusEnum.Processing,
                    ApplicStatusEnum.Recall) && isOwner
            );
            btnUploadFile.Visible = btnSortFile.Visible =
                btnEditFile.Visible = btnDeleteFile.Visible = btnUpdate.Visible;

            btnApprove.Visible = isEditMode && (
                FunctionBase.IsInArray(status,
                    ApplicStatusEnum.Init,
                    ApplicStatusEnum.Editing) && isCreater && isNoOwner && isHasPermissionAdd
                ||
                FunctionBase.IsInArray(status,
                    ApplicStatusEnum.Editing,
                    ApplicStatusEnum.WaitForApprove,
                    ApplicStatusEnum.Processing,
                    ApplicStatusEnum.Return,
                    ApplicStatusEnum.Recall) && isOwner && (isHasPermissionAdd || isHasPermissionApprove)
            );

            btnReturn.Visible = isEditMode && isHasPermissionApprove && isOwner && isCreater == false;
            btnRecall.Visible = isEditMode && isHasPermissionRecall && isProcesser &&
                status == ApplicStatusEnum.WaitForApprove;
            btnClose.Visible = isEditMode && isHasPermissionClose &&
                FunctionBase.IsInArray(status,
                    ApplicStatusEnum.Init,
                    ApplicStatusEnum.Editing,
                    ApplicStatusEnum.Return,
                    ApplicStatusEnum.Recall) && (isCreater && isNoOwner || isOwner);

            PanelNavigator.Visible = btnSave.Visible || btnUpdate.Visible || btnApprove.Visible
                || btnReturn.Visible || btnRecall.Visible || btnClose.Visible;
        }


        protected void ViewPolicyDetai(object sender, EventArgs e)
        {
            PolicyData policy = CacheBase.Receive<PolicyData>(ddlPolicy.SelectedValue);
            string title = $"Chính sách: {policy.PolicyCode} - {policy.Name}";
            string message = policy.Remark.Replace(Environment.NewLine, string.Empty);
            ShowAlertDialog(message, title);
        }

        protected void OnNeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            gridLogData.DataSource = ApplicationFormBusiness.LoadLogHistory(hidUniqueID.Value);
        }


        private const string DocumentGroupHtml = @"
            <div class='dnnPanels'>
                <h2 class='dnnFormSectionHead'>
                    <span tabindex='0'></span>
                    <a href='javascript:;'>
                        {0}
                        <i class='fa fa-caret-right c-margin-l-5 c-margin-r-5'></i>Tổng số: {1}
                        {2}
                        {3}
                        {4}
                    </a>
                </h2>
                <fieldset>
                    <div class='form-group'>
                        {5}
                    </div>
                </fieldset>
            </div>
        ";

        private void LoadDocuments(DataTable documentTable)
        {
            // Get Permission
            bool isEditMode = IsEditMode;
            bool isHasPermissionEdit = IsHasPermission(PermissionEnum.Edit);
            bool isCreater = hidCreateUserName.Value == UserInfo.Username.ToUpper();
            bool isOwner = hidNextProcessUserName.Value == UserInfo.Username.ToUpper();
            bool isNoOwner = string.IsNullOrWhiteSpace(hidNextProcessUserName.Value);
            string status = hidStatus.Value;
            bool isAllowEdit = isEditMode && isHasPermissionEdit && (
                FunctionBase.IsInArray(status,
                    ApplicStatusEnum.Init,
                    ApplicStatusEnum.Editing) && isCreater && isNoOwner
                ||
                FunctionBase.IsInArray(status,
                    ApplicStatusEnum.WaitForApprove,
                    ApplicStatusEnum.Return,
                    ApplicStatusEnum.Editing,
                    ApplicStatusEnum.Processing,
                    ApplicStatusEnum.Recall) && isOwner
            );
            const string totalFilesInput = "<input type='hidden' class='TotalFiles' value='{0}' alt='{1}'/>";
            string sortButton = isAllowEdit
                ? @"<span class='iconButton c-margin-l-20' onclick='sortFile(arguments[0], ""{0}"")'>
                        <i class='fa fa-sort c-margin-r-5'></i> Sort
                    </span>"
                : string.Empty;
            string uploadButton = isAllowEdit
                ? @"<span class='iconButton c-margin-l-20' onclick='uploadFile(arguments[0], ""{0}"")'>
                        <i class='fa fa-upload c-margin-r-5'></i> Upload
                    </span>"
                : string.Empty;
            string editButton = isAllowEdit
                ? @"<a href='javascript:;' class='iconButton iconInline' onclick='editFile(""{0}"", {1}, {2});'>
                        <i class='fa fa-pencil'></i>
                    </a>"
                : string.Empty;
            string deleteButton = isAllowEdit
                ? @"<a href='javascript:;' class='iconButton iconInline iconDelete' onclick='getFile({0});'>
                        <i class='fa fa-trash-o'></i>
                    </a>"
                : string.Empty;


            // Get list Document Type base on Policy
            List<string> listDocumentTypeID = CacheBase
                .Filter<PolicyDocumentData>(ApplicationFormTable.PolicyID, ddlPolicy.SelectedValue)
                .Where(item => FunctionBase.ConvertToBool(item.IsDisable) == false)
                .OrderBy(item => int.Parse(item.OrderNo))
                .Select(item => item.DocumentTypeID)
                .ToList();

            // Get list require Documents
            List<string> listRequireDocument = CacheBase
                .Filter<PolicyDocumentData>(ApplicationFormTable.PolicyID, ddlPolicy.SelectedValue)
                .Where(item => FunctionBase.ConvertToBool(item.IsRequire))
                .Select(item => item.DocumentTypeID)
                .ToList();

            // Get list Document Type available from Cache
            List<DocumentTypeData> listDocumentType = listDocumentTypeID
                .Select(CacheBase.Receive<DocumentTypeData>)
                .Where(document => document != null && FunctionBase.ConvertToBool(document.IsDisable) == false)
                .ToList();

            // Build Html Dictionary
            Dictionary<string, List<string>> htmlDictionary =
                listDocumentType.ToDictionary(document => document.DocumentCode, document => new List<string>());

            // Add Group Exceptions
            List<string> listExceptions = new List<string>();

            // Render Documents
            foreach (DataRow row in documentTable.Rows)
            {
                string documentType = row[ApplicationFormTable.DocumentCode].ToString().Trim();
                string imageName = row[ApplicationFormTable.FileName].ToString().Trim();
                string imageData = "data:image/png;base64," + row[ApplicationFormTable.FileData].ToString().Trim();
                string imageId = row[ApplicationFormTable.FileID].ToString().Trim();
                string imageNumber = row[ApplicationFormTable.FileNumber].ToString().Trim();

                List<string> listHtml = htmlDictionary.ContainsKey(documentType)
                    ? htmlDictionary[documentType]
                    : listExceptions;

                listHtml.Add($@"
                    <div class='col-sm-6 c-margin-b-20'>
                        <div class='col-sm-5 control-label'>
                            <label>{imageName}</label>
                            {string.Format(editButton, documentType, imageId, imageNumber)}
                            {string.Format(deleteButton, imageId)}
                        </div>
                        <div class='col-sm-7' style='overflow: hidden'>
                            <img style='padding-top: 4px; height:200px' 
                                id='image{imageId}' name='{imageName}' src='{imageData}' group='{documentType}'/>
                        </div>
                    </div>
                ");
            }

            // Build HTML String
            StringBuilder builder = new StringBuilder();
            foreach (DocumentTypeData document in listDocumentType)
            {
                bool isRequire = listRequireDocument.Contains(document.DocumentTypeID);
                List<string> listDocument = htmlDictionary[document.DocumentCode];

                builder.AppendLine(string.Format(DocumentGroupHtml,
                    $"{(isRequire ? "<span class='c-font-red-2'>*</span>&nbsp;" : string.Empty)}{document.Name}",
                    listDocument.Count,
                    listDocument.Count > 1 ? string.Format(sortButton, document.DocumentCode) : string.Empty,
                    string.Format(uploadButton, document.DocumentCode),
                    isRequire ? string.Format(totalFilesInput, listDocument.Count, document.Name) : string.Empty,
                    string.Join("", listDocument)));
            }
            if (listExceptions.Count > 0)
            {
                builder.AppendLine(string.Format(DocumentGroupHtml,
                    "Các chứng từ khác",
                    listExceptions.Count,
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    string.Join("", listExceptions)));
            }

            TabDocumentContainer.InnerHtml = builder.ToString();
        }

        protected void Save(object sender, EventArgs e)
        {
            hidBranchCode.Value = UserBusiness.GetCurrentBranchCode(UserInfo.UserID);
            hidStatus.Value = ApplicStatusEnum.Init;
            Dictionary<string, SQLParameterData> dataDictionary = GetData();
            string uniqueID;
            int result = ApplicationFormBusiness.InsertApplication(dataDictionary, out uniqueID);
            string url = $"{ApplicationUrl}/{ApplicationFormTable.UniqueID}/{uniqueID}";
            switch (result)
            {
                case 1:
                    string script = $"location.href = '{url}'";
                    ShowAlertDialog("Thêm mới hồ sơ thành công.", null, false, script);
                    break;

                case 0:
                    ShowMessage($@"
                        Khách hàng này đã có hồ sơ trên hệ thống.
                        Click vào <a class='c-theme-color c-edit-link' target='_blank' href='{
                            url
                        }'>đây</a> để xem chi tiết.");
                    break;

                case -1:
                    ShowMessage("Thêm mới hồ sơ thất bại.", ModuleMessage.ModuleMessageType.RedError);
                    break;

                default:
                    ShowMessage("Bạn không có quyền thực hiện chức năng này.");
                    break;
            }
        }

        protected void Update(object sender, EventArgs e)
        {
            Dictionary<string, SQLParameterData> dataDictionary = GetData();
            string uniqueID;
            int result = ApplicationFormBusiness.UpdateApplication(dataDictionary, out uniqueID);

            switch (result)
            {
                case 1:
                    ShowMessage("Cập nhật hồ sơ thành công.", ModuleMessage.ModuleMessageType.GreenSuccess);
                    LoadData(hidUniqueID.Value);
                    break;

                case 0:
                    string url = $"{ApplicationUrl}/{ApplicationFormTable.UniqueID}/{uniqueID}";
                    ShowMessage($@"
                        Khách hàng này đã có hồ sơ trên hệ thống.
                        Click vào <a class='c-theme-color c-edit-link' target='_blank' href='{
                            url
                        }'>đây</a> để xem chi tiết.");
                    break;

                case -1:
                    ShowMessage("Cập nhật hồ sơ thất bại.", ModuleMessage.ModuleMessageType.RedError);
                    break;

                case -3:
                    ShowMessage("Trạng thái của hồ sơ đã bị thay đổi, vui lòng reload lại thông tin.");
                    break;

                default:
                    ShowMessage("Bạn không có quyền thực hiện chức năng này.");
                    break;
            }
        }

        private Dictionary<string, SQLParameterData> GetData()
        {
            Dictionary<string, SQLParameterData> dataDictionary = new Dictionary<string, SQLParameterData>
            {
                { "UserName", new SQLParameterData(UserInfo.Username, SqlDbType.VarChar) },
                { "BranchID", new SQLParameterData(hidBranchCode.Value, SqlDbType.VarChar) },
                { "UserType", new SQLParameterData(hidRoleName.Value, SqlDbType.VarChar) },
                { "ApplicID", new SQLParameterData(hidUniqueID.Value, SqlDbType.VarChar) },
                { "ProcessID", new SQLParameterData(ddlApplicationType.SelectedValue, SqlDbType.VarChar) },
                { "Priority", new SQLParameterData(ddlPriority.SelectedValue, SqlDbType.VarChar) },
                { "Status", new SQLParameterData(hidStatus.Value, SqlDbType.VarChar) },
                { "CustName", new SQLParameterData(txtCustomerName.Text.Trim(), SqlDbType.NVarChar) },
                {
                    "SearchCustName",
                    new SQLParameterData(FunctionBase.RemoveUnicode(txtCustomerName.Text.Trim()), SqlDbType.VarChar)
                },
                { "LegalID", new SQLParameterData(txtCustomerID.Text.Trim(), SqlDbType.VarChar) },
                { "LegalType", new SQLParameterData(ddlIdentityType.SelectedValue, SqlDbType.VarChar) },
                { "Birthday", new SQLParameterData(string.Empty, SqlDbType.VarChar) },
                { "Phone", new SQLParameterData(txtMobileNumber.Text.Trim(), SqlDbType.VarChar) },
                { "Address", new SQLParameterData(string.Empty, SqlDbType.NVarChar) },
                { "Description", new SQLParameterData(txtRemark.Text.Trim(), SqlDbType.NVarChar) },
                {
                    "CreditLimit",
                    new SQLParameterData(txtProposalLimit.Text.Trim().Replace(",", string.Empty), SqlDbType.Money)
                },
                { "PolicyID", new SQLParameterData(ddlPolicy.SelectedValue, SqlDbType.VarChar) }
            };
            return dataDictionary;
        }

        protected void ProcessDocument(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            if (button == null)
            {
                return;
            }

            string uniqueID = hidUniqueID.Value;
            string userName = UserInfo.Username;
            string roleName = hidRoleName.Value;
            string branchCode = hidBranchCode.Value;
            string remark = txtRemark.Text.Trim();
            string action = button.CommandName;
            string status = hidStatus.Value;
            bool isLinkApplication = hidIsLinkApplication.Value == "Y";

            Dictionary<string, SQLParameterData> dataDictionary = new Dictionary<string, SQLParameterData>
            {
                { "ApplicID", new SQLParameterData(uniqueID, SqlDbType.VarChar) },
                { "UserName", new SQLParameterData(userName, SqlDbType.VarChar) },
                { "UserType", new SQLParameterData(roleName, SqlDbType.VarChar) },
                { "BranchID", new SQLParameterData(branchCode, SqlDbType.VarChar) },
                { "Description", new SQLParameterData(remark, SqlDbType.NVarChar) },
                { "Action", new SQLParameterData(action, SqlDbType.VarChar) },
                { "Status", new SQLParameterData(status, SqlDbType.VarChar) }
            };
            int result = ApplicationFormBusiness.ProcessApplication(dataDictionary);
            switch (result)
            {
                case 1:
                    ShowMessage($"{button.Text} thành công.", ModuleMessage.ModuleMessageType.GreenSuccess);
                    LoadData(hidUniqueID.Value);

                    if (action == "Approve" && IsRoleLeader(roleName))
                    {
                        if (isLinkApplication)
                        {
                            ApplicationBusiness.SupplyDocument(uniqueID);
                        }
                        else
                        {
                            ApplicationBusiness.LinkApplication(uniqueID);
                        }
                    }
                    break;

                case -1:
                    ShowMessage($"{button.Text} thất bại.", ModuleMessage.ModuleMessageType.RedError);
                    break;

                case -3:
                    ShowMessage("Trạng thái của hồ sơ đã bị thay đổi, vui lòng reload lại thông tin.");
                    break;

                default:
                    ShowMessage("Bạn không có quyền thực hiện chức năng này.");
                    break;
            }
        }

        protected void UploadFile(object sender, EventArgs e)
        {
            string guid = FunctionBase.GetRandomGuid();
            Session[guid] = guid;

            string script = EditUrl("Attach", 800, 300, true, true, "reloadFile",
                ApplicationFormTable.UniqueID, hidUniqueID.Value,
                ApplicationFormTable.PolicyID, ddlPolicy.SelectedValue,
                ApplicationFormTable.DocumentCode, hidDocumentCode.Value,
                "Action", "INSERT",
                "Token", guid);
            RegisterScript(script);
        }

        protected void EditFile(object sender, EventArgs e)
        {
            string guid = FunctionBase.GetRandomGuid();
            Session[guid] = guid;
            Session[ApplicationFormTable.FileName] = hidFileName.Value;
            Session[ApplicationFormTable.FileData] = hidFileData.Value;
            Session[ApplicationFormTable.FileNumber] = hidFileNumber.Value;

            string script = EditUrl("Attach", 800, 300, true, true, "reloadFile",
                ApplicationFormTable.UniqueID, hidUniqueID.Value,
                ApplicationFormTable.PolicyID, ddlPolicy.SelectedValue,
                ApplicationFormTable.DocumentCode, hidDocumentCode.Value,
                "Action", "EDIT",
                "Token", guid);
            RegisterScript(script);
        }

        protected void SortFile(object sender, EventArgs e)
        {
            string guid = FunctionBase.GetRandomGuid();
            Session[guid] = guid;

            string script = EditUrl("Sort", 800, 300, true, true, "reloadFile",
                ApplicationFormTable.UniqueID, hidUniqueID.Value,
                ApplicationFormTable.PolicyID, ddlPolicy.SelectedValue,
                ApplicationFormTable.DocumentCode, hidDocumentCode.Value,
                "Token", guid);
            RegisterScript(script);
        }

        protected void DeleteFile(object sender, EventArgs e)
        {
            bool result = ApplicationFormBusiness.DeleteFile(UserInfo.Username, hidFileID.Value);
            if (result)
            {
                ShowMessage("Xoá file thành công.", ModuleMessage.ModuleMessageType.GreenSuccess);
                ReloadFile(null, null);
            }
            else
            {
                ShowMessage("Xoá file thất bại", ModuleMessage.ModuleMessageType.RedError);
            }
        }

        protected void ReloadFile(object sender, EventArgs e)
        {
            DataTable dtFiles = ApplicationFormBusiness.LoadAttachFiles(hidUniqueID.Value);
            LoadDocuments(dtFiles);
        }
    }
}