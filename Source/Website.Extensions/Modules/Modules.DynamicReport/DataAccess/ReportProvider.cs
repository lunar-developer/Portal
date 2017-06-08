using System.Collections.Generic;
using System.Data;
using System.Linq;
using Modules.DynamicReport.Database;
using Modules.DynamicReport.DataTransfer;
using Website.Library.DataAccess;
using Website.Library.Database;
using Website.Library.Enum;

namespace Modules.DynamicReport.DataAccess
{
    public class ReportProvider : DataProvider
    {
        public ReportProvider(string connectionName = ConnectionEnum.SiteModules)
            : base(connectionName)
        {
        }


        public List<ReportData> GetReports(int userID)
        {
            Connector.AddParameter(BaseTable.UserID, SqlDbType.Int, userID);
            Connector.ExecuteProcedure<ReportData, List<ReportData>>("dbo.DR_SP_GetReports",
                out List<ReportData> result);
            return result;
        }

        public List<ReportFieldData> GetReportSetting(string reportID)
        {
            Connector.AddParameter(ReportTable.ReportID, SqlDbType.Int, reportID);
            Connector.ExecuteProcedure<ReportFieldData, List<ReportFieldData>>("dbo.DR_SP_GetReportSetting",
                out List<ReportFieldData> result);
            return result;
        }

        public DataTable LoadOptionData(string sql)
        {
            Connector.ExecuteSql(sql, out DataTable result);
            return result;
        }


        private const string ScriptGetReportData = "execute {0}.{1}.{2} {3}";

        public DataTable GetReportData(string databaseName, string schemaName, string procedueName,
            Dictionary<string, string> parameterDictionary)
        {
            List<string> listParameters =
                parameterDictionary.Select(pair => $" @{pair.Key} = '{pair.Value}'").ToList();
            string sql = string.Format(ScriptGetReportData, databaseName, schemaName, procedueName,
                string.Join(",", listParameters));
            Connector.ExecuteSql(sql, out DataTable result);
            return result;
        }
    }
}