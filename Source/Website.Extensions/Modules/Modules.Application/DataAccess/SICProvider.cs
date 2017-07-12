using System.Collections.Generic;
using System.Data;
using Modules.Application.Database;
using Modules.Application.DataTransfer;
using Website.Library.DataAccess;

namespace Modules.Application.DataAccess
{
    public class SICProvider : DataProvider
    {
        private static readonly string ScriptAll =
            $@"Select * from dbo.{SICTable.TableName} with(nolock) order by SICID";
        public List<SICData> GetList()
        {
            Connector.ExecuteSql<SICData, List<SICData>>(ScriptAll, out List<SICData> outList);
            return outList;
        }


        private static readonly string Script = $@"
            Select * 
            from dbo.{SICTable.TableName} with(nolock) 
            where {SICTable.SICCode} = @{SICTable.SICCode}";
        public SICData GetItem(string code)
        {
            Connector.AddParameter(SICTable.SICCode, SqlDbType.VarChar, code);
            Connector.ExecuteSql(Script, out SICData result);
            return result;
        }
    }
}