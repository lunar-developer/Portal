using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Modules.Application.Database;
using Modules.Application.DataTransfer;
using Modules.Application.Enum;
using Modules.Application.Global;
using Website.Library.Global;

namespace DesktopModules.Modules.Applic
{
    public partial class SectionOtherService : ApplicationModuleBase
    {
        protected override void OnLoad(EventArgs e)
        {
            try
            {
                if (IsPostBack)
                {
                    return;
                }
                AutoWire();
                BindData();
            }
            catch (Exception exception)
            {
                ShowException(exception);
            }
            finally
            {
                //SetPermission();
            }
        }

        private void BindData()
        {

        }
        public List<ApplicationValueData> SectionOtherServiceData()
        {
            List<ApplicationValueData> dataList = new List<ApplicationValueData>()
            {
                new ApplicationValueData()
                {
                    TableName = ApplicationFieldTypeTable.TableNameString,
                    FieldName = ApplicationFieldNameEnum.DebitAccountType,
                    Value = ctlDebitAccountType.SelectedValue,
                    FieldType = SqlDbType.VarChar
                },
                new ApplicationValueData()
                {
                    TableName = ApplicationFieldTypeTable.TableNameString,
                    FieldName = ApplicationFieldNameEnum.LocalDebitAccountNumber,
                    Value = ctLocalDebitAccountNumber.SelectedValue ?? "-1",
                    FieldType = SqlDbType.VarChar
                },
                new ApplicationValueData()
                {
                    TableName = ApplicationFieldTypeTable.TableNameString,
                    FieldName = ApplicationFieldNameEnum.InternationalDebitAccountNumber,
                    Value = ctInternationalDebitAccountNumber.SelectedValue ?? "-1",
                    FieldType = SqlDbType.VarChar
                },
            };
            List<ApplicationValueData> eBankingService = GetEbankingService();
            if (eBankingService != null && eBankingService.Count > 0)
            {
                dataList.AddRange(eBankingService);
            }
            
            return dataList;
        }

        private List<ApplicationValueData> GetEbankingService()
        {
            List<ApplicationValueData> list = new List<ApplicationValueData>();
            foreach (ListItem chk in cteBankingService.Items)
            {
                ApplicationValueData data = new ApplicationValueData()
                {
                    TableName = ApplicationFieldTypeTable.TableNameInteger,
                    FieldName = chk.Value,
                    Value = chk.Selected ? "1":"0",
                    FieldType = SqlDbType.Int
                };
                list.Add(data);
            }
            if (!string.IsNullOrWhiteSpace(ctEBankMobileNumber.Text))
            {
                ApplicationValueData data = new ApplicationValueData()
                {
                    TableName = ApplicationFieldTypeTable.TableNameString,
                    FieldName = ApplicationFieldNameEnum.EBankMobileNumber,
                    Value = ctEBankMobileNumber.Text,
                    FieldType = SqlDbType.VarChar
                };
                list.Add(data);
            }
            if (!string.IsNullOrWhiteSpace(cteBankingUsername.Text))
            {
                ApplicationValueData data = new ApplicationValueData()
                {
                    TableName = ApplicationFieldTypeTable.TableNameString,
                    FieldName = ApplicationFieldNameEnum.EBankUsername,
                    Value = cteBankingUsername.Text,
                    FieldType = SqlDbType.VarChar
                };
                list.Add(data);
            }
            return list;
        }
    }
}