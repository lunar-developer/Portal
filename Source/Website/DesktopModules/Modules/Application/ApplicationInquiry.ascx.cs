using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.WebControls;
using Modules.Application.Business;
using Modules.Application.Database;
using Modules.Application.DataTransfer;
using Modules.Application.Global;
using Modules.UserManagement.DataTransfer;
using Telerik.Web.UI;
using Website.Library.DataTransfer;
using Website.Library.Enum;
using Website.Library.Global;

namespace DesktopModules.Modules.Application
{
    public partial class ApplicationInquiry : ApplicationModuleBase
    {
        #region INTERNAL CLASS

        internal class OptionData
        {
            public string Text;
            public string Value;
            public string ClassGuid;

            public OptionData(string text, string value, string classGuid)
            {
                Text = text;
                Value = value;
                ClassGuid = classGuid;
            }
        }

        #endregion

        #region DEFAULT PROPERTY

        private List<OptionData> ListFieldOption;

        private List<OptionData> FieldOption => ListFieldOption ?? (ListFieldOption = new List<OptionData>
        {
            new OptionData("Chưa chọn", string.Empty, null),

            new OptionData("Chi nhánh", ApplicationTable.SourceBranchCode,
                FunctionBase.GetClassGuid(typeof(BranchData))),

            new OptionData("Chính sách", ApplicationTable.PolicyCode,
                FunctionBase.GetClassGuid(typeof(PolicyData))),

            new OptionData("Loại hồ sơ", ApplicationTable.ApplicationTypeID,
                FunctionBase.GetClassGuid(typeof(ApplicationTypeData))),

            new OptionData("Loại chứng từ", ApplicationTable.IdentityTypeCode,
                FunctionBase.GetClassGuid(typeof(IdentityTypeData)))
        });

        private Dictionary<string, string> ListFieldInput;

        private Dictionary<string, string> FieldInput => ListFieldInput
            ?? (ListFieldInput = new Dictionary<string, string>
            {
                { string.Empty, "Chưa chọn" },
                { ApplicationTable.UniqueID, "Mã hồ sơ" },
                { ApplicationTable.ApplicationID, "Số thứ tự" },
                { ApplicationTable.CustomerID, "CMND/Hộ Chiếu" },
                { ApplicationTable.FullName, "Họ và Tên" },
                { ApplicationTable.Mobile01, "Số điện thoại" },
                { ApplicationTable.Email01, "Email" }
            });

        private Dictionary<string, string> ListFieldDate;

        private Dictionary<string, string> FieldDate => ListFieldDate
            ?? (ListFieldDate = new Dictionary<string, string>
            {
                { string.Empty, "Chưa chọn" },
                { ApplicationTable.CreateDateTime, "Ngày tạo" },
                { ApplicationTable.ExportDate, "Ngày chạy Batch" }
            });

        #endregion


        protected override void OnLoad(EventArgs e)
        {
            if (IsPostBack)
            {
                return;
            }
            BindData();
        }

        protected void Search(object sender, EventArgs e)
        {
            hidInputName01.Value = ddlInputName01.SelectedValue;
            hidInputName02.Value = ddlInputName02.SelectedValue;
            hidDateField.Value = ddlDateField.SelectedValue;
            hidSelectName01.Value = ddlSelectName01.SelectedValue;
            hidSelectName02.Value = ddlSelectName02.SelectedValue;
            hidSelectName03.Value = ddlSelectName03.SelectedValue;

            hidInputValue01.Value = BuildQueryData(txtInputValue01.Text.Trim());
            hidInputValue02.Value = BuildQueryData(txtInputValue02.Text.Trim());
            hidFromDate.Value = calFromDate.SelectedDate?.ToString(PatternEnum.Date);
            hidToDate.Value = calToDate.SelectedDate?.ToString(PatternEnum.Date);
            hidSelectValue01.Value = ddlSelectValue01.SelectedValue;
            hidSelectValue02.Value = ddlSelectValue02.SelectedValue;
            hidSelectValue02.Value = ddlSelectValue02.SelectedValue;

            BindGridData();
        }

        protected void LoadData(object sender, EventArgs e)
        {
            RadComboBox source = sender as RadComboBox;
            if (source == null)
            {
                return;
            }

            // Identify Target
            RadComboBox target;
            if (source.ID == ddlSelectName01.ID)
            {
                target = ddlSelectValue01;
            }
            else if (source.ID == ddlSelectName02.ID)
            {
                target = ddlSelectValue02;
            }
            else
            {
                target = ddlSelectValue03;
            }

            // Load Data
            target.Enabled = source.SelectedIndex > 0;
            target.Items.Clear();
            if (target.Enabled)
            {
                LoadOptionData(source.SelectedItem.Attributes["ClassGuid"], target);
            }
        }

        protected void OnNeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            gridData.DataSource = QueryData();
        }


        private void BindData()
        {
            BindPickListField(ddlSelectName01, FieldOption);
            BindPickListField(ddlSelectName02, FieldOption);
            BindPickListField(ddlSelectName03, FieldOption);

            BindInputField(ddlInputName01, FieldInput);
            BindInputField(ddlInputName02, FieldInput);
            BindInputField(ddlDateField, FieldDate);

            ddlSelectValue01.Enabled = ddlSelectValue02.Enabled = ddlSelectValue03.Enabled = false;
            //ddlInputName01.SelectedValue = ApplicationTable.CustomerID;
            calFromDate.SelectedDate = DateTime.Now.AddDays(-1);
            calToDate.SelectedDate = DateTime.Now;
        }

        private static void BindPickListField(RadComboBox dropDownList, IEnumerable<OptionData> listOption)
        {
            foreach (OptionData option in listOption)
            {
                RadComboBoxItem item = new RadComboBoxItem(option.Text, option.Value);
                item.Attributes.Add("ClassGuid", option.ClassGuid);
                dropDownList.Items.Add(item);
            }
        }

        private static void BindInputField(RadComboBox dropDownList, Dictionary<string, string> fieldDictionary)
        {
            foreach (KeyValuePair<string, string> pair in fieldDictionary)
            {
                dropDownList.Items.Add(new RadComboBoxItem(pair.Value, pair.Key));
            }
        }

        private void LoadOptionData(string classGuid, RadComboBox dropDownList)
        {
            if (classGuid == FunctionBase.GetClassGuid(typeof(BranchData)))
            {
                BindBranchData(dropDownList);
            }
            else if (classGuid == FunctionBase.GetClassGuid(typeof(PolicyData)))
            {
                BindPolicyData(dropDownList);
            }
            else if (classGuid == FunctionBase.GetClassGuid(typeof(ApplicationTypeData)))
            {
                BindApplicationTypeData(dropDownList);
            }
            else if (classGuid == FunctionBase.GetClassGuid(typeof(IdentityTypeData)))
            {
                BindIdentityTypeData(dropDownList);
            }
        }

        private void BindGridData()
        {
            gridData.Visible = true;
            gridData.DataSource = QueryData();
            gridData.DataBind();
        }

        private DataTable QueryData()
        {
            Dictionary<string, SQLParameterData> conditionDictionary = new Dictionary<string, SQLParameterData>();
            AddCondition(hidInputName01, hidInputValue01, conditionDictionary);
            AddCondition(hidInputName02, hidInputValue02, conditionDictionary);
            AddCondition(hidSelectName01, hidSelectValue01, conditionDictionary);
            AddCondition(hidSelectName02, hidSelectValue02, conditionDictionary);
            AddCondition(hidSelectName03, hidSelectValue03, conditionDictionary);

            // Date Field
            if (string.IsNullOrWhiteSpace(hidDateField.Value) == false)
            {
                conditionDictionary.Add("DateField", new SQLParameterData(hidDateField.Value, SqlDbType.VarChar));
                conditionDictionary.Add("FromDate", new SQLParameterData(hidFromDate.Value, SqlDbType.Int));
                conditionDictionary.Add("ToDate", new SQLParameterData(hidToDate.Value, SqlDbType.Int));
            }

            return ApplicationBusiness.SearchApplication(conditionDictionary);
        }

        private static void AddCondition(HiddenField fieldName, HiddenField fieldValue,
            IDictionary<string, SQLParameterData> dictionary)
        {
            if (string.IsNullOrWhiteSpace(fieldName.Value))
            {
                return;
            }
            dictionary.Add(fieldName.ID.Replace("hid", string.Empty), new SQLParameterData(fieldName.Value, SqlDbType.NVarChar));
            dictionary.Add(fieldValue.ID.Replace("hid", string.Empty), new SQLParameterData(fieldValue.Value, SqlDbType.NVarChar));
        }

        private static string BuildQueryData(string value)
        {
            string[] listData = value.Split(';');
            return "N'" + string.Join("', N'", listData) + "'";
        }

        protected string GetEditUrl(string applicationID)
        {
            return EditUrl(ApplicationTable.ApplicationID, applicationID, "Edit");
        }
    }
}