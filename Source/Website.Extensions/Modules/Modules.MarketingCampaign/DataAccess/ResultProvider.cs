using Modules.MarketingCampaign.Database;
using Modules.MarketingCampaign.DataTransfer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
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

        private bool ValidateField(ResultData result,
            out int userGroup,
            out double point,
            out string reportType,
            out int reportNum,
            out int reportYear, 
            out string message)
        {
            userGroup = -1;
            point = -1;
            reportNum = -1;
            reportYear = -1;
            reportType = result?.ReportType;

            if (result == null)
            {
                message = "Dữ liệu không hợp lệ";
                return false;
            }
            foreach (FieldInfo field in result.GetType().GetFields())
            {
                string value = (string)field.GetValue(result);
                if (!field.Name.Equals(ResultTable.ReportDate) &&
                    !field.Name.Equals(ResultTable.Ranking) &&
                    string.IsNullOrWhiteSpace(value))
                {
                    message = $"'{field.Name}' Không được phép bỏ trống";
                    return false;
                }
            }

            if (!double.TryParse(result.Point, out point))
            {
                message = "'Point' Giá trị không đúng; giá trị phải là kiểu số";
                return false;
            }
            if (!int.TryParse(result.UserGroup, out userGroup))
            {
                message = "'UserGroup' Giá trị không đúng; giá trị phải là kiểu số";
                return false;
            }

            if (reportType == null)
            {
                message = "'ReportType' Không đúng định dạng";
                return false;
            }


            if (!(reportType.Equals(ReportTypeEnum.Week) ||
                reportType.Equals(ReportTypeEnum.Month) ||
                reportType.Equals(ReportTypeEnum.Year)))
            {
                message = "'ReportType' Giá trị không đúng; Giá trị phải theo định dạng như sau: 'W': Tuần; 'M': Tháng; 'Y': Year";
                return false;
            }

            if (!int.TryParse(result.ReportNum, out reportNum))
            {
                message = "'ReportNum' Giá trị không đúng; giá trị phải là kiểu số";
                return false;
            }

            if (!int.TryParse(result.ReportYear, out reportYear))
            {
                message = "'ReportYear' Giá trị không đúng; giá trị phải là kiểu số";
                return false;
            }


            message = string.Empty;
            return true;
        }
        public bool InsertResult(List<ResultData> listResult,out string message)
        {
            // Build insert script
            StringBuilder script = new StringBuilder();
            List<string> listSQL = new List<string>();
            string reportDate = DateTime.Now.ToString(PatternEnum.Date);
            string createDateTime = DateTime.Now.ToString(PatternEnum.DateTime);

            string reportType = "-1";
            int reportNum = -1;
            int reportYear = -1;

            int rownum = 1;

            foreach (ResultData result in listResult)
            {
                if (ValidateField(result,
                    out int userGroup,
                    out double point,
                    out reportType,
                    out reportNum,
                    out reportYear,
                    out message))
                {
                    listSQL.Add($@"('{result?.StaffID}','{reportType}','{reportYear}',
                    '{reportNum}', N'{result?.FullName}', N'{result?.Title}',
                    N'{result?.BranchName}', {userGroup}, {point}, 
                    {reportDate}, {createDateTime})");

                    if (listSQL.Count < 1000)
                    {
                        continue;
                    }
                    script.Append($"{ScriptInsert} values {string.Join(",", listSQL.ToArray())};");
                    listSQL = new List<string>();
                    
                }
                else
                {
                    message = $"Dòng dữ liệu: {rownum} | {message}";
                    return false;
                }
                rownum++;
            }
            script.Append($"{ScriptInsert} values {string.Join(",", listSQL.ToArray())};");

            return ExecuteResultImport(script.ToString(), reportType, reportYear, reportNum, out message);
        }

        private bool ExecuteResultImport(string script, string reportType, int reportYear, int reportNum, out string message)
        {
            DataTable dtResult;
            Connector.ExecuteSql(script);
            Connector.AddParameter(ResultTable.ReportType, SqlDbType.VarChar, reportType);
            Connector.AddParameter(ResultTable.ReportYear, SqlDbType.Int, reportYear);
            Connector.AddParameter(ResultTable.ReportNum, SqlDbType.Int, reportNum);
            Connector.ExecuteProcedure("dbo.MC_InsertResult", out dtResult);

            message = dtResult.Rows[0][1].ToString();
            return dtResult.Rows[0][0].ToString() == "1";
        }
    }
}
