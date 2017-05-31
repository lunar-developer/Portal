using Modules.MarketingCampaign.Database;
using Modules.MarketingCampaign.DataTransfer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Modules.MarketingCampaign.Enum;
using Website.Library.DataAccess;
using Website.Library.Enum;

namespace Modules.MarketingCampaign.DataAccess
{
    public class ResultProvider : DataProvider
    {
        public DataTable LoadResult(int topRanking, string reportType)
        {
            DataTable dtResult;
            Connector.AddParameter("TopRanking", SqlDbType.Int, topRanking);
            Connector.AddParameter(ResultTable.ReportType, SqlDbType.VarChar, reportType);
            Connector.ExecuteProcedure("dbo.MC_LoadResult", out dtResult);
            return dtResult;
        }

        public List<ResultData> SearchResult(string staffID)
        {
            List<ResultData> dtResult;
            Connector.AddParameter(ResultTable.StaffID, SqlDbType.VarChar, staffID);
            Connector.ExecuteProcedure<ResultData, List<ResultData>>("dbo.MC_SearchResult", out dtResult);
            return dtResult;
        }

        private static readonly string ScriptInsert = $@"
            insert into dbo.{ResultTable.TableShadowName}(
                {ResultTable.StaffID},
                {ResultTable.ReportType},
                {ResultTable.ReportYear},
                {ResultTable.ReportNum},
                {ResultTable.FullName},
                {ResultTable.Title},
                {ResultTable.BranchName},
                {ResultTable.UserGroup},
                {ResultTable.Point},
                {ResultTable.ReportDate},
                {ResultTable.CreateDateTime})";
        public DataTable InsertResult(List<ResultData> listResult)
        {
            // Build insert script
            StringBuilder script = new StringBuilder();
            List<string> listSQL = new List<string>();
            string reportDate = DateTime.Now.ToString(PatternEnum.Date);
            string createDateTime = DateTime.Now.ToString(PatternEnum.DateTime);
            string reportType = "-1";
            int reportNum = -1;
            int reportYear = -1;
            foreach (ResultData result in listResult)
            {
                reportType = result.ReportType;
                bool isOk = reportType.Equals(ReportTypeEnum.Week) ||
                    reportType.Equals(ReportTypeEnum.Month) ||
                    reportType.Equals(ReportTypeEnum.Year);
                                
                isOk = isOk && int.TryParse(result.ReportNum, out reportNum);
                isOk = isOk && int.TryParse(result.ReportYear, out reportYear);
                if (!isOk)
                {
                    DataTable dtErr = new DataTable();
                    DataRow drError = dtErr.NewRow();
                    drError[""] = "0";
                    drError["message"] = "Dữ liệu file không đúng định dạng";
                    dtErr.Rows.InsertAt(drError, 0);
                    dtErr.AcceptChanges();
                    return dtErr;
                }
                listSQL.Add($@"('{result.StaffID}','{reportType}','{reportYear}','{reportNum}', N'{result.FullName}', N'{result.Title}', N'{result.BranchName}', 
                    {result.UserGroup}, {result.Point}, {reportDate}, {createDateTime})");

                if (listSQL.Count < 1000)
                {
                    continue;
                }
                script.Append($"{ScriptInsert} values {string.Join(",", listSQL.ToArray())};");
                listSQL = new List<string>();
                
            }
            script.Append($"{ScriptInsert} values {string.Join(",", listSQL.ToArray())};");

            DataTable dtResult;
            Connector.ExecuteSql(script.ToString());
            Connector.AddParameter(ResultTable.ReportType, SqlDbType.VarChar, reportType);
            Connector.AddParameter(ResultTable.ReportYear, SqlDbType.Int, reportYear);
            Connector.AddParameter(ResultTable.ReportNum,SqlDbType.Int, reportNum);
            Connector.ExecuteProcedure("dbo.MC_InsertResult", out dtResult);
            return dtResult;
        }
    }
}
