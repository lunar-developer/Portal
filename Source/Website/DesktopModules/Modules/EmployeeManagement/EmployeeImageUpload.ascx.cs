using System;
using System.Data;
using DotNetNuke.UI.Skins.Controls;
using System.Collections.Generic;
using System.IO;
using Modules.EmployeeManagement.Business;
using Modules.EmployeeManagement.Database;
using Modules.EmployeeManagement.Enum;
using Modules.EmployeeManagement.Global;
using Telerik.Web.UI;
using Website.Library.DataTransfer;
using Website.Library.Global;

namespace DesktopModules.Modules.EmployeeManagement
{
    public partial class EmployeeImageUpload : EmployeeManagementModuleBase
    {
        protected override void OnLoad(EventArgs e)
        {
            if (IsPostBack == false)
            {
                LoadData();
            }
        }

        private void LoadData()
        {
            BindData();
            ShowEmployeeInfo();
        }

        private void ShowEmployeeInfo()
        {
            string email = cbxEmail.SelectedValue;
            DataTable dtTable = EmployeeBusiness.LoadEmployeeInfo(email);

            if (dtTable.Rows.Count == 0)
            {
                lblFullName.Text = string.Empty;
                lblBranch.Text = string.Empty;
                lblEmail.Text = string.Empty;
                lblMobile.Text = string.Empty;
                lblPhoneExtension.Text = string.Empty;
                return;
            }

            DataRow row = dtTable.Rows[0];
            lblFullName.Text = row[EmployeeTable.FullName].ToString();
            lblBranch.Text = row[EmployeeTable.Branch].ToString();
            lblEmail.Text = row[EmployeeTable.Email].ToString();
            lblMobile.Text = row[EmployeeTable.PhoneNumber].ToString();
            lblPhoneExtension.Text = row[EmployeeTable.PhoneExtendNumber].ToString();
            imgAvatar.Src = "data:image/png;base64," + row[EmployeeTable.Image];
        }

        protected void BindData()
        {
            DataTable dtTable = EmployeeBusiness.GetAllEmployeeEmail();
            foreach (DataRow email in dtTable.Rows)
            {
                cbxEmail.Items.Add(new RadComboBoxItem(email[EmployeeTable.Email].ToString(),
                    email[EmployeeTable.Email].ToString()));
            }
            cbxEmail.SelectedIndex = -1;
            cbxEmail.DataBind();
        }

        protected void OnEmailChanged(object sender, EventArgs eventArgs)
        {
            ShowEmployeeInfo();
        }

        protected void UpdateImage(object sender, EventArgs eventArgs)
        {
            if (fupFile.HasFile == false)
            {
                ShowMessage(MessageDefinitionEnum.FileNotFound, ModuleMessage.ModuleMessageType.RedError);
                return;
            }
            const int maxFileSize = 5 * 1024 * 1024; //in MB
            if (fupFile.PostedFile.ContentLength > maxFileSize)
            {
                ShowMessage(MessageDefinitionEnum.FileExceedSize, ModuleMessage.ModuleMessageType.RedError);
                return;
            }

            string extension = Path.GetExtension(fupFile.FileName);
            if (extension != null)
            {
                string fileExtension = extension.ToLower();

                if (string.IsNullOrEmpty(fileExtension) ||
                    FunctionBase.IsInArray(fileExtension, ".jpg", ".jpeg", ".png") == false)
                {
                    ShowMessage(MessageDefinitionEnum.FileNotImage, ModuleMessage.ModuleMessageType.RedError);
                    return;
                }
            }

            string email = cbxEmail.SelectedValue;
            if (string.IsNullOrWhiteSpace(email))
            {
                return;
            }

            string base64 = GetImageBase64String(fupFile.PostedFile.InputStream);
            if (string.IsNullOrEmpty(base64))
            {
                return;
            }

            // Update to database
            Dictionary<string, SQLParameterData> dictionaryParameterData = new Dictionary<string, SQLParameterData>
            {
                { EmployeeTable.Email, new SQLParameterData(email) },
                { EmployeeTable.Image, new SQLParameterData(base64, SqlDbType.NVarChar) }
            };
            bool result = EmployeeBusiness.UpdateEmployeeImage(dictionaryParameterData);
            if (result)
            {
                ShowMessage(MessageDefinitionEnum.UpdateEmployeeImageSuccess,
                    ModuleMessage.ModuleMessageType.GreenSuccess);
                ShowEmployeeInfo();
            }
            else
            {
                ShowMessage(MessageDefinitionEnum.UpdateEmployeeImageFail,
                    ModuleMessage.ModuleMessageType.RedError);
            }
        }

        private static string GetStatusMessage(int offset, int total)
        {
            return total <= 0 ? "No matches" : $"Items <b>1</b>-<b>{offset}</b> out of <b>{total}</b>";
        }

        protected void GetEmployeeEmail(object sender, RadComboBoxItemsRequestedEventArgs e)
        {
            DataTable data = EmployeeBusiness.GetEmployeeEmail(e.Text);
            if (data.Rows.Count == 0)
            {
                cbxEmail.Items.Clear();
                return;
            }
            int itemOffset = e.NumberOfItems;
            int endOffset = Math.Min(itemOffset + cbxEmail.ItemsPerRequest, data.Rows.Count);
            e.EndOfItems = endOffset == data.Rows.Count;

            for (int i = itemOffset; i < endOffset; i++)
            {
                cbxEmail.Items.Add(new RadComboBoxItem(data.Rows[i][EmployeeTable.Email].ToString(),
                    data.Rows[i][EmployeeTable.Email].ToString()));
            }
            e.Message = GetStatusMessage(endOffset, data.Rows.Count);
        }
    }
}