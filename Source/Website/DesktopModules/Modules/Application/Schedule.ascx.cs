using System;
using Modules.Application.Business;
using Modules.Application.DataTransfer;
using Modules.Application.Global;
using Telerik.Web.UI;
using Website.Library.Global;

namespace DesktopModules.Modules.Application
{
    public partial class Schedule : ApplicationModuleBase
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
            btnRefresh.Visible = btnClear.Visible = false;
            BindScheduleData(ddlSchedule, GetEmptyItem());
            RegisterConfirmDialog(btnClear, "Bạn muốn xoá toàn bộ nhật ký?");
        }

        #region HTML Template
        private const string HtmlStringResult = @"
            <table class=""table c-margin-t-10"">
                <colgroup>
                    <col width=""10%"" />
                    <col width=""10%"" />
                    <col width=""10%"" />
                    <col width=""20%"" />
                    <col width=""50%"" />
                </colgroup>
                <thead>
                    <tr>
                        <th>Bắt đầu</th>
                        <th>Kết thúc</th>
                        <th>Chu kỳ</th>
                        <th>Đang hoạt động</th>
                        <th>Ghi chú</th>
                    </tr>
                </thead>
                <tbody>{0}</tbody>
            </table>";
        #endregion

        protected void OnSelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            if (ddlSchedule.SelectedValue == string.Empty)
            {
                btnRefresh.Visible = btnClear.Visible = false;
                DivInfo.Visible = false;
                return;
            }

            // Bind Schedule Info
            DivInfo.Visible = true;
            btnRefresh.Visible = btnClear.Visible = true;
            string scheduleCode = ddlSchedule.SelectedValue;
            ScheduleData scheduleData = CacheBase.Receive<ScheduleData>(scheduleCode);
            string info = string.Empty;
            if (scheduleData != null)
            {
                info = $@"
                    <tr>
                        <td>{FunctionBase.FormatHourAndMinutes(scheduleData.StartTime)}</td>
                        <td>{FunctionBase.FormatHourAndMinutes(scheduleData.EndTime)}</td>
                        <td>{scheduleData.Period.PadLeft(2, '0')} phút</td>
                        <td>{FormatState(scheduleData.IsDisable, false)}</td>
                        <td>{scheduleData.Remark}</td>
                    </tr>
                ";
            }
            DivSheduleInfo.InnerHtml = string.Format(HtmlStringResult, info);

            // Bind Schedule Log
            GridData.DataSource = ScheduleLogBusiness.GetList(scheduleCode);
            GridData.DataBind();
        }

        protected void OnNeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            GridData.DataSource = ScheduleLogBusiness.GetList(ddlSchedule.SelectedValue);
        }

        protected string FormatState(string status, bool expected = true)
        {
            return FunctionBase.FormatState(status, expected);
        }

        protected void Refresh(object sender, EventArgs e)
        {
            GridData.DataSource = ScheduleLogBusiness.GetList(ddlSchedule.SelectedValue);
            GridData.DataBind();
        }

        protected void ClearLog(object sender, EventArgs e)
        {
            ScheduleLogBusiness.Purge(ddlSchedule.SelectedValue);
            Refresh(sender, e);
        }
    }
}