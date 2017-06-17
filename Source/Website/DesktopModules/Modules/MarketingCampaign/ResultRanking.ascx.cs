using DotNetNuke.UI.Skins.Controls;
using Modules.MarketingCampaign.Business;
using Modules.MarketingCampaign.Database;
using System;
using System.Data;
using System.Text;
using Modules.MarketingCampaign.DataTransfer;
using Modules.MarketingCampaign.Enum;
using Website.Library.Global;
using System.Collections.Generic;
using DocumentFormat.OpenXml.Drawing;

namespace DesktopModules.Modules.MarketingCampaign
{
    public partial class ResultRanking : DesktopModuleBase
	{
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            txtStaffID.Attributes.Add("placeholder", GetResource("lblStaffID.Help"));
        }

        protected override void OnLoad(EventArgs e)
        {
            if (IsPostBack == false)
            {
                BindData(ReportTypeEnum.Year);
                BindData(ReportTypeEnum.Week);
            }
        }

        #region HTML Template
        private const string HtmlStringResult = @"
            <table class=""table c-margin-t-10"">
                <colgroup>
                    <col width=""10%"" />
                    <col width=""10%"" />
                    <col width=""20%"" />
                    <col width=""20%"" />
                    <col width=""30%"" />
                    <col width=""10%"" />
                </colgroup>
                <thead>
                    <tr>
                        <th class=""text-center"">Hạng</th>
                        <th>Mã NV</th>
                        <th>Họ & Tên</th>
                        <th>Chức danh</th>
                        <th>Đơn Vị</th>
                        <th>Điểm</th>
                        </tr>
                </thead>
                <tbody>{0}</tbody>
            </table>";
        private const string HtmlStringSearch = @"
            <table class=""table c-margin-t-10"">
                <colgroup>
                    <col width=""5%"" />
                    <col width=""10%"" />
                    <col width=""20%"" />
                    <col width=""15%"" />
                    <col width=""20%"" />
                    <col width=""10%"" />
                    <col width=""20%"" />
                </colgroup>
                <thead>
                    <tr>
                        <th class=""text-center"">Hạng</th>
                        <th>Mã NV</th>
                        <th>Họ & Tên</th>
                        <th>Chức danh</th>
                        <th>Đơn Vị</th>
                        <th>Điểm</th>
                        <th>Thời gian</th>
                        </tr>
                </thead>
                <tbody>{0}</tbody>
            </table>";
        #endregion
        private void BindData(string reportType)
	    {
            StringBuilder sbManager = new StringBuilder();
            StringBuilder sbStaff = new StringBuilder();
	        DataTable dtResult = ResultBusiness.LoadResult(5, reportType);
	        foreach (DataRow row in dtResult.Rows)
	        {
	            bool isManager = row[ResultTable.UserGroup].ToString() == "1";
                StringBuilder data = new StringBuilder();
                string year = row[ResultTable.ReportYear].ToString();
                data.Append("<tr>");
	            data.Append($"<td class=\"text-center\">{row[ResultTable.Ranking].ToString().PadLeft(2, '0')}</td>");
                data.Append($"<td>{row[ResultTable.StaffID]}</td>");
                data.Append($"<td>{row[ResultTable.FullName]}</td>");
	            data.Append($"<td>{row[ResultTable.Title]}</td>");
	            data.Append($"<td>{row[ResultTable.BranchName]}</td>");
	            data.Append($"<td>{FunctionBase.FormatDecimal(row[ResultTable.Point].ToString())}</td>");
                #region Report Number
                string reportName;
                switch (reportType)
                {
                    case ReportTypeEnum.Week:
                        dsWeek.Visible = true;
                        titleWeek.InnerHtml = $"Danh sách Top 5 Tuần {row[ResultTable.ReportNum]} Năm {year}";
                        reportName = $"Tuần {row[ResultTable.ReportNum].ToString().PadLeft(2, '0')} ";
                        break;
                    case ReportTypeEnum.Month:
                        dsMonth.Visible = true;
                        titleMonth.InnerHtml = $"Danh sách Top 5 Tháng {row[ResultTable.ReportNum]} Năm {year}";
                        reportName = $"Tháng {row[ResultTable.ReportNum].ToString().PadLeft(2, '0')} ";
                        break;
                    case ReportTypeEnum.Year:
                        dsYear.Visible = true;
                        titleYear.InnerHtml = $"Danh sách Top 5 Năm {year}";
                        reportName = string.Empty;
                        break;
                    default:
                        reportName = string.Empty;
                        break;
                }
                #endregion
                data.Append("</tr>");
	            if (isManager)
	            {
	                sbManager.Append(data);
	            }
	            else
	            {
	                sbStaff.Append(data);
	            }
                
            }
	        if (reportType.Equals(ReportTypeEnum.Year))
	        {
                DivTopManagerByYear.InnerHtml = string.Format(HtmlStringResult, sbManager);
                DivTopStaffByYear.InnerHtml = string.Format(HtmlStringResult, sbStaff);
            }
            if (reportType.Equals(ReportTypeEnum.Month))
            {
                DivTopManagerByMonth.InnerHtml = string.Format(HtmlStringResult, sbManager);
                DivTopStaffByMonth.InnerHtml = string.Format(HtmlStringResult, sbStaff);
            }
            if (reportType.Equals(ReportTypeEnum.Week))
            {
                DivTopManagerByWeek.InnerHtml = string.Format(HtmlStringResult, sbManager);
                DivTopStaffByWeek.InnerHtml = string.Format(HtmlStringResult, sbStaff);
            }
        }

        protected void Search(object sender, EventArgs e)
	    {
            try
            {
                string staffID = txtStaffID.Text.Trim();
                List<ResultData> resultList = ResultBusiness.SearchResult(staffID);
                if (resultList == null || resultList.Count == 0)
                {
                    ShowMessage("Không tìm thấy kết quả!");
                }
                else
                {
                    StringBuilder sbResult = new StringBuilder();
                    foreach (ResultData resultData in resultList)
                    {
                        StringBuilder data = new StringBuilder();
                        data.Append("<tr>");
                        data.Append($"<td class=\"text-center\">{resultData.Ranking.PadLeft(2, '0')}</td>");
                        data.Append($"<td>{staffID}</td>");
                        data.Append($"<td>{resultData.FullName}</td>");
                        data.Append($"<td>{resultData.Title}</td>");
                        data.Append($"<td>{resultData.BranchName}</td>");
                        data.Append($"<td>{FunctionBase.FormatDecimal(resultData.Point)}</td>");
                        #region Report Number
                        string reportName;
                        switch (resultData.ReportType)
                        {
                            case ReportTypeEnum.Week:
                                reportName = $"Tuần {resultData.ReportNum.PadLeft(2, '0')} ";
                                break;
                            case ReportTypeEnum.Month:
                                reportName = $"Tháng {resultData.ReportNum.PadLeft(2, '0')} ";
                                break;
                            case ReportTypeEnum.Year:
                                reportName = string.Empty;
                                break;
                            default:
                                reportName = string.Empty;
                                break;
                        }
                        data.Append($"<td>{reportName}Năm {resultData.ReportYear}</td>");
                        #endregion
                        data.Append("</tr>");

                        sbResult.Append(data);
                    }
                    
                    string message = "<div style=\"width: 800px\">" + string.Format(HtmlStringSearch, sbResult) + "</div>";
                    ShowAlertDialog(message.Replace(Environment.NewLine, string.Empty), $"Kết quả tìm kiếm của {staffID}");
                }
            }
            catch (Exception exception)
            {
                ShowMessage(exception.Message, ModuleMessage.ModuleMessageType.RedError);
            }
	    }
    }
}