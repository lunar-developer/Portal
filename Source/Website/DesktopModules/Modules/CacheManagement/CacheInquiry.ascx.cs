using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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

            ResetFilter();
            BindDataField();
            BindGrid();
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
            if (ddlCacheType.SelectedValue == string.Empty)
            {
                ShowAlertDialog(GetResource("TypeHint"));
                return;
            }

            string guid = ddlCacheType.SelectedValue;
            CacheBase.Reload(guid);
            BindGrid();
            lblTotal.Text = CacheBase.GetCacheCount(ddlCacheType.SelectedValue).ToString();
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
            ResetFilter();
            BindGrid();
        }


        private void BindData()
        {
            BindCacheInfo();
        }

        private void BindCacheInfo()
        {
            ddlCacheType.Items.Clear();
            ddlCacheType.ClearSelection();
            foreach (KeyValuePair<string, string> pair in CacheBase.GetCacheInfo())
            {
                ddlCacheType.Items.Add(new RadComboBoxItem(pair.Value, pair.Key));
            }
            ddlCacheType.DataBind();
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
            foreach (MemberInfo member in members)
            {
                ddlField.Items.Add(new RadComboBoxItem(member.Name, member.Name));
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

        private void ResetFilter()
        {
            ddlField.ClearSelection();
            tbKeyword.Text = string.Empty;
            hidFieldName.Value = hidFieldValue.Value = string.Empty;
        }
    }
}