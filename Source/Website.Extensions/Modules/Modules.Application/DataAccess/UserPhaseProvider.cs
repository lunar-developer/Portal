using System.Data;
using Modules.Application.Database;
using Website.Library.DataAccess;

namespace Modules.Application.DataAccess
{
    public class UserPhaseProvider : DataProvider
    {
        private static readonly string ScriptGetAllUserData =
            $"select * from dbo.{UserPhaseTable.TableName} with(nolock) where {UserPhaseTable.IsDisable} = 0";

        public DataTable GetAllUserData()
        {
            Connector.ExecuteSql(ScriptGetAllUserData, out DataTable result);
            return result;
        }


        private static readonly string ScriptGetUserData = $@"
            select * from dbo.{UserPhaseTable.TableName} with(nolock)
            where
                {UserPhaseTable.IsDisable} = 0
            and {UserPhaseTable.UserID} = @{UserPhaseTable.UserID}";

        public DataTable GetUserData(string userID)
        {
            Connector.AddParameter(UserPhaseTable.UserID, SqlDbType.Int, userID);
            Connector.ExecuteSql(ScriptGetUserData, out DataTable result);
            return result;
        }
    }
}