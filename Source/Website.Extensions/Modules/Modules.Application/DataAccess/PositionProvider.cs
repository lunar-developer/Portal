using System.Collections.Generic;
using System.Data;
using Modules.Application.Database;
using Modules.Application.DataTransfer;
using Website.Library.DataAccess;

namespace Modules.Application.DataAccess
{
    public class PositionProvider:DataProvider
    {
        private static readonly string ScriptAll = $@"
            Select * from dbo.{PositionTable.TableName} with(nolock)";
        public List<PositionData> GetList()
        {
            Connector.ExecuteSql<PositionData, List<PositionData>>(ScriptAll, out List<PositionData> outList);
            return outList;
        }


        private static readonly string Script = $@"
            Select * 
            from dbo.{PositionTable.TableName} with(nolock) 
            where {PositionTable.PositionCode} = @{PositionTable.PositionCode}";
        public PositionData GetItem(string code)
        {
            Connector.AddParameter(PositionTable.PositionCode, SqlDbType.VarChar, code);
            Connector.ExecuteSql(Script, out PositionData result);
            return result;
        }
    }
}
