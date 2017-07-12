using System.Collections.Generic;
using System.Data;
using Modules.Application.Database;
using Modules.Application.DataTransfer;
using Website.Library.DataAccess;

namespace Modules.Application.DataAccess
{
    public class MaritalStatusProvider:DataProvider
    {
        private static readonly string ScriptAll = $@"
            Select * from dbo.{MaritalStatusTable.TableName} with(nolock) order by MaritalStatusID";
        public List<MaritalStatusData> GetList()
        {
            Connector.ExecuteSql<MaritalStatusData, List<MaritalStatusData>>(ScriptAll, out List<MaritalStatusData> outList);
            return outList;
        }


        private static readonly string Script = $@"
            Select * 
            from dbo.{MaritalStatusTable.TableName} with(nolock) 
            where {MaritalStatusTable.MaritalStatusCode} = @{MaritalStatusTable.MaritalStatusCode}";
        public MaritalStatusData GetItem(string id)
        {
            Connector.AddParameter(MaritalStatusTable.MaritalStatusCode, SqlDbType.VarChar, id);
            Connector.ExecuteSql(Script, out MaritalStatusData result);
            return result;
        }
    }
}