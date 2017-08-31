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

namespace DesktopModules.Modules.MarketingCampaign
{
    public partial class SummerPromotionResult : DesktopModuleBase
	{
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            txtBranchCode.Attributes.Add("placeholder", GetResource("lblBranchCode.Help"));
        }

        protected override void OnLoad(EventArgs e)
        {
            if (IsPostBack == false)
            {
                //BindData(ReportTypeEnum.Year);
                //BindData(ReportTypeEnum.Month);
                //BindData(ReportTypeEnum.Week);
                BindData(ReportTypeEnum.Session);
            }
        }

        #region HTML Template
        private const string HtmlStringResult = @"
            <table class=""table c-margin-t-10"">
                <colgroup>
                    <col width=""5%"" />
                    <col width=""30%"" />
                    <col width=""15%"" />
                    <col width=""20%"" />
                    <col width=""20%"" />
                    <col width=""10%"" />
                </colgroup>
                <thead>
                    <tr>
                        <th class=""text-center"" style=""vertical-align: middle"">Hạng</th>
                        <th style=""vertical-align: middle"">Trung tâm kinh doanh</th>
                        <th style=""vertical-align: middle"">Mã trung tâm kinh doanh</th>
                        <th style=""vertical-align: middle"">Chỉ tiêu huy động (đồng)</th>
                        <th style=""vertical-align: middle"">Số dư huy động thực hiện (đồng)</th>
                        <th style=""vertical-align: middle"">Tỷ lệ hoàn thành (%)</th>
                        </tr>
                </thead>
                <tbody>{0}</tbody>
            </table>";
        private const string HtmlStringSearch = @"
            <table class=""table c-margin-t-10"">
                <colgroup>
                    <col width=""5%"" />
                    <col width=""25%"" />
                    <col width=""15%"" />
                    <col width=""15%"" />
                    <col width=""15%"" />
                    <col width=""10%"" />
                    <col width=""20%"" />
                </colgroup>
                <thead>
                    <tr>
                        <th class=""text-center"" style=""vertical-align: middle"">Hạng</th>
                        <th style=""vertical-align: middle"">Trung tâm kinh doanh</th>
                        <th style=""vertical-align: middle"">Mã trung tâm kinh doanh</th>
                        <th style=""vertical-align: middle"">Chỉ tiêu huy động (đồng)</th>
                        <th style=""vertical-align: middle"">Số dư huy động thực hiện (đồng)</th>
                        <th style=""vertical-align: middle"">Tỷ lệ hoàn thành (%)</th>
                        <th style=""vertical-align: middle"">Thời gian</th>
                        </tr>
                </thead>
                <tbody>{0}</tbody>
            </table>";
        #endregion
        private void BindData(string reportType)
	    {
	        StringBuilder data = new StringBuilder();
            DataTable dtResult = SummerPromotionBusiness.LoadResult(10,reportType);

	        string year = string.Empty;
	        string reportNum = string.Empty;

	        int top = dtResult.Rows.Count;

	        foreach (DataRow row in dtResult.Rows)
	        {
                
                year = row[SummerPromotionTable.ReportYear].ToString();
	            reportNum = row[SummerPromotionTable.ReportNum].ToString();

                data.Append("<tr>");
	            data.Append($"<td class=\"text-center\">{row[SummerPromotionTable.Rank].ToString().PadLeft(3, '0')}</td>");
                data.Append($"<td>{row[SummerPromotionTable.BranchName]}</td>");
                data.Append($"<td>{row[SummerPromotionTable.BranchCode]}</td>");
	            data.Append($"<td>{Money(row[SummerPromotionTable.BalanceTarget].ToString())}</td>");
                data.Append($"<td>{Money(row[SummerPromotionTable.BalanceReality].ToString())}</td>");
                data.Append($"<td>{Percent(row[SummerPromotionTable.Complete].ToString())}</td>");
                data.Append("</tr>");
                
            }
	        #region Report Number
	        switch (reportType)
	        {
	            case ReportTypeEnum.Session:
	                dsSession.Visible = dtResult.Rows?.Count > 0;
	                //titleSession.InnerHtml = $"Danh sách Top {top} kỳ {reportNum} Năm {year}";
	                break;
                case ReportTypeEnum.Week:
	                dsWeek.Visible = dtResult.Rows?.Count > 0;
	                titleWeek.InnerHtml = $"Danh sách Top {top} tuần {reportNum} Năm {year}";
	                break;
	            case ReportTypeEnum.Month:
	                dsMonth.Visible = dtResult.Rows?.Count > 0;
	                titleMonth.InnerHtml = $"Danh sách Top {top} Tháng {reportNum} Năm {year}";
	                break;
	            case ReportTypeEnum.Year:
	                dsYear.Visible = dtResult.Rows?.Count > 0;
	                titleYear.InnerHtml = $"Danh sách Top {top} Năm {year}";
	                break;
	        }
	        #endregion
            if (reportType.Equals(ReportTypeEnum.Year))
	        {
	            DivListByYear.InnerHtml = string.Format(HtmlStringResult, data);
            }
            if (reportType.Equals(ReportTypeEnum.Month))
            {
                DivListByMonth.InnerHtml = string.Format(HtmlStringResult, data);
            }
            if (reportType.Equals(ReportTypeEnum.Week))
            {
                DivListByWeek.InnerHtml = string.Format(HtmlStringResult, data);
            }
	        if (reportType.Equals(ReportTypeEnum.Session))
	        {
	            DivListBySession.InnerHtml = string.Format(HtmlStringResult, data);
	        }
        }

        protected void Search(object sender, EventArgs e)
	    {
            try
            {
                string branchCode = txtBranchCode.Text.Trim();
                List<SummerPromotionData> resultList = SummerPromotionBusiness.SearchResult(branchCode);
                if (resultList == null || resultList.Count == 0)
                {
                    ShowMessage("Không tìm thấy kết quả!");
                }
                else
                {
                    StringBuilder sbResult = new StringBuilder();
                    foreach (SummerPromotionData resultData in resultList)
                    {
                        StringBuilder data = new StringBuilder();
                        data.Append("<tr>");
                        data.Append($"<td class=\"text-center\">{resultData.Rank.PadLeft(3, '0')}</td>");
                        data.Append($"<td>{resultData.BranchName}</td>");
                        data.Append($"<td>{branchCode}</td>");
                        data.Append($"<td>{Money(resultData.BalanceTarget)}</td>");
                        data.Append($"<td>{Money(resultData.BalanceReality)}</td>");
                        data.Append($"<td>{Percent(resultData.Complete)}</td>");
                        
                        #region Report Number
                        string reportName;
                        switch (resultData.ReportType)
                        {
                            case ReportTypeEnum.Week:
                                reportName = !string.IsNullOrWhiteSpace(resultData.ReportNum) ?
                                    $"Tuần {resultData.ReportNum.PadLeft(2, '0')} " : string.Empty;
                                break;
                            case ReportTypeEnum.Month:
                                reportName = !string.IsNullOrWhiteSpace(resultData.ReportNum) ?
                                    $"Tháng {resultData.ReportNum.PadLeft(2, '0')} " : string.Empty;
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
                    ShowAlertDialog(message.Replace(Environment.NewLine, string.Empty), $"Kết quả tìm kiếm của {branchCode}");
                }
            }
            catch (Exception exception)
            {
                ShowMessage(exception.Message, ModuleMessage.ModuleMessageType.RedError);
            }
	    }

	    private string Money(string amt)
	    {
	        if (string.IsNullOrWhiteSpace(amt)) return "0";
	        decimal amount;
            bool isCheck = decimal.TryParse(amt, out amount);
	        if (!isCheck) return amt;
	        return amount.ToString("#,##0.00"); ;
        }

	    private string Percent(string p)
	    {
	        if (string.IsNullOrWhiteSpace(p)) return "0";
	        decimal percent;
	        bool isCheck = decimal.TryParse(p, out percent);
	        if (!isCheck) return p;
	        return percent.ToString("##0.00"); 
        }
    }
}