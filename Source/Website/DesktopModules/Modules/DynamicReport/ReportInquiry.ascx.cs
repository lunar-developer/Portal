using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web.UI;
using Modules.DynamicReport.Business;
using Modules.DynamicReport.Database;
using Modules.DynamicReport.DataTransfer;
using Modules.DynamicReport.Enum;
using Telerik.Web.UI;
using Website.Library.Global;

namespace DesktopModules.Modules.DynamicReport
{
    public partial class ReportInquiry : DesktopModuleBase
    {
        protected override void OnLoad(EventArgs e)
        {
            if (IsPostBack)
            {
                return;
            }

            btnView.Visible = btnExport.Visible = false;
            BindData();
        }

        public void BindData()
        {
            foreach (ReportData reportInfo in ReportBusiness.GetReports(UserInfo.UserID))
            {
                string text = reportInfo.Title.Trim();
                string value = reportInfo.ReportID;

                RadComboBoxItem item = new RadComboBoxItem(text, value);
                item.Attributes.Add(ReportTable.ConnectionName, reportInfo.ConnectionName.Trim());
                item.Attributes.Add(ReportTable.DatabaseName, reportInfo.DatabaseName.Trim());
                item.Attributes.Add(ReportTable.SchemaName, reportInfo.SchemaName.Trim());
                item.Attributes.Add(ReportTable.ProcedureName, reportInfo.ProcedureName.Trim());
                item.Attributes.Add(ReportTable.IsAllowExport, reportInfo.IsAllowExport.Trim());
                ddlReport.Items.Add(item);
            }
        }

        protected void ProcessOnSelectReport(object sender, EventArgs e)
        {
            gridData.Visible = false;
            btnExport.Visible = false;

            if (ddlReport.SelectedValue == string.Empty)
            {
                DivForm.Visible = false;
                btnView.Visible = false;
                return;
            }

            AttributeCollection collection = ddlReport.SelectedItem.Attributes;
            hidConnectionName.Value = collection[ReportTable.ConnectionName];
            hidDatabaseName.Value = collection[ReportTable.DatabaseName];
            hidSchemaName.Value = collection[ReportTable.SchemaName];
            hidProcedureName.Value = collection[ReportTable.ProcedureName];
            hidIsAllowExport.Value = collection[ReportTable.IsAllowExport];

            List<ReportFieldData> listField = ReportBusiness.GetReportSetting(ddlReport.SelectedValue);
            hidListFieldName.Value = string.Join(";", listField.Select(field => field.FieldName));
            RenderForm(listField);
            DivForm.Visible = true;
            btnView.Visible = true;
        }

        private void RenderForm(IReadOnlyList<ReportFieldData> listField)
        {
            int count = 0;
            bool isClose = false;
            StringBuilder html = new StringBuilder("<div class=\"form-group\">");
            for (int i = 0; i < listField.Count; i++)
            {
                // Render field
                html.Append(RenderField(listField[i]));

                // Is wrapping row?
                count++;
                if (count % 2 != 0)
                {
                    continue;
                }
                html.Append("</div>");
                if (i + 1 < listField.Count)
                {
                    html.Append("<div class=\"form-group\">");
                }
                else
                {
                    isClose = true;
                }
            }

            // Is Well Form?
            if (isClose == false)
            {
                html.Append("</div>");
            }
            DivForm.InnerHtml = html.ToString();
            RegisterScript("renderForm()");
        }

        private string RenderField(ReportFieldData fieldInfo)
        {
            bool isRequire = FunctionBase.ConvertToBool(fieldInfo.IsRequire);
            bool isReadOnly = FunctionBase.ConvertToBool(fieldInfo.IsReadOnly);
            string fieldName = fieldInfo.FieldName;
            string fieldValue = fieldInfo.DefaultValue;
            string fieldLabel = fieldInfo.FieldLabel;
            string tooltip = string.Empty;
            string placeHolder = string.IsNullOrWhiteSpace(fieldInfo.PlaceHolder) ? fieldLabel : fieldInfo.PlaceHolder;
            string inputType = fieldInfo.InputType;
            string minLength = fieldInfo.MinLength;
            string maxLength = fieldInfo.MaxLength;

            #region Render Tooltip
            if (string.IsNullOrWhiteSpace(fieldInfo.Tooltip) == false)
            {
                tooltip = $@"
                    <a tabindex=""-1"" class=""dnnFormHelp"" href=""javascript:;""></a>
                    <div class=""dnnTooltip"">
			            <div class=""dnnFormHelpContent dnnClear"">
                            <span class=""dnnHelpText"">{fieldInfo.Tooltip}</span>
                            <a href=""javascript:;"" class=""pinHelp""></a>
                        </div>
                    </div>";
            }
            #endregion

            #region Render HTML
            string control;
            string html = $@"
                <div class=""col-sm-2 control-label c-font-bold"">
                    <div class=""dnnLabel"" style=""position: relative;"">
                        <label>
                            {(isRequire ? "<span class=\"c-font-red-2\">*</span>&nbsp;" : string.Empty)}
                            {fieldLabel}
                        </label>
                        {tooltip}
                    </div>
                </div>
                <div class=""col-sm-4"">
                    {{0}}
                </div>";

            switch (inputType)
            {
                case InputEnum.DropDownList:
                case InputEnum.Combobox:
                    if (isReadOnly)
                    {
                        control = $@"
                                <select class=""form-control {
                                (inputType == InputEnum.Combobox ? inputType : string.Empty)
                            }""
                                        placeholder=""{fieldLabel}""
                                        is-require=""{isRequire}""
                                        disabled=""true"">
                                    {RenderOption(fieldInfo)}
                                </select>
                                <input  type=""hidden""
                                        name=""{fieldName}""
                                        value=""{fieldValue}""/>";
                    }
                    else
                    {
                        control = $@"
                                <select name=""{fieldName}""
                                        class=""form-control {
                                (inputType == InputEnum.Combobox ? inputType : string.Empty)
                            }""
                                        placeholder=""{fieldLabel}""
                                        is-require=""{isRequire}"">
                                    {RenderOption(fieldInfo)}
                                </select>";
                    }
                    break;


                case InputEnum.TextArea:
                    control = $@"
                            <textarea   name=""{fieldName}"" 
                                        class=""form-control""
                                        placeholder=""{placeHolder}""
                                        is-require=""{isRequire}""
                                        minlength = ""{minLength}""
                                        maxlength = ""{maxLength}""
                                        {(isReadOnly ? "readonly=readonly" : string.Empty)}>{fieldValue}</textarea>";
                    break;


                case InputEnum.Calendar:
                    control = $@"<input type=""text""
                                        class=""form-calendar""
                                        name=""{fieldName}""/>";
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
                                    {(isReadOnly ? "readonly=readonly" : string.Empty)}/>";
                    break;
            }

            return string.Format(html, control);
            #endregion
        }

        private string RenderOption(ReportFieldData fieldInfo)
        {
            // Validation
            if (fieldInfo == null)
            {
                return string.Empty;
            }
            string dataSource = fieldInfo.DataSource.Trim();
            string fieldText = fieldInfo.FieldText.Trim();
            string fieldValue = fieldInfo.FieldValue.Trim();
            if (FunctionBase.IsNullOrWhiteSpace(dataSource, fieldText, fieldValue))
            {
                return string.Empty;
            }


            StringBuilder html = new StringBuilder();
            bool isRunTime = fieldInfo.IsRunTime == "1";
            string selectedValue = fieldInfo.DefaultValue;


            // Load Data
            if (isRunTime)
            {
                Type type = Type.GetType(dataSource);
                if (type == null)
                {
                    return html.ToString();
                }

                foreach (object data in ReceiveCache(type))
                {
                    string text = FunctionBase.GetCoalesceString(
                        type.GetField(fieldText)?.GetValue(data) + string.Empty,
                        type.GetProperty(fieldText)?.GetValue(data) + string.Empty);
                    string value = FunctionBase.GetCoalesceString(
                        type.GetField(fieldValue)?.GetValue(data) + string.Empty,
                        type.GetProperty(fieldValue)?.GetValue(data) + string.Empty);
                    bool isSelected = value == selectedValue;
                    html.Append($@"
                        <option value=""{value}""
                                {(isSelected ? "selected=\"true\"" : string.Empty)}>{text}</option>");
                }
            }
            else
            {
                DataTable dataTable = ReportBusiness.LoadOptionData(dataSource);
                foreach (DataRow row in dataTable.Rows)
                {
                    string text = row[fieldText].ToString();
                    string value = row[fieldValue].ToString();
                    bool isSelected = value == selectedValue;
                    html.Append($@"
                        <option value=""{value}""
                                {(isSelected ? "selected=\"true\"" : string.Empty)}>{text}</option>");
                }
            }
            return html.ToString();
        }

        public List<object> ReceiveCache(Type type)
        {
            MethodInfo method = typeof(CacheBase).GetMethod("Receive", new Type[0]).MakeGenericMethod(type);
            return ((IEnumerable<object>) method.Invoke(null, new object[0])).ToList();
        }

        protected void LoadReportData(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(hidListFieldName.Value) == false)
            {
                string[] arrayFields = hidListFieldName.Value.Split(';');
                hidListFieldValue.Value = string.Join(";", arrayFields.Select(fieldName => Request.Form[fieldName]));
            }
            gridData.Visible = true;
            btnExport.Visible = true;
            BindGrid();
            gridData.DataBind();
            upGridData.Update();
        }

        protected void ExportData(object sender, EventArgs e)
        {
            DataTable dtResult = ReportBusiness.GetReportData(hidConnectionName.Value,
                hidDatabaseName.Value, hidSchemaName.Value, hidProcedureName.Value,
                GetParameters());
            ExportToExcel(dtResult, "Report");
        }

        protected void OnNeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            BindGrid();
        }

        private void BindGrid()
        {
            gridData.DataSource = ReportBusiness.GetReportData(hidConnectionName.Value,
                hidDatabaseName.Value, hidSchemaName.Value, hidProcedureName.Value,
                GetParameters());
        }

        private Dictionary<string, string> GetParameters()
        {
            Dictionary<string, string> parameterDictionary = new Dictionary<string, string>();
            string[] listFieldName = hidListFieldName.Value.Split(';');
            string[] listFieldValue = hidListFieldValue.Value.Split(';');
            for (int i = 0; i < listFieldName.Length; i++)
            {
                string fieldName = listFieldName[i];
                if (string.IsNullOrWhiteSpace(fieldName))
                {
                    continue;
                }
                parameterDictionary.Add(fieldName, listFieldValue[i]);
            }
            return parameterDictionary;
        }
    }
}