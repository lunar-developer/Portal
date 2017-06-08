using System;
using System.Collections.Generic;
using System.Data;
using Modules.DynamicReport.DataAccess;
using Modules.DynamicReport.DataTransfer;
using Website.Library.Global;

namespace Modules.DynamicReport.Business
{
    public static class ReportBusiness
    {
        public static List<ReportData> GetReports(int userID)
        {
            return new ReportProvider().GetReports(userID);
        }

        public static List<ReportFieldData> GetReportSetting(string reportID)
        {
            return new ReportProvider().GetReportSetting(reportID);

        }

        public static DataTable LoadOptionData(string sql)
        {
            try
            {
                return new ReportProvider().LoadOptionData(sql);
            }
            catch (Exception exception)
            {
                FunctionBase.LogError(exception);
                return new DataTable();
            }
        }

        public static DataTable GetReportData(string connectionName, string databaseName, string schemaName,
            string procedureName, Dictionary<string, string> parameterDictionary)
        {
            return new ReportProvider(connectionName).GetReportData(databaseName, schemaName, procedureName,
                parameterDictionary);
        }
    }
}