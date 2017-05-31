using DotNetNuke.UI.Skins.Controls;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Lucene.Net.Support;
using Modules.Application.Database;
using Modules.Application.DataTransfer;
using Modules.Application.Global;
using Telerik.Web.UI;
using Website.Library.Global;
//using Modules.PolicyField;
//using Modules.PolicyField.Business;
//using Modules.PolicyField.DataTransfer;

namespace DesktopModules.Modules.Application
{
    public partial class PolicyFieldSetting : ApplicationModuleBase
    {
        protected override void OnLoad(EventArgs e)
        {
            if (IsPostBack)
            {
                return;
            }
            BindData();
        }

        public void BindData()
        {
            BindPolicyData(ddlPolicy);
        }

        protected void LoadData(object sender, EventArgs e)
        {
            hidPolicy.Value = ddlPolicy.SelectedValue;
            lbForPolicyField.Text = ddlPolicy.SelectedItem.Text;
            //int selected = int.Parse(ddlPolicy.SelectedValue);
            //List<PolicyFieldData> listPolicy = new PolicyFieldBusiness().FindPolicy(int.Parse(ddlPolicy.SelectedValue));
            //if (listPolicy.Count == 0)
            //{
            //    BindListBoxSource(CacheBase.Receive<ApplicationFieldData>());
            //    ClearListBoxDestination();
            //    lbTotalPolicy.Text = String.Empty;
            //}
            //else
            //{
            //    lbTotalPolicy.Text = "Total: " + listPolicy.Count;
            //    List<string> fieldName = new List<string>();
            //    foreach (var item in listPolicy)
            //    {
            //        fieldName.Add(item.FieldName);
            //    }
            //    BindListBoxSource(
            //        CacheBase.Receive<ApplicationFieldData>().Where(x => !fieldName.Contains(x.FieldName)).ToList());
            //    BindListBoxDestination(listPolicy);
            //}
        }








        public void BindListBoxSource(List<ApplicationFieldData> appField)
        {
            RadListBoxSource.DataSource = appField;
            RadListBoxSource.DataTextField = ApplicationFieldTable.FieldName;
            RadListBoxSource.DataValueField = ApplicationFieldTable.FieldName;
            RadListBoxSource.DataBind();
        }

        public void BindListBoxDestination(List<ApplicationFieldData> appField)
        {
            RadListBoxDestination.DataSource = appField;
            RadListBoxDestination.DataTextField = ApplicationFieldTable.FieldName;
            RadListBoxDestination.DataValueField = ApplicationFieldTable.FieldName;
            RadListBoxDestination.DataBind();
        }


        public void ClearListBoxDestination()
        {
            RadListBoxDestination.Items.Clear();
        }
        

        protected void Submit(object sender, EventArgs e)
        {
            //List<PolicyFieldData> dataPolicyField = new EquatableList<PolicyFieldData>();
            //foreach (RadListBoxItem item in RadListBoxDestination.Items)
            //{
            //    dataPolicyField.Add(new PolicyFieldData
            //    {
            //        PolicyID = hidPolicy.Text,
            //        FieldName = item.Value,
            //        ModifyUserID = UserId.ToString(),
            //        IsDisable = "1",
            //        ModifyDateTime = DateTime.Now.ToString("yyyyMMddHHmmss")
            //    });
            //}
            //List<PolicyFieldData> listPolicy = new PolicyFieldBusiness().FindPolicy(int.Parse(hidPolicy.Text));
            //if (listPolicy.Count == 0)
            //{
            //    if (new PolicyFieldBusiness().AddPolicy(dataPolicyField) > 0)
            //    {
            //        ShowMessage("Add Success!", messageType: ModuleMessage.ModuleMessageType.GreenSuccess);
            //    }
            //}
            //else
            //{
            //    if (new PolicyFieldBusiness().UpdatePolicy(int.Parse(hidPolicy.Text), dataPolicyField) > 0)
            //    {
            //        CacheBase.Reload<PolicyFieldCacheData>(hidPolicy.Text);
            //        ShowMessage("Update Success!", messageType: ModuleMessage.ModuleMessageType.GreenSuccess);
            //    }
            //}
        }
    }
}