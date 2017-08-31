using System;
using System.Collections.Generic;
using System.Data;
using DotNetNuke.UI.Skins.Controls;
using Modules.Application.Business;
using Modules.VSaleKit.Business;
using Modules.VSaleKit.Database;
using Modules.VSaleKit.Global;
using Telerik.Web.UI;
using Website.Library.Global;

namespace DesktopModules.Modules.VSaleKit
{
    public partial class DocumentSort : VSaleKitModuleBase
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

            DivForm.Visible = true;
            BindDocument();
        }

        private void BindDocument()
        {
            ListSource.Items.Clear();
            ListSource.ClearSelection();
            DataTable dtResult = ApplicationFormBusiness.LoadAttachFiles(hidUniqueID.Value, hidDocumentCode.Value);
            foreach (DataRow row in dtResult.Rows)
            {
                string text = row[ApplicationFormTable.FileName].ToString().Trim();
                string value = row[ApplicationFormTable.FileID].ToString();
                RadListBoxItem item = new RadListBoxItem(text, value)
                {
                    ImageUrl = "data:image/png;base64," + row[ApplicationFormTable.FileData].ToString().Trim()
                };
                ListSource.Items.Add(item);
            }
            ListSource.DataBind();
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

        protected void SortDocument(object sender, EventArgs e)
        {
            Dictionary<int, int> dataDictionary = new Dictionary<int, int>();
            foreach (RadListBoxItem item in ListSource.Items)
            {
                dataDictionary.Add(int.Parse(item.Value), item.Index + 1);
            }
            bool result = ApplicationFormBusiness.SortFiles(hidUniqueID.Value, hidDocumentCode.Value, dataDictionary);
            if (result)
            {
                string script = GetCloseScript();
                ShowAlertDialog("Sắp xếp Files theo thứ thự thành công.", null, false, script);
            }
            else
            {
                ShowMessage("Có lỗi khi xử lý.", ModuleMessage.ModuleMessageType.RedError);
            }
        }
    }
}