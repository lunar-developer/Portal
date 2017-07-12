using System.Collections.Generic;
using System.Data;
using Modules.Application.Database;
using Modules.Application.DataTransfer;
using Website.Library.DataAccess;

namespace Modules.Application.DataAccess
{
    public class StateProvider : DataProvider
    {
        private static readonly string ScriptGetAllState =
            $"select * from dbo.{StateTable.TableName} with(nolock) order by {StateTable.StateName}";

        public List<StateData> GetAllState()
        {
            Connector.ExecuteSql<StateData, List<StateData>>(ScriptGetAllState, out List<StateData> result);
            return result;
        }


        private static readonly string ScriptGetState = $@"
            select * from dbo.{StateTable.TableName} with(nolock)
            where {StateTable.StateCode} = @{StateTable.StateCode}";

        public StateData GetState(string stateCode)
        {
            Connector.AddParameter(StateTable.StateCode, SqlDbType.VarChar, stateCode);
            Connector.ExecuteSql(ScriptGetState, out StateData result);
            return result;
        }
    }
}