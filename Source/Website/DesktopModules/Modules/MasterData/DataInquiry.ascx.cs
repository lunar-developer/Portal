using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web.UI;
using DotNetNuke.UI.Skins.Controls;
using Modules.Controls;
using Modules.MasterData.Business;
using Modules.MasterData.Database;
using Modules.MasterData.Global;
using Telerik.Web.UI;
using Website.Library.Enum;
using Website.Library.Global;

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

                RadComboBoxItem item = new RadComboBoxItem(text, value);
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

            // Switch display grid (clear previous viewstate)
            SwitchGrid();
            Grid grid = GetCurrentGrid();

            DataRow row = dataSet.Tables[0].Rows[0];
            btnRefresh.Visible = true;
            btnAdd.Visible = row[MasterDataTable.IsAllowInsert].ToString() == "1";
            btnExport.Visible = row[MasterDataTable.IsAllowExport].ToString() == "1";
            grid.Columns[0].Visible = row[MasterDataTable.IsAllowUpdate].ToString() == "1";
            grid.Columns[1].Visible = row[MasterDataTable.IsAllowDelete].ToString() == "1";
            grid.MasterTableView.DataKeyNames = hidPrimaryKey.Value.Split(';');
            grid.Visible = true;


            // Keep Table Field Settings in ViewState
            Dictionary<string, string> fieldDictionary = new Dictionary<string, string>();
            Dictionary<string, MethodInfo> templateDictionary = new Dictionary<string, MethodInfo>();
            foreach (DataRow item in dataSet.Tables[1].Rows)
            {
                string fieldName = item[MasterDataTable.FieldName].ToString();
                string fieldLabel = item[MasterDataTable.FieldLabel].ToString();
                fieldDictionary.Add(fieldName, fieldLabel);

                string assemblyName = item[MasterDataTable.AssemblyName].ToString();
                string className = item[MasterDataTable.ClassName].ToString();
                string functionName = item[MasterDataTable.FunctionName].ToString();
                Type type = FunctionBase.GetAssemblyType(assemblyName, className);
                if (type == null)
                {
                    continue;
                }
                MethodInfo templateMethod = type.GetMethod(functionName, new[] { typeof(string) });
                if (templateMethod != null)
                {
                    templateDictionary.Add(fieldName, templateMethod);
                }
            }
            ViewState[MasterDataTable.FieldList] = fieldDictionary;
            ViewState[MasterDataTable.FieldValue] = templateDictionary;


            // Bind Data
            grid.EnableViewState = true;
            BindGrid(dataSet.Tables[2]);
            grid.DataBind();
        }

        protected void Export(object sender, EventArgs e)
        {
            DataTable dataTable = MasterDataBusiness.LoadTableData(hidConnectionName.Value,
                hidDatabaseName.Value, hidSchemaName.Value, hidTableName.Value);
            ExportToExcel(dataTable);
        }

        protected void Refresh(object sender, EventArgs e)
        {
            Grid grid = GetCurrentGrid();
            grid.MasterTableView.ClearEditItems();
            BindGrid(null);
            grid.DataBind();
        }

        protected void ProcessGridDataOnNeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            BindGrid(null);
        }

        protected void ProcessGridDataOnColumnCreated(object sender, GridColumnCreatedEventArgs e)
        {
            GridColumn column = e.Column;
            if (column.UniqueName == "ExpandColumn")
            {
                return;
            }
            column.HeaderText = GetHeaderText(column.UniqueName, column.HeaderText);            
        }

        protected void ProcessGridDataOnItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridDataItem == false)
            {
                return;
            }

            Grid grid = GetCurrentGrid();
            GridDataItem item = (GridDataItem)e.Item;
            foreach (GridColumn column in grid.MasterTableView.AutoGeneratedColumns)
            {
                if (item[column.UniqueName].Visible == false)
                {
                    continue;
                }

                if (IsTemplateField(column.UniqueName))
                {
                    item[column.UniqueName].Text = GetTemplateValue(column.UniqueName, item[column.UniqueName].Text);
                }
                else
                {
                    Dictionary<string, MethodInfo> templateDictionary = GetTemplateSettings();
                    if (templateDictionary != null
                        && templateDictionary.ContainsKey(column.UniqueName))
                    {
                        item[column.UniqueName].Text = templateDictionary[column.UniqueName]
                            .Invoke(null, new object[]{ item[column.UniqueName].Text }).ToString();
                    }
                }
            }
        }

        protected void ProcessGridDataOnItemCommand(object source, GridCommandEventArgs e)
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
                case "EditRow":
                    EditData(dictionary);
                    break;

                default:
                    DeleteData(dictionary);
                    break;
            }
        }

        private void BindGrid(DataTable dataSource)
        {
            if (dataSource == null)
            {
                dataSource = MasterDataBusiness.LoadTableData(hidConnectionName.Value,
                    hidDatabaseName.Value, hidSchemaName.Value, hidTableName.Value);
            }
            Grid grid = GetCurrentGrid();
            grid.DataSource = dataSource;
        }


        protected void CreateData(object sender, EventArgs e)
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            EditData(dictionary);
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
                Refresh(null, null);
            }
            else
            {
                ShowMessage("Xóa thông tin thất bại.", ModuleMessage.ModuleMessageType.RedError);
            }
        }


        private Dictionary<string, string> GetFieldSettings()
        {
            return ViewState[MasterDataTable.FieldList] as Dictionary<string, string>;
        }

        private Dictionary<string, MethodInfo> GetTemplateSettings()
        {
            return ViewState[MasterDataTable.FieldValue] as Dictionary<string, MethodInfo>;
        }

        private string GetHeaderText(string fieldName, string headerText)
        {
            Dictionary<string, string> fieldDictionary = GetFieldSettings();
            if (fieldDictionary == null || fieldDictionary.ContainsKey(fieldName) == false)
            {
                return headerText;
            }

            string fieldLabel = fieldDictionary[fieldName];
            return string.IsNullOrWhiteSpace(fieldLabel) ? headerText : fieldLabel;
        }

        private Grid GetCurrentGrid()
        {
            return hidGridID.Value == gridData.ID ? gridData : gridDataReserve;
        }

        private void SwitchGrid()
        {
            gridData.EnableViewState = false;
            gridData.Visible = false;
            gridData.DataSource = null;
            gridDataReserve.EnableViewState = false;
            gridDataReserve.Visible = false;
            gridDataReserve.DataSource = null;

            hidGridID.Value = hidGridID.Value == gridData.ID
                ? gridDataReserve.ID
                : gridData.ID;
        }
    }
}