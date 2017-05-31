using System;
using Modules.Application.Business;
using Modules.Application.Global;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using Modules.Application.Database;
using Modules.Application.DataTransfer;

namespace DesktopModules.Modules.Application
{
    public partial class ApplicationHistory : ApplicationModuleBase
    {
        private const string NoSelection = "109XXQIEIXUARJIOAJFK";

        protected override void OnLoad(EventArgs e)
        {
            if (IsPostBack)
            {
                return;
            }
            AutoWire();
            BindData();
        }

        private void BindData()
        {
            ResetFields();

            ddlLeftVersion.Items.Clear();
            ddlLeftVersion.Items.Add(new ListItem("Please select a version", NoSelection));
            ddlLeftVersion.DataBind();

            ddlRightVersion.Items.Clear();
            ddlRightVersion.Items.Add(new ListItem("Please select previous version", NoSelection));
            ddlRightVersion.DataBind();

            ddlDiff.Items.Clear();
            ddlDiff.Items.Add(new ListItem("Both", NoSelection));
            ddlDiff.Items.Add(new ListItem(ApplicationLogBusiness.Yes, ApplicationLogBusiness.Yes));
            ddlDiff.Items.Add(new ListItem(ApplicationLogBusiness.No, ApplicationLogBusiness.No));
            ddlDiff.DataBind();
            
            DoSearch(Request.QueryString[ApplicationTable.ApplicationID]);

            ddlLeftVersion.SelectedValue = Request.QueryString[ApplicationLogTable.ApplicationLogID];
            OnLeftSelected();
        }

        private void ResetFields()
        {
            ddlField.Items.Clear();
            ddlField.Items.Add(new ListItem("All fields", NoSelection));
            ddlField.DataBind();
        }

        protected void Search(object sender, EventArgs e)
        {
            string applicationID = tbApplicationID.Text;
            if (string.IsNullOrEmpty(applicationID))
            {
                ShowAlertDialog("Please enter an application ID.");
                return;
            }
            DoSearch(applicationID);
        }

        private void DoSearch(string applicationID = null)
        {
            if (string.IsNullOrEmpty(applicationID))
            {
                tbApplicationID.Text = applicationID;
            }
            else
            {
                hidApplicationId.Value = applicationID;
            }

            tableView.InnerHtml = string.Empty;

            ddlField.Items.Clear();
            ddlField.Items.Add(new ListItem("All fields", NoSelection));
            
            ddlLeftVersion.Items.Clear();
            ddlLeftVersion.Items.Add(new ListItem("Please select a version", NoSelection));
            
            Dictionary<string, string> versions = ApplicationLogBusiness.GetVersion(applicationID);
            foreach (KeyValuePair<string, string> version in versions)
            {
                ddlLeftVersion.Items.Add(new ListItem(version.Value, version.Key));
            }

            ddlField.DataBind();
            ddlLeftVersion.DataBind();
        }

        protected void LeftVersionChange(object sender, EventArgs e)
        {
            OnLeftSelected();
        }

        private void OnLeftSelected()
        {
            string applicationID = hidApplicationId.Value;
            tableView.InnerHtml = string.Empty;
            ddlDiff.SelectedValue = NoSelection;
            ddlField.SelectedValue = NoSelection;

            if (string.IsNullOrEmpty(applicationID))
            {
                return;
            }

            string leftVersion = ddlLeftVersion.SelectedValue;
            ddlRightVersion.Items.Clear();
            ddlRightVersion.Items.Add(new ListItem("Please select previous version", NoSelection));

            if (!NoSelection.Equals(leftVersion))
            {
                Dictionary<string, string> versions = ApplicationLogBusiness.GetVersion(applicationID, leftVersion);

                foreach (KeyValuePair<string, string> version in versions)
                {
                    ddlRightVersion.Items.Add(new ListItem(version.Value, version.Key));
                }
            }
            else
            {
                ResetFields();
            }

            ddlRightVersion.DataBind();

            hidApplicationId.Value = applicationID;
        }

        protected void RightVersionChange(object sender, EventArgs e)
        {
            tableView.InnerHtml = string.Empty;
            ddlDiff.SelectedValue = NoSelection;

            ddlField.Items.Clear();
            ddlField.Items.Add(new ListItem("All fields", NoSelection));

            string applicationID = hidApplicationId.Value;
            string leftVersion = ddlLeftVersion.SelectedValue;
            string rightVersion = ddlRightVersion.SelectedValue;

            if (!NoSelection.Equals(rightVersion))
            {
                BindGrid(leftVersion, rightVersion);
            }
            else
            {
                ResetFields();
            }
            hidApplicationId.Value = applicationID;
            ddlField.DataBind();
        }

        private void BindGrid(string lhs, string rhs = null)
        {
            string appId = hidApplicationId.Value;

            if (string.IsNullOrEmpty(lhs)) return;
            ddlField.Items.Clear();
            ddlField.Items.Add(new ListItem("All fields", NoSelection));

            string innerHtml = string.Empty;
            Dictionary<string, ApplicationLogDetailDiffData> items = ApplicationLogBusiness.GetChangeOfTwoVersions(appId, lhs, rhs);
            
            foreach (KeyValuePair<string, ApplicationLogDetailDiffData> i in items)
            {
                ddlField.Items.Add(new ListItem(i.Key, i.Key));
                ApplicationLogDetailDiffData item = i.Value;

                string classTr = item.Diff;
                if (ApplicationLogBusiness.Yes.Equals(item.Diff))
                {
                    classTr += " ";
                    classTr += string.IsNullOrEmpty(rhs) ? "": "c-font-red-3";
                }
                
                innerHtml += $@"<tr class=""{classTr} "" id=""{i.Key}"">
                        <td>{item.FieldName}</td>
                        <td>{item.FieldValueLhs}</td>
                        <td>{item.FieldValueRhs}</td>
                        <td>{item.Diff}</td>
				        </tr>";
            }
            tableView.InnerHtml = innerHtml;
        }
    }
}
