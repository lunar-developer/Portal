using System;
using System.Data;
using System.Text;
using System.Web.UI.WebControls;
using Modules.VSaleKit.Business;
using Modules.VSaleKit.Database;
using Modules.VSaleKit.Global;
using Website.Library.Global;

namespace DesktopModules.Modules.VSaleKit
{
    public partial class ApplicationDetail : VSaleKitModuleBase
    {
        protected override void OnLoad(EventArgs e)
        {
            if (IsPostBack)
            {
                return;
            }

            AutoWire();
            BindData();
            LoadData(Request.QueryString[ApplicationFormTable.UniqueID]);
        }

        protected void btnProcess_Click(object sender, EventArgs e)
        {
            ApplicationFormBusiness.ProcessApplication(hidApplicationID.Value, UserInfo.Username, "TD", "0001", "");

            //string actionCode = ddlAction.SelectedValue;
            //string remark = txtRemark.Text.Trim();

            //Dictionary<string, string> dictionary = new Dictionary<string, string>
            //{
            //    { "UserID", UserInfo.UserID.ToString() },
            //    { ApplicationTable.ActionCode, actionCode },
            //    { ApplicationTable.Remark, remark },
            //    { ApplicationTable.ApplicationID, hidApplicationID.Value },
            //    { ApplicationTable.ProcessID, hidProcessID.Value },
            //    { ApplicationTable.PhaseID, hidPhaseID.Value }
            //};

            //if (ApplicationController.ProcessApplication(dictionary) == 1)
            //{
            //    LoadData(hidApplicationID.Value);
            //    ShowMessage("Xử lý thành công", ModuleMessage.ModuleMessageType.GreenSuccess);
            //}
            //else
            //{
            //    ShowMessage("Xử lý thất bại", ModuleMessage.ModuleMessageType.YellowWarning);
            //}
        }

        private void ResetData()
        {
            // Tab General
            txtCustomerName.Text = txtMobileNumber.Text = string.Empty;
            ddlIdentityType.SelectedIndex = 0;
            txtCustomerID.Text = string.Empty;
            ddlPolicy.SelectedIndex = ddlApplicationType.SelectedIndex = 0;
            lblStatus.Text = string.Empty;
            chkIsHighPriority.Checked = false;
            txtProposalLimit.Text = string.Empty;

            // Tab Document
            tabDocumentInfo.InnerHtml = string.Empty;

            // Tab Log
            gridLogData.DataSource = null;

            // Process
            txtRemark.Text = string.Empty;
            PanelNavigator.Visible = false;

            // Hidden
            hidApplicationID.Value = string.Empty;
            hidProcessID.Value = string.Empty;
            hidPhaseID.Value = string.Empty;
        }

        private void LoadData(string applicationID)
        {
            ResetData();
            if (string.IsNullOrWhiteSpace(applicationID))
            {
                return;
            }

            DataSet dsResult = ApplicationFormBusiness.LoadApplication(applicationID, UserInfo.UserID);
            if (dsResult.Tables.Count == 0 || dsResult.Tables[0].Rows.Count == 0)
            {
                ShowMessage("Không tìm thấy thông tin hồ sơ.");
                return;
            }

            // General Info
            DataTable dataTable = dsResult.Tables[0];
            DataRow row = dataTable.Rows[0];
            txtCustomerName.Text = row[ApplicationFormTable.CustomerName].ToString();
            txtMobileNumber.Text = row[ApplicationFormTable.MobileNumber].ToString();
            ddlIdentityType.SelectedValue = row[ApplicationFormTable.IdentityTypeCode].ToString();
            txtCustomerID.Text = row[ApplicationFormTable.CustomerID].ToString();
            ddlPolicy.SelectedValue = row[ApplicationFormTable.PolicyCode].ToString();
            ddlApplicationType.SelectedValue = row[ApplicationFormTable.ApplicationTypeID].ToString();
                
            lblStatus.Text = GetResource(row[ApplicationFormTable.PhaseName].ToString());
            chkIsHighPriority.Checked = row[ApplicationFormTable.IsHighPriority].ToString() == "1";
            txtProposalLimit.Text = FunctionBase.FormatDecimal(row[ApplicationFormTable.ProposalLimit].ToString());


            // Hidden Info
            hidApplicationID.Value = row[ApplicationFormTable.UniqueID].ToString();
            hidProcessID.Value = row[ApplicationFormTable.ProcessID].ToString();
            hidPhaseID.Value = row[ApplicationFormTable.PhaseID].ToString();


            // Process Info
            dataTable = dsResult.Tables[1];
            PanelNavigator.Visible = dataTable.Rows.Count > 0;
            ddlAction.Items.Clear();
            foreach (DataRow item in dataTable.Rows)
            {
                string value = item[ApplicationFormTable.ActionCode].ToString();
                string text = GetResource(value);
                ddlAction.Items.Add(new ListItem(text, value));
            }
            ddlAction.DataBind();


            // Document Info
            dataTable = dsResult.Tables[2];
            StringBuilder html = new StringBuilder();
            if (dataTable.Rows.Count == 0)
            {
                html.Append(@"
                    <div class='form-group'>
                        <div class='col-sm-12 control-value'><label>Không có thông tin</label></div>
                    </div>");
            }
            else
            {
                bool isClose = false;
                int count = 0;
                html.Append("<div class='form-group'>");
                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    DataRow item = dataTable.Rows[i];
                    //string documentType = GetResource(item[ApplicationFormTable.DocumentCode].ToString().Trim());
                    string documentType = item[ApplicationFormTable.DocumentCode].ToString().Trim();
                    string image = "data:image/png;base64," + item[ApplicationFormTable.FileData].ToString().Trim();

                    html.Append($@"
                            <div class='col-sm-2 control-label'><label>{documentType}</label></div>
                            <div class='col-sm-4'>
                                <a href='{image}' data-lightbox='image' data-title='{documentType}'>
                                    <img class='lightbox-image' src='{image}'/>
                                </a>
                            </div>");


                    // Is wrapping row?
                    count++;
                    if (count % 2 != 0)
                    {
                        continue;
                    }
                    html.Append("</div>");
                    if (i + 1 < dataTable.Rows.Count)
                    {
                        html.Append("<div class='form-group'>");
                    }
                    else
                    {
                        isClose = true;
                    }
                }
                if (isClose == false)
                {
                    html.Append("</div>");
                }
            }
            tabDocumentInfo.InnerHtml = html.ToString();

            // Log Info
            gridLogData.DataSource = dsResult.Tables[3];
            gridLogData.DataBind();
        }

        private void BindData()
        {
            BindIdentityTypeData(ddlIdentityType);
            BindPolicyData(ddlPolicy);
            BindApplicationTypeData(ddlApplicationType);
        }
    }
}