using System;
using System.Collections.Generic;
using System.Data;
using DotNetNuke.UI.Skins.Controls;
using Modules.Application.Business;
using Modules.VSaleKit.Business;
using Modules.VSaleKit.Database;
using Modules.VSaleKit.DataTransfer;
using Modules.VSaleKit.Global;
using Telerik.Web.UI;
using Telerik.Web.UI.AsyncUpload;
using Website.Library.Business;
using Website.Library.DataTransfer;
using Website.Library.Enum;
using Website.Library.Global;

namespace DesktopModules.Modules.VSaleKit
{
    public partial class DocumentUpload : VSaleKitModuleBase
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
            // Load Require Fields
            string uniqueID = Request[ApplicationFormTable.UniqueID];
            string policyID = Request[ApplicationFormTable.PolicyID];
            string documentCode = Request[ApplicationFormTable.DocumentCode];
            string token = Request["Token"];
            if (FunctionBase.IsNullOrWhiteSpace(uniqueID, policyID, documentCode, token, GetSessionData(token)))
            {
                ShowMessage("Bạn không có quyền thực hiện chức năng này.");
                return;
            }

            hidUniqueID.Value = uniqueID;
            hidPolicyID.Value = policyID;
            hidDocumentCode.Value = documentCode;
            lblUniqueID.Text = uniqueID;
            lblPolicy.Text = PolicyBusiness.GetDisplayName(int.Parse(policyID));
            lblDocumentType.Text = DocumentTypeBusiness.GetDisplayName(documentCode);
            

            // Set Control State (Insert or Edit)
            DivForm.Visible = true;
            fuImage.MaxFileSize = 5 * 1024 * UnitEnum.KiloByte;

            string action = (Request["Action"] + string.Empty).ToUpper();
            hidAction.Value = action;
            switch (action)
            {
                case "EDIT":
                    DivFile.Visible = true;
                    lblFileName.Text = GetSessionData(ApplicationFormTable.FileName);
                    imgFileContent.Src = GetSessionData(ApplicationFormTable.FileData);
                    hidCurrentFileNumber.Value = GetSessionData(ApplicationFormTable.FileNumber);
                    fuImage.MultipleFileSelection = MultipleFileSelection.Disabled;
                    fuImage.MaxFileInputsCount = 1;
                    break;

                default:
                    Dictionary<string, SQLParameterData> dataDictionary = new Dictionary<string, SQLParameterData>
                    {
                        { ApplicationFormTable.UniqueID, new SQLParameterData(uniqueID, SqlDbType.VarChar) },
                        { ApplicationFormTable.PolicyID, new SQLParameterData(policyID, SqlDbType.Int) },
                        { ApplicationFormTable.DocumentCode, new SQLParameterData(documentCode, SqlDbType.VarChar) }
                    };
                    int pageNumber = ApplicationFormBusiness.GetCurrentFileNumber(dataDictionary);
                    hidCurrentFileNumber.Value = pageNumber.ToString();
                    fuImage.MultipleFileSelection = MultipleFileSelection.Automatic;
                    break;
            }
        }

        private string GetSessionData(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                return string.Empty;
            }

            string content = Session[key] + string.Empty;
            if (string.IsNullOrWhiteSpace(content) == false)
            {
                Session.Remove(key);
            }
            return content;
        }

        protected void UploadDocument(object sender, EventArgs e)
        {
            // Collect upload files
            bool isEdit = hidAction.Value == "EDIT";
            int fileNumber;
            int.TryParse(hidCurrentFileNumber.Value, out fileNumber);
            List<FileData> listFiles = new List<FileData>();

            foreach (UploadedFile file in fuImage.UploadedFiles)
            {
                string number = isEdit ? fileNumber.ToString() : (++fileNumber).ToString();
                string fileName = $"{hidUniqueID.Value}-{hidDocumentCode.Value}-{number}";
                string fileContent = ImageBusiness.GetBase64StringFromStream(file.InputStream, 1024, 768);
                string fileSize = Convert.FromBase64String(fileContent).Length.ToString();

                listFiles.Add(new FileData
                {
                    DocumentCode = hidDocumentCode.Value,
                    FileName = fileName,
                    FileExtension = file.GetExtension(),
                    FileSize = fileSize,
                    FileNumber = number,
                    FilePath = string.Empty,
                    FileContent = fileContent
                });
            }
            if (listFiles.Count == 0)
            {
                return;
            }

            // Process
            Dictionary<string, SQLParameterData> dataDictionary = new Dictionary<string, SQLParameterData>
            {
                { "UserName", new SQLParameterData(UserInfo.Username, SqlDbType.VarChar) },
                { "RoleName", new SQLParameterData(GetRoleName(), SqlDbType.VarChar) },
                { "UniqueID", new SQLParameterData(hidUniqueID.Value, SqlDbType.VarChar) },
                { "PolicyID", new SQLParameterData(hidPolicyID.Value, SqlDbType.Int) },
                { "DocumentCode", new SQLParameterData(hidDocumentCode.Value, SqlDbType.VarChar) }
            };
            int result = ApplicationFormBusiness.AttachFiles(dataDictionary, listFiles);
            if (result > 0)
            {
                string script = GetCloseScript();
                ShowAlertDialog("Đính kèm files thành công.", null, false, script);
            }
            else
            {
                ShowMessage("Có lỗi khi xử lý.", ModuleMessage.ModuleMessageType.RedError);
            }
        }
    }
}