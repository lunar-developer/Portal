using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using Website.Library.Global;

namespace DesktopModules.Modules.CacheManagement
{
    public partial class CacheInquiry : DesktopModuleBase
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
            BindCacheInfo();
        }

        private void BindCacheInfo()
        {
            ddlCacheType.Items.Add(new ListItem(GetResource("TypeHint"), string.Empty));
            foreach (KeyValuePair<string, string> pair in CacheBase.GetCacheInfo())
            {
                ddlCacheType.Items.Add(new ListItem(pair.Value, pair.Key));
            }
            ddlCacheType.DataBind();
        }

        protected void ProcessOnSelectionChange(object sender, EventArgs e)
        {
            if (ddlCacheType.SelectedValue == string.Empty)
            {
                gridView.Visible = false;
                DivControl.Visible = false;
                lblTotal.Text = @"0";
                return;
            }

            gridView.Visible = true;
            DivControl.Visible = true;
            lblTotal.Text = CacheBase.GetCacheCount(ddlCacheType.SelectedValue).ToString();
            BindDataField();
            BindGrid();
        }

        private void BindDataField()
        {
            string guid = ddlCacheType.SelectedValue;
            Type type = CacheBase.GetCacheType(guid);

            const BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.Instance;
            MemberInfo[] members = type.GetFields(bindingFlags)
                .Cast<MemberInfo>()
                .Concat(type.GetProperties(bindingFlags))
                .ToArray();

            ddlField.Items.Clear();
            ddlField.Items.Add(new ListItem(GetResource("FieldHint"), string.Empty));
            foreach (MemberInfo member in members)
            {
                ddlField.Items.Add(new ListItem(member.Name, member.Name));
            }
        }

        private void BindGrid(int pageIndex = 0)
        {
            string guid = ddlCacheType.SelectedValue;
            gridView.CurrentPageIndex = pageIndex;
            gridView.DataSource = string.IsNullOrWhiteSpace(hidFieldName.Value)
                ? CacheBase.Receive(guid)
                : CacheBase.Filter(guid, hidFieldName.Value, hidFieldValue.Value);
            gridView.DataBind();
        }

        protected void OnPageIndexChanging(object sender, GridPageChangedEventArgs e)
        {
            BindGrid(e.NewPageIndex);
        }

        protected void OnPageSizeChanging(object sender, GridPageSizeChangedEventArgs e)
        {
            BindGrid();
        }

        protected void Reload(object sender, EventArgs e)
        {
            string guid = ddlCacheType.SelectedValue;
            CacheBase.Reload(guid);
            BindGrid();
        }

        protected void Filter(object sender, EventArgs e)
        {
            string fieldName = ddlField.SelectedValue;
            string fieldValue = tbKeyword.Text.Trim();
            if (string.IsNullOrWhiteSpace(fieldName))
            {
                ShowAlertDialog(GetResource("FieldHint"));
                return;
            }
            if (string.IsNullOrWhiteSpace(fieldValue))
            {
                ShowAlertDialog(GetResource("MissingKeyword"));
                return;
            }

            hidFieldName.Value = fieldName;
            hidFieldValue.Value = fieldValue;
            BindGrid();
        }

        protected void ClearFilter(object sender, EventArgs e)
        {
            ddlField.SelectedIndex = 0;
            tbKeyword.Text = string.Empty;
            hidFieldName.Value = hidFieldValue.Value = string.Empty;
            BindGrid();
        }
    }
}