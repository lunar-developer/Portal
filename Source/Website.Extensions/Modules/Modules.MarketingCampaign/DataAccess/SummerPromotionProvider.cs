using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;
using Modules.MarketingCampaign.Database;
using Modules.MarketingCampaign.DataTransfer;
using Modules.MarketingCampaign.Enum;
using Website.Library.DataAccess;
using Website.Library.Enum;

namespace Modules.MarketingCampaign.DataAccess
{
    public class SummerPromotionProvider:DataProvider
    {
        private static readonly string ScriptInsert = $@"
            insert into dbo.{SummerPromotionTable.TableShadowName}(
                {SummerPromotionTable.ReportType},
                {SummerPromotionTable.ReportYear},
                {SummerPromotionTable.ReportNum},
                {SummerPromotionTable.BranchCode},
                {SummerPromotionTable.BranchName},
                {SummerPromotionTable.Rank},
                {SummerPromotionTable.BalanceTarget},
                {SummerPromotionTable.BalanceReality},
                {SummerPromotionTable.Point},
                {SummerPromotionTable.Complete},
                {SummerPromotionTable.ReportDate},
                {SummerPromotionTable.CreateDateTime})";


        private static bool ValidateField(SummerPromotionData result,
            out int rank,
            out decimal balanceTarget,
            out decimal balanceReality,
            out double point,
            out decimal complete,
            out string reportType,
            out int reportNum,
            out int reportYear,
            out string message)
        {
            rank = -1;
            balanceTarget = -1;
            balanceReality = -1;
            point = -1;
            complete = -1;
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
                if (!field.Name.Equals(SummerPromotionTable.ReportDate) &&
                    !field.Name.Equals(SummerPromotionTable.CreateDateTime) &&
                    !field.Name.Equals(SummerPromotionTable.ReportDate) &&
                    string.IsNullOrWhiteSpace(value))
                {
                    message = $"'{field.Name}' Không được phép bỏ trống";
                    return false;
                }
            }

            if (!int.TryParse(result.Rank, out rank))
            {
                message = "'Rank (Hạng)' Giá trị không đúng; giá trị phải là kiểu số";
                return false;
            }

            if (!decimal.TryParse(result.BalanceTarget, out balanceTarget))
            {
                message = "'Balance (Số dư huy động theo kế hoạch)' Giá trị không đúng; giá trị phải là kiểu số";
                return false;
            }
            if (!decimal.TryParse(result.BalanceReality, out balanceReality))
            {
                message = "'Balance (Số dư huy động thực tế)' Giá trị không đúng; giá trị phải là kiểu số";
                return false;
            }
            if (!decimal.TryParse(result.Complete, out complete))
            {
                message = "'Complete (Tiến độ thời gian)' Giá trị không đúng; giá trị phải là kiểu số";
                return false;
            }
            if (!double.TryParse(result.Point, out point))
            {
                message = "'Point (Điểm)' Giá trị không đúng; giá trị phải là kiểu số";
                return false;
            }
            

            if (reportType == null)
            {
                message = "'ReportType (Loại báo cáo: 'W': Tuần; 'M': Tháng; 'Y': Year; K: Kỳ) Không đúng định dạng";
                return false;
            }


            if (!(reportType.Equals(ReportTypeEnum.Week) ||
                reportType.Equals(ReportTypeEnum.Month) ||
                reportType.Equals(ReportTypeEnum.Year) ||
                reportType.Equals(ReportTypeEnum.Session)))
            {
                message = "'ReportType' Giá trị không đúng; Giá trị phải theo định dạng như sau: 'W': Tuần; 'M': Tháng; 'Y': Year; K: Kỳ";
                return false;
            }

            if (!int.TryParse(result.ReportNum, out reportNum))
            {
                message = "'ReportNum (Mã báo cáo)' Giá trị không đúng; giá trị phải là kiểu số";
                return false;
            }

            if (!int.TryParse(result.ReportYear, out reportYear))
            {
                message = "'ReportYear (Năm báo cáo)' Giá trị không đúng; giá trị phải là kiểu số";
                return false;
            }


            message = string.Empty;
            return true;
        }

        private bool ExecuteResultImport(string script, string reportType, int reportYear, int reportNum, out string message)
        {
            DataTable dtResult;
            Connector.ExecuteSql(script);
            Connector.AddParameter(SummerPromotionTable.ReportType, SqlDbType.VarChar, reportType);
            Connector.AddParameter(SummerPromotionTable.ReportYear, SqlDbType.Int, reportYear);
            Connector.AddParameter(SummerPromotionTable.ReportNum, SqlDbType.Int, reportNum);
            Connector.ExecuteProcedure("dbo.MC_ImportSummerPromotionResult", out dtResult);

            message = dtResult.Rows[0][1].ToString();
            return dtResult.Rows[0][0].ToString() == "1";
        }

        public bool InsertResult(List<SummerPromotionData> listResult, out string message)
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

            foreach (SummerPromotionData result in listResult)
            {
                if (ValidateField(result,
                    out int rank,
                    out decimal balanceTarget,
                    out decimal balanceReality,
                    out double point,
                    out decimal complete,
                    out reportType,
                    out reportNum,
                    out reportYear,
                    out message))
                {
                    listSQL.Add($@"('{reportType}','{reportYear}','{reportNum}',
                     '{result?.BranchCode}', N'{result?.BranchName}',
                     '{rank}', '{balanceTarget}','{balanceReality}', '{point}','{complete}', 
                     '{reportDate}', '{createDateTime}')");

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

        public DataTable LoadResult(int top, string reportType)
        {
            Connector.AddParameter("TopRanking", SqlDbType.Int, top);
            Connector.AddParameter(ResultTable.ReportType, SqlDbType.VarChar, reportType);
            Connector.ExecuteProcedure("dbo.MC_LoadSummerPromotionResult", out DataTable dtResult);
            return dtResult;
        }

        public List<SummerPromotionData> SearchResult(string branchCode)
        {
            List<SummerPromotionData> dtResult;
            Connector.AddParameter(SummerPromotionTable.BranchCode, SqlDbType.VarChar, branchCode);
            Connector.ExecuteProcedure<SummerPromotionData, List<SummerPromotionData>>("dbo.MC_SearchSummerPromotionResult", out dtResult);
            return dtResult;
        }
    }
}
