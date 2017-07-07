using System.Data;
using System.Text;
using Modules.Application.Database;
using Modules.Application.Global;
using Website.Library.Database;
using Website.Library.Global;

namespace DesktopModules.Modules.Application.Controls
{
    public partial class SectionHistoryInfo : ApplicationFormModuleBase
    {
        private const string TableLog = @"
            <table class=""table c-margin-t-10"">
                <colgroup>
                    <col width=""15%"" />
                    <col width=""30%"" />
                    <col width=""20%"" />
                    <col width=""30%"" />
                    <col width=""5%"" />
                </colgroup>
                <thead>
                    <tr>
                        <th>Thời Gian</th>
                        <th>Người thực hiện</th>
                        <th>Hành động</th>
                        <th>Ghi chú</th>
                        <th>#</th>
                    </tr>
                </thead>
                <tbody>{0}</tbody>
            </table>";


        public void BindData(string applicationID, string historyUrl, DataTable logTable)
        {
            StringBuilder html = new StringBuilder();
            foreach (DataRow row in logTable.Rows)
            {
                bool isHasLogDetail = row[ApplicationLogTable.IsHasLogDetail].ToString() == "1";
                string link = isHasLogDetail
                    ? $@"<a href='{string.Format(historyUrl, row[ApplicationLogTable.ApplicationLogID])}' target='_blank'>
                            <i class='fa fa-eye'></i>
                         </a>"
                    : string.Empty;

                html.Append($@"
                    <tr>
                        <td>{FunctionBase.FormatDate(row[BaseTable.ModifyDateTime].ToString())}</td>
                        <td>{FunctionBase.FormatUserID(row[BaseTable.ModifyUserID].ToString())}</td>
                        <td>{row[ApplicationLogTable.LogAction]}</td>
                        <td>{row[ApplicationLogTable.Remark]}</td>
                        <td>{link}</td>
                    </tr>
                ");
            }
            DivLog.InnerHtml = string.Format(TableLog, html);
        }
    }
}