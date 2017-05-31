﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using DotNetNuke.UI.Skins.Controls;
using Modules.MasterData.Business;
using Modules.MasterData.Database;
using Modules.MasterData.Global;
using Telerik.Web.UI;
using Website.Library.Enum;

namespace DesktopModules.Modules.MasterData
{
    public partial class DataInquiry : MasterDataModuleBase
    {
        protected override void OnLoad(EventArgs e)
        {
            if (IsPostBack)
            {
                return;
            }

            btnAdd.Visible = btnExport.Visible = btnRefresh.Visible = false;
            BindData();
        }

        private void BindData()
        {
            DataTable dataTable = MasterDataBusiness.LoadTableSetting(UserInfo.UserID);
            foreach (DataRow row in dataTable.Rows)
            {
                string text = row[MasterDataTable.Title].ToString();
                string value = row[MasterDataTable.TableID].ToString();

                ListItem item = new ListItem(text, value);
                item.Attributes.Add(MasterDataTable.ConnectionName, row[MasterDataTable.ConnectionName].ToString());
                item.Attributes.Add(MasterDataTable.DatabaseName, row[MasterDataTable.DatabaseName].ToString());
                item.Attributes.Add(MasterDataTable.SchemaName, row[MasterDataTable.SchemaName].ToString());
                item.Attributes.Add(MasterDataTable.TableName, row[MasterDataTable.TableName].ToString());
                item.Attributes.Add(MasterDataTable.PrimaryKey, row[MasterDataTable.PrimaryKey].ToString());
                item.Attributes.Add(MasterDataTable.AssemblyName, row[MasterDataTable.AssemblyName].ToString());
                item.Attributes.Add(MasterDataTable.CacheName, row[MasterDataTable.CacheName].ToString());
                item.Attributes.Add(MasterDataTable.CacheID, row[MasterDataTable.CacheID].ToString());

                ddlDataTable.Items.Add(item);
            }
            ddlDataTable.DataBind();
            ddlDataTable.SelectedIndex = -1;
        }


        protected void LoadData(object sender, EventArgs e)
        {
            hidTableID.Value = ddlDataTable.SelectedValue;
            AttributeCollection collection = ddlDataTable.SelectedItem.Attributes;
            hidConnectionName.Value = collection[MasterDataTable.ConnectionName];
            hidDatabaseName.Value = collection[MasterDataTable.DatabaseName];
            hidSchemaName.Value = collection[MasterDataTable.SchemaName];
            hidTableName.Value = collection[MasterDataTable.TableName];
            hidPrimaryKey.Value = collection[MasterDataTable.PrimaryKey];
            hidAssemblyName.Value = collection[MasterDataTable.AssemblyName];
            hidCacheName.Value = collection[MasterDataTable.CacheName];
            hidCacheID.Value = collection[MasterDataTable.CacheID];

            DataSet dataSet = MasterDataBusiness.LoadTableData(UserInfo.UserID, hidTableID.Value,
                hidConnectionName.Value, hidDatabaseName.Value, hidSchemaName.Value, hidTableName.Value);
            if (dataSet.Tables.Count == 0)
            {
                ShowMessage("Bạn không có quyền xem thông tin này.");
                return;
            }

            DataRow row = dataSet.Tables[0].Rows[0];
            btnRefresh.Visible = true;
            btnAdd.Visible = row[MasterDataTable.IsAllowInsert].ToString() == "1";
            btnExport.Visible = row[MasterDataTable.IsAllowExport].ToString() == "1";
            gridData.Columns[0].Visible = row[MasterDataTable.IsAllowUpdate].ToString() == "1";
            gridData.Columns[1].Visible = row[MasterDataTable.IsAllowDelete].ToString() == "1";
            gridData.MasterTableView.DataKeyNames = hidPrimaryKey.Value.Split(';');
            gridData.Visible = true;

            // Bind Data
            BindGrid(dataSet.Tables[1]);
        }

        protected void Export(object sender, EventArgs e)
        {
            DataTable dataTable = MasterDataBusiness.LoadTableData(hidConnectionName.Value,
                hidDatabaseName.Value, hidSchemaName.Value, hidTableName.Value);
            ExportToExcel(dataTable);
        }

        protected void Refresh(object sender, EventArgs e)
        {
            gridData.MasterTableView.ClearEditItems();
            BindGrid(null, int.Parse(hidCurrentPage.Value));
        }

        protected void OnPageSizeChanging(object sender, GridPageSizeChangedEventArgs e)
        {
            BindGrid(null);
        }

        protected void OnPageIndexChanging(object sender, GridPageChangedEventArgs e)
        {
            BindGrid(null, e.NewPageIndex);
        }

        protected void OnGridDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridDataItem == false)
            {
                return;
            }

            GridDataItem item = (GridDataItem)e.Item;
            foreach (GridColumn column in gridData.MasterTableView.AutoGeneratedColumns)
            {
                if (item[column.UniqueName].Visible == false)
                {
                    continue;
                }

                if (IsRuntimeField(column.UniqueName))
                {
                    item[column.UniqueName].Text = GetTemplateValue(column.UniqueName, item[column.UniqueName].Text);
                }
            }
        }

        private void BindGrid(DataTable dataSource, int pageIndex = 0)
        {
            if (dataSource == null)
            {
                dataSource = MasterDataBusiness.LoadTableData(hidConnectionName.Value,
                    hidDatabaseName.Value, hidSchemaName.Value, hidTableName.Value);
            }
            hidCurrentPage.Value = pageIndex.ToString();
            gridData.DataSource = dataSource;
            gridData.CurrentPageIndex = pageIndex;
            gridData.DataBind();
        }


        protected void Create(object sender, EventArgs e)
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            EditData(dictionary);
        }

        protected void GridOnItemCommand(object source, GridCommandEventArgs e)
        {
            GridDataItem item = e.Item as GridDataItem;
            if (item == null)
            {
                return;
            }

            Dictionary<string, string> dictionary = hidPrimaryKey.Value.Split(';')
                .ToDictionary(key => key, key => item.GetDataKeyValue(key).ToString());
            switch (e.CommandName)
            {
                case ActionEnum.Edit:
                    EditData(dictionary);
                    break;

                default:
                    DeleteData(dictionary);
                    break;
            }
        }

        private void EditData(Dictionary<string, string> dictionary)
        {
            dictionary.Add(MasterDataTable.UniqueID, hidTableID.Value);
            dictionary.Add(MasterDataTable.FieldList, hidPrimaryKey.Value);
            string url = EditUrl(ActionEnum.Edit);
            string script = EditUrl(url, 800, 400, true, dictionary, "refresh");
            RegisterScript(script);
        }

        private void DeleteData(Dictionary<string, string> dictionary)
        {
            bool result = MasterDataBusiness.DeleteData(hidConnectionName.Value,
                hidDatabaseName.Value, hidSchemaName.Value, hidTableName.Value, dictionary);
            if (result)
            {
                // Remove Cache
                string key = dictionary.ContainsKey(hidCacheID.Value)
                    ? dictionary[hidCacheID.Value]
                    : string.Empty;
                RemoveCache(hidAssemblyName.Value, hidCacheName.Value, key);
                ShowMessage("Xóa thông tin thành công.", ModuleMessage.ModuleMessageType.GreenSuccess);
            }
            else
            {
                ShowMessage("Xóa thông tin thất bại.", ModuleMessage.ModuleMessageType.RedError);
            }
        }
    }
}