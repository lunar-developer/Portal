using System.Collections.Generic;
using System.Data;
using Modules.Application.Database;
using Modules.Application.DataTransfer;
using Website.Library.DataAccess;

namespace Modules.Application.DataAccess
{
    public class HomeOwnershipProvider:DataProvider
    {
        private static readonly string ScriptAll = $@"Select * from dbo.{HomeOwnershipTable.TableName} with(nolock)";
        public List<HomeOwnershipData> GetList()
        {
            Connector.ExecuteSql<HomeOwnershipData, List<HomeOwnershipData>>(ScriptAll, out List<HomeOwnershipData> outList);
            return outList;
        }


        private static readonly string Script = $@"
            Select * 
            from dbo.{HomeOwnershipTable.TableName} with(nolock) 
            where {HomeOwnershipTable.HomeOwnershipCode} = @{HomeOwnershipTable.HomeOwnershipCode}";

        public HomeOwnershipData GetItem(string id)
        {
            Connector.AddParameter(HomeOwnershipTable.HomeOwnershipCode, SqlDbType.VarChar, id);
            Connector.ExecuteSql(Script, out HomeOwnershipData result);
            return result;
        }
    }
}