using System.Data;
using Modules.Application.Database;
using Website.Library.DataAccess;
using Website.Library.Database;

namespace Modules.Application.DataAccess
{
    public class ApplicationLogProvider : DataProvider
    {
        public DataTable GetVersion(string applicationID, string baseVersion)
        {
            string sql;
            if (string.IsNullOrEmpty(baseVersion))
            {
                sql = $@"select  {ApplicationLogTable.ApplicationLogID},
                                 {BaseTable.ModifyDateTime}
                        from dbo.{ApplicationLogTable.TableName}
                        where    {ApplicationTable.ApplicationID} = '{applicationID}'
                        and      {ApplicationLogTable.ApplicationLogID} > '1'";
            }
            else
            {
                if ("0".Equals(baseVersion))
                {
                    return null;
                }
                sql = $@"select  {ApplicationLogTable.ApplicationLogID},
                                 {BaseTable.ModifyDateTime}
                        from dbo.{ApplicationLogTable.TableName}
                        where    {ApplicationTable.ApplicationID} = '{applicationID}'
                        and      {ApplicationLogTable.ApplicationLogID} < '{baseVersion}'";
            }
            Connector.ExecuteSql(sql, out DataTable dataTable);
            return dataTable;
        }

        public DataTable GetChangeOfTwoVersions(string applicationID, string lhs, string rhs, string fieldName = null)
        {
            if (string.IsNullOrEmpty(applicationID) || string.IsNullOrEmpty(lhs))
            {
                return null;
            }
            string sql = $@"select   {ApplicationLogTable.ApplicationLogID}, 
                                     {ApplicationFieldTable.FieldName},
                                     {ApplicationFieldTable.FieldValue}
                            from dbo.{ApplicationLogDetailTable.TableName}
                            where    {ApplicationTable.ApplicationID} = '{applicationID}'";
            if (!string.IsNullOrEmpty(rhs))
            {
                sql += $@" 
                        and ({ApplicationLogTable.ApplicationLogID} = '{lhs}'
                        or   {ApplicationLogTable.ApplicationLogID} = '{rhs}')";
            }
            else
            {
                sql += $@" and  {ApplicationLogTable.ApplicationLogID} = '{lhs}'";
            }

            if (!string.IsNullOrEmpty(fieldName))
            {
                sql += $@" and  {ApplicationFieldTable.FieldName} = '{fieldName}'";
            }

            Connector.ExecuteSql(sql, out DataTable dataTable);
            return dataTable;
        }
    }
}
