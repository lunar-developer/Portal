using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Modules.Application.Database;
using Modules.Application.DataTransfer;
using Modules.Application.Global;
using Website.Library.Global;

namespace DesktopModules.Modules.Applic
{
    public partial class SectionPolicy : ApplicationModuleBase
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
            BindApplicationType();
            BindPriority();
        }
        private void BindApplicationType()
        {
            ctApplicationType.Items.Add(new ListItem("Chưa chọn", "-1"));
            BindApplicationTypeData(ctApplicationType);
        }

        private void BindPriority()
        {
            ctPriority.Items.Add(new ListItem("Chưa chọn", "-1"));
            ctPriority.Items.Add(new ListItem("VIP ", "0"));
            ctPriority.Items.Add(new ListItem("Gấp", "1"));
        }

        public List<ApplicationValueData> SectionHeaderData()
        {
            List<ApplicationValueData> dataList = new List<ApplicationValueData>()
            {
                new ApplicationValueData()
                {
                    TableName = ApplicationTable.TableName,
                    FieldName = ApplicationTable.ApplicationTypeID,
                    Value = CacheBase.Receive<ApplicationTypeData>(ctApplicationType.SelectedValue).ApplicationTypeID,
                    FieldType = SqlDbType.TinyInt
                },
                new ApplicationValueData()
                {
                    TableName = ApplicationTable.TableName,
                    FieldName = ApplicationTable.ApplicationStatus,
                    Value = ctApplicStatus.Text ?? "-1",
                    FieldType = SqlDbType.TinyInt
                },
                new ApplicationValueData()
                {
                    TableName = ApplicationTable.TableName,
                    FieldName = ApplicationTable.DecisionCode,
                    Value = ctDecision.Text ?? "-1",
                    FieldType = SqlDbType.VarChar
                },
                new ApplicationValueData()
                {
                    TableName = ApplicationTable.TableName,
                    FieldName = ApplicationTable.UniqueID,
                    Value = "TestTest",
                    FieldType = SqlDbType.VarChar
                },
            };

            return dataList;
        }
    }
}