using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using DotNetNuke.UI.Skins.Controls;
using Modules.MasterData.Business;
using Modules.MasterData.Database;
using Modules.MasterData.Enum;
using Modules.MasterData.Global;
using Website.Library.Database;
using Website.Library.Global;
using DataTable = System.Data.DataTable;

namespace DesktopModules.Modules.MasterData
{
    public partial class DataEditor : MasterDataModuleBase
    {
        private bool IsInsertMode;
        private Dictionary<string, string> ParameterDictionary = new Dictionary<string, string>();


        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack)
            {
                return;
            }

            SetRequestParameter();
            BindData();
            RegisterConfirmDialog(btnDelete, "Bạn có chắc muốn <b>XÓA</b> thông tin này?");
        }


        private void SetRequestParameter()
        {
            hidTableID.Value = Request.QueryString[MasterDataTable.UniqueID];
            hidPrimaryKey.Value = Request.QueryString[MasterDataTable.FieldList];
            if (string.IsNullOrWhiteSpace(hidPrimaryKey.Value) == false)
            {
                ParameterDictionary = hidPrimaryKey.Value.Split(';')
                    .ToDictionary(fieldName => fieldName, fieldName => Request.QueryString[fieldName]);
            }
        }

        private void SetFormParameter()
        {
            ParameterDictionary = hidPrimaryKey.Value.Split(';')
                .ToDictionary(fieldName => fieldName, fieldName => Request.Form[fieldName].Trim());
        }

        private void SetParameter(IReadOnlyDictionary<string, string> dataDictionary)
        {
            ParameterDictionary = hidPrimaryKey.Value.Split(';')
                .ToDictionary(fieldName => fieldName, fieldName => dataDictionary[fieldName]);
        }


        private void BindData()
        {
            // Validate Data
            int tableID;
            if (int.TryParse(hidTableID.Value, out tableID) == false)
            {
                ShowMessage("Không tìm thấy thông tin liên quan.");
                return;
            }

            // Validate Permission
            DataTable tablePermission = MasterDataBusiness.LoadTablePermission(UserInfo.UserID, hidTableID.Value);
            if (tablePermission.Rows.Count == 0)
            {
                ShowMessage("Bạn không có quyền xem thông tin này.");
                return;
            }

            // Load Data
            DataSet dataSet = MasterDataBusiness.LoadRowData(hidTableID.Value, ParameterDictionary);
            if (dataSet == null)
            {
                ShowMessage("Không tìm thấy thông tin liên quan.");
                return;
            }

            // Render Form
            BindData(dataSet);

            // Set Permission
            SetPermission(tablePermission.Rows[0]);
        }

        private void BindData(DataSet dataSet)
        {
            // Binding Data
            // Table 0 - Connection & Cache Settings
            DataRow row = dataSet.Tables[0].Rows[0];
            lblTitle.Text = row[MasterDataTable.Title].ToString().Trim();
            hidConnectionName.Value = row[MasterDataTable.ConnectionName].ToString().Trim();
            hidDatabaseName.Value = row[MasterDataTable.DatabaseName].ToString().Trim();
            hidSchemaName.Value = row[MasterDataTable.SchemaName].ToString().Trim();
            hidTableName.Value = row[MasterDataTable.TableName].ToString().Trim();
            hidAssemblyName.Value = row[MasterDataTable.AssemblyName].ToString().Trim();
            hidCacheName.Value = row[MasterDataTable.CacheName].ToString().Trim();
            hidCacheID.Value = row[MasterDataTable.CacheID].ToString().Trim();

            // Render Controls
            // Table 1 - Field Settings
            // Table 2 - Field Definition
            // Table 3 - Field Value
            RenderForm(dataSet.Tables[1], dataSet.Tables[2],
                dataSet.Tables.Count == 4 && dataSet.Tables[3].Rows.Count > 0 ? dataSet.Tables[3].Rows[0] : null);
        }

        private void RenderForm(DataTable settingTable, DataTable fieldTable, DataRow dataRow)
        {
            int count = 0;
            bool isClose = false;
            IsInsertMode = dataRow == null;
            List<DataRow> listSettingRow = settingTable.Rows.Cast<DataRow>().ToList();
            List<string> listScript = new List<string>(); 
            StringBuilder html = new StringBuilder("<div class=\"form-group\">");
            for (int i = 0; i < fieldTable.Rows.Count; i++)
            {
                // Get field information
                DataRow fieldInfo = fieldTable.Rows[i];
                string fieldName = fieldInfo[MasterDataTable.FieldName].ToString();
                string fieldValue = dataRow?[fieldName].ToString();

                // Get field settings
                DataRow fieldSetting = listSettingRow.FirstOrDefault(
                    iterator => iterator[MasterDataTable.FieldName].ToString() == fieldName);

                // Build control
                string fieldHTML = RenderField(fieldName, fieldValue, fieldInfo, fieldSetting, listScript);
                html.Append(fieldHTML);

                // Is wrapping row?
                count++;
                if (count % 2 != 0)
                {
                    continue;
                }
                html.Append("</div>");
                if (i + 1 < fieldTable.Rows.Count)
                {
                    html.Append("<div class=\"form-group\">");
                }
                else
                {
                    isClose = true;
                }
            }

            // Render Form
            if (isClose == false)
            {
                html.Append("</div>");
            }
            DivForm.InnerHtml = html.ToString();
            RegisterScript(string.Join(Environment.NewLine, listScript));

            // Store list fields
            if (btnSave.Visible || btnUpdate.Visible)
            {
                hidFields.Value = string.Join(";",
                    fieldTable.Rows.Cast<DataRow>().Select(row => row[MasterDataTable.FieldName]));
            }
        }

        private string RenderField(
            string fieldName,
            string fieldValue,
            DataRow fieldInfo,
            DataRow fieldSetting,
            ICollection<string> listScript)
        {
            #region Default Setting
            bool isIdentity = bool.Parse(fieldInfo[MasterDataTable.IsIdentity].ToString());
            bool isPrimary = bool.Parse(fieldInfo[MasterDataTable.IsPrimaryKey].ToString());
            bool isRunTimeField = IsRuntimeField(fieldName);
            bool isReadOnly = isIdentity || isRunTimeField || IsInsertMode == false && isPrimary;
            bool isRequire = isIdentity || isPrimary;
            int minLength = 0;
            int maxLength = int.Parse(fieldInfo[MasterDataTable.MaxLength].ToString());
            string fieldLabel = FunctionBase.SeparateCapitalLetters(fieldName);
            string tooltip = string.Empty;
            string placeHolder = fieldLabel;
            string dataType = fieldInfo[MasterDataTable.DataType].ToString();
            string inputType = InputEnum.Text;
            string onClientChange = string.Empty;
            #endregion

            #region Read User Settings
            if (fieldSetting != null)
            {
                // Read Only?
                if (isReadOnly == false)
                {
                    isReadOnly = bool.Parse(IsInsertMode
                        ? fieldSetting[MasterDataTable.IsDisableOnInsert].ToString()
                        : fieldSetting[MasterDataTable.IsDisableOnUpdate].ToString());
                }

                // Required?
                isRequire = bool.Parse(fieldSetting[MasterDataTable.IsRequire].ToString());

                // Min Length & Max Length
                minLength = int.Parse(fieldSetting[MasterDataTable.MinLength].ToString());

                // Field Label
                string label = fieldSetting[MasterDataTable.FieldLabel].ToString();
                if (string.IsNullOrWhiteSpace(label) == false)
                {
                    fieldLabel = label;
                }

                // Tooltip
                string tip = fieldSetting[MasterDataTable.Tooltip].ToString();
                if (string.IsNullOrWhiteSpace(tip) == false)
                {
                    tooltip = $@"
                    <a tabindex=""-1"" class=""dnnFormHelp"" href=""javascript:;""></a>
                    <div class=""dnnTooltip"">
			            <div class=""dnnFormHelpContent dnnClear"">
                            <span class=""dnnHelpText"">{tip}</span>
                            <a href=""javascript:;"" class=""pinHelp""></a>
                        </div>
                    </div>";
                }

                // PlaceHolder
                label = fieldSetting[MasterDataTable.PlaceHolder].ToString();
                placeHolder = FunctionBase.GetCoalesceString(label, fieldLabel);

                // Input Type
                inputType = fieldSetting[MasterDataTable.InputType].ToString();

                // On Client Change?
                onClientChange = fieldSetting[MasterDataTable.OnClientChange].ToString();
            }
            #endregion

            #region Store Data for Next Request
            if (isIdentity)
            {
                hidIdentityField.Value = fieldName;
            }
            #endregion

            #region Field Value & Template
            fieldValue = isIdentity && IsInsertMode
                ? "NULL"
                : FunctionBase.GetCoalesceString(
                    IsRuntimeField(fieldName) ? GetTemplateValue(fieldName, fieldValue) : fieldValue,
                    fieldSetting?[MasterDataTable.DefaultValue].ToString());
            #endregion

            #region Render HTML
            string control;
            string html = $@"
                <div class=""col-sm-2 control-label c-font-bold"">
                    <div class=""dnnLabel"" style=""position: relative;"">
                        <label>
                            { (isRequire ? "<span class=\"c-font-red-2\">*</span>&nbsp;" : string.Empty)}
                            {fieldLabel}
                        </label>
                        {tooltip}
                    </div>
                </div>
                <div class=""col-sm-4"">
                    {{0}}
                </div>";


            // Fixed Control (Bit)
            if (dataType == SQLDataTypeEnum.Bit)
            {
                string value = FunctionBase.ConvertToBool(fieldValue) ? "1" : "0";
                string options = $@"{{
                    ""name"": ""{fieldName}"",
                    ""emptyMessage"": ""{placeHolder}"",
                    ""options"": [
                        {{ ""text"": ""No"",  ""value"": ""0"", ""selected"": {(value == "0").ToString().ToLower()} }},
                        {{ ""text"": ""Yes"", ""value"": ""1"", ""selected"": {(value == "1").ToString().ToLower()} }}
                    ],
                    ""lazyLoad"": false,
                    ""onChange"": """"
                }}";
                listScript.Add($"declareVariable(\"glo_MasterData_{fieldName}\", {options});");
                control = RenderRadCombobox(fieldName, value, isRequire, isReadOnly);
            }
            else
            {
                switch (inputType)
                {
                    case InputEnum.Combobox:
                        string options = RenderOption(fieldName, fieldValue, placeHolder, fieldSetting, onClientChange);
                        listScript.Add($"declareVariable(\"glo_MasterData_{fieldName}\", {options});");
                        control = RenderRadCombobox(fieldName, fieldValue, isRequire, isReadOnly);
                        break;


                    case InputEnum.TextArea:
                        control = $@"
                            <textarea   name=""{fieldName}"" 
                                        class=""form-control""
                                        placeholder=""{placeHolder}""
                                        is-require=""{isRequire}""
                                        minlength = ""{minLength}""
                                        maxlength = ""{maxLength}""
                                        onchange = ""{onClientChange}""
                                        {(isReadOnly ? "readonly=readonly" : string.Empty)}>{fieldValue}</textarea>";
                        break;


                    default:
                        control = $@"
                            <input  name=""{fieldName}"" 
                                    type =""text"" 
                                    class=""form-control"" 
                                    value=""{fieldValue}""
                                    placeholder=""{placeHolder}""
                                    is-require=""{isRequire}""
                                    minlength = ""{minLength}""
                                    maxlength = ""{maxLength}""
                                    onchange = ""{onClientChange}""
                                    {(isReadOnly ? "readonly=readonly" : string.Empty)}/>";
                        break;
                }
            }
            return string.Format(html, control);
            #endregion            
        }

        private static string RenderRadCombobox(
            string fieldName,
            string fieldValue,
            bool isRequire,
            bool isReadOnly)
        {
            return $@"
                <div class=""RadComboBox RadComboBox_Default"" id=""{fieldName}"">
                    <span class=""rcbInner {(isReadOnly ? "rcbDisabled" : string.Empty)}"">
                        <input type=""text"" id=""{fieldName}_Input"" class=""rcbInput radPreventDecorate"" autocomplete=""off"" {(isReadOnly ? "disabled=\"disabled\"" : string.Empty)}>
                        <button type=""button"" class=""rcbActionButton"" tabindex=""-1"">
                            <span class=""rcbIcon rcbIconDown""></span>
                            <span class=""rcbButtonText""></span>
                        </button>
                    </span>
                    <div style=""z-index:6000;"" class=""rcbSlide"">
                            <div style=""display: none;"" class=""RadComboBoxDropDown RadComboBoxDropDown_Default "" id=""{fieldName}_DropDown"">
                            <div style=""height:200px; width:100%;"" class=""rcbScroll rcbWidth"">
			                    <ul style=""list-style: none; margin: 0; padding: 0; zoom: 1;"" class=""rcbList"">
			                    </ul>
		                    </div>
	                    </div>
                    </div>
                    <input type=""hidden"" id=""{fieldName}_Hidden"" name=""{fieldName}"" value=""{fieldValue}"" is-require=""{isRequire}""/>
                </div>
            ";
        }

        private string RenderOption(string fieldName, string selectedValue, string placeHolder, DataRow fieldSetting, string onClientChange)
        {
            List<Dictionary<string, object>> listOptions =  new List<Dictionary<string, object>>();
            Dictionary<string, object> dataDictionary = new Dictionary<string, object>
            {
                { "name", fieldName },
                { "emptyMessage", placeHolder },
                { "options", listOptions },
                { "lazyLoad", false },
                { "onChange", onClientChange }
            };

            if (fieldSetting == null)
            {
                goto EndPoint;
            }

            string dataSource = fieldSetting[MasterDataTable.DataSource].ToString();
            string fieldText = fieldSetting[MasterDataTable.FieldText].ToString().Trim();
            string fieldValue = fieldSetting[MasterDataTable.FieldValue].ToString().Trim();
            if (FunctionBase.IsNullOrWhiteSpace(dataSource, fieldText, fieldValue))
            {
                goto EndPoint;
            }

            // Is support Lazy Loading?
            bool isLazyLoad = bool.Parse(fieldSetting[MasterDataTable.IsLazyLoad].ToString());
            dataDictionary["lazyLoad"] = isLazyLoad;

            // Load Data
            bool isRunTime = bool.Parse(fieldSetting[MasterDataTable.IsRunTime].ToString());
            string fieldGroup = fieldSetting[MasterDataTable.FieldGroup].ToString().Trim();
            if (isRunTime)
            {
                Type type = Type.GetType(dataSource);
                if (type == null)
                {
                    goto EndPoint;
                }
                
                foreach (object data in ReceiveCache(type))
                {
                    string text = FunctionBase.GetCoalesceString(
                        type.GetField(fieldText)?.GetValue(data) + string.Empty,
                        type.GetProperty(fieldText)?.GetValue(data) + string.Empty);
                    string value = FunctionBase.GetCoalesceString(
                        type.GetField(fieldValue)?.GetValue(data) + string.Empty,
                        type.GetProperty(fieldValue)?.GetValue(data) + string.Empty);
                    string group = FunctionBase.GetCoalesceString(
                        type.GetField(fieldGroup)?.GetValue(data) + string.Empty,
                        type.GetProperty(fieldGroup)?.GetValue(data) + string.Empty);
                    string isDisable = FunctionBase.GetCoalesceString(
                        type.GetField(BaseTable.IsDisable)?.GetValue(data) + string.Empty,
                        type.GetProperty(BaseTable.IsDisable)?.GetValue(data) + string.Empty);
                    bool isSelected = value == selectedValue;

                    listOptions.Add(new Dictionary<string, object>
                    {
                        { "text", text },
                        { "value", value },
                        { "group", group },
                        { "selected", isSelected },
                        { "disabled", FunctionBase.ConvertToBool(isDisable) }
                    });
                }
            }
            else
            {
                DataTable dataTable = MasterDataBusiness.LoadOptionData(dataSource);
                bool isContainGroup = dataTable.Columns.Contains(fieldGroup);
                foreach (DataRow row in dataTable.Rows)
                {
                    string text = row[fieldText].ToString();
                    string value = row[fieldValue].ToString();
                    string group = isContainGroup ? row[fieldGroup].ToString() : string.Empty;
                    bool isSelected = value == selectedValue;

                    listOptions.Add(new Dictionary<string, object>
                    {
                        { "text", text },
                        { "value", value },
                        { "group", group },
                        { "selected", isSelected },
                        { "disabled", false }
                    });
                }
            }

            EndPoint:
            return FunctionBase.Serialize(dataDictionary);
        }

        private void SetPermission(DataRow row)
        {
            btnSave.Visible = IsInsertMode && row[MasterDataTable.IsAllowInsert].ToString() == "1";
            btnUpdate.Visible = IsInsertMode == false && row[MasterDataTable.IsAllowUpdate].ToString() == "1";
            btnDelete.Visible = IsInsertMode == false && row[MasterDataTable.IsAllowDelete].ToString() == "1";
        }

        private Dictionary<string, string> GetData()
        {
            string[] arrayFields = hidFields.Value.Split(';');
            Dictionary<string, string> dataDictionary = new Dictionary<string, string>();
            foreach (string fieldName in arrayFields)
            {
                // Bypass identity field when in insert mode
                if (IsInsertMode && hidIdentityField.Value == fieldName)
                {
                    continue;
                }

                string fieldValue = IsRuntimeField(fieldName)
                    ? GetRuntimeValue(fieldName)
                    : Request.Form[fieldName].Trim();
                dataDictionary.Add(fieldName, fieldValue);
            }
            return dataDictionary;
        }

        private Dictionary<string, string> GetPrimaryData()
        {
            string[] arrayFields = hidPrimaryKey.Value.Split(';');
            Dictionary<string, string> dataDictionary = new Dictionary<string, string>();
            foreach (string fieldName in arrayFields)
            {
                string fieldValue = Request.Form[fieldName].Trim();
                dataDictionary.Add(fieldName, fieldValue);
            }
            return dataDictionary;
        }

        protected void InsertData(object sender, EventArgs e)
        {
            IsInsertMode = true;
            Dictionary<string, string> dataDictionary = GetData();
            int result = MasterDataBusiness.InsertData(hidConnectionName.Value,
                hidDatabaseName.Value, hidSchemaName.Value, hidTableName.Value,
                dataDictionary);

            if (result > 0
                || result == 0 && hidIdentityField.Value == string.Empty)
            {
                // Identity
                if (string.IsNullOrWhiteSpace(hidIdentityField.Value) == false)
                {
                    dataDictionary.Add(hidIdentityField.Value, result.ToString());
                }

                // Reload Cache
                ReloadCache(hidAssemblyName.Value, hidCacheName.Value, hidCacheID.Value, dataDictionary);

                // Reload Screen
                SetParameter(dataDictionary);
                BindData();
                ShowMessage("Cập nhật thông tin thành công.", ModuleMessage.ModuleMessageType.GreenSuccess);
            }
            else
            {
                ShowMessage("Cập nhật thông tin thất bại.", ModuleMessage.ModuleMessageType.RedError);
            }
        }

        protected void UpdateData(object sender, EventArgs e)
        {
            IsInsertMode = false;
            List<string> listKey = hidPrimaryKey.Value.Split(';').ToList();
            Dictionary<string, string> dataDictionary = GetData();

            // Remove Identity field when it is not Primary Key
            if (string.IsNullOrWhiteSpace(hidIdentityField.Value) == false
                && listKey.Contains(hidIdentityField.Value) == false)
            {
                dataDictionary.Remove(hidIdentityField.Value);
            }

            // Process
            bool result = MasterDataBusiness.UpdateData(hidConnectionName.Value,
                hidDatabaseName.Value, hidSchemaName.Value, hidTableName.Value,
                listKey, dataDictionary);
            if (result)
            {
                // Reload Cache
                ReloadCache(hidAssemblyName.Value, hidCacheName.Value, hidCacheID.Value, dataDictionary);

                // Reload Screen
                SetFormParameter();
                BindData();
                ShowMessage("Cập nhật thông tin thành công.", ModuleMessage.ModuleMessageType.GreenSuccess);
            }
            else
            {
                ShowMessage("Cập nhật thông tin thất bại.", ModuleMessage.ModuleMessageType.RedError);
            }
        }

        protected void DeleteData(object sender, EventArgs e)
        {
            IsInsertMode = false;
            Dictionary<string, string> dataDictionary = GetPrimaryData();
            bool result = MasterDataBusiness.DeleteData(hidConnectionName.Value,
                hidDatabaseName.Value, hidSchemaName.Value, hidTableName.Value,
                dataDictionary);
            if (result)
            {
                // Remove Cache
                string key = dataDictionary.ContainsKey(hidCacheID.Value)
                    ? dataDictionary[hidCacheID.Value]
                    : GetCacheIDValue(hidAssemblyName.Value, hidCacheName.Value, hidCacheID.Value, dataDictionary);
                RemoveCache(hidAssemblyName.Value, hidCacheName.Value, key);

                // Notify and Close
                string script = GetAlertScript("Xóa thông tin thành công", null, false, GetCloseScript());
                RegisterScript(script);
            }
            else
            {
                ShowMessage("Xóa thông tin thất bại.", ModuleMessage.ModuleMessageType.RedError);
            }
        }
    }
}