using System.Collections.Generic;
using System.Data;
using Modules.Application.Database;
using Website.Library.DataAccess;
using Website.Library.DataTransfer;

namespace Modules.Application.DataAccess
{
    public class UserPhaseProvider : DataProvider
    {
        private static readonly string ScriptGetAllUserData =
            $"select * from dbo.{UserPhaseTable.TableName} with(nolock)";

        public DataTable GetAllUserData()
        {
            Connector.ExecuteSql(ScriptGetAllUserData, out DataTable result);
            return result;
        }

        private static readonly string ScriptGetAllUserDataInPhase = $@"
            select * from dbo.{UserPhaseTable.TableName} with(nolock)
            where {UserPhaseTable.PhaseID} = @{UserPhaseTable.PhaseID}";
        public DataTable GetAllUserData(string phaseID)
        {
            Connector.AddParameter(UserPhaseTable.PhaseID, SqlDbType.Int, phaseID);
            Connector.ExecuteSql(ScriptGetAllUserDataInPhase, out DataTable result);
            return result;
        }

        private static readonly string ScriptGetUserData = $@"
            select * from dbo.{UserPhaseTable.TableName} with(nolock)
            where
                {UserPhaseTable.PhaseID} = @{UserPhaseTable.PhaseID}
            and {UserPhaseTable.UserID} = @{UserPhaseTable.UserID}";

        public DataTable GetUserData(string phaseID, string userID)
        {
            Connector.AddParameter(UserPhaseTable.PhaseID, SqlDbType.Int, phaseID);
            Connector.AddParameter(UserPhaseTable.UserID, SqlDbType.Int, userID);
            Connector.ExecuteSql(ScriptGetUserData, out DataTable result);
            return result;
        }

        public int InsertUserPhase(Dictionary<string, SQLParameterData> dataDictionary)
        {
            foreach (KeyValuePair<string, SQLParameterData> pair in dataDictionary)
            {
                Connector.AddParameter(pair.Key, pair.Value.ParameterType, pair.Value.ParameterValue);
            }
            Connector.ExecuteProcedure("dbo.APP_SP_InsertUserPhase", out string result);
            return int.Parse(result);
        }

        public int UpdateUserPhase(Dictionary<string, SQLParameterData> dataDictionary)
        {
            foreach (KeyValuePair<string, SQLParameterData> pair in dataDictionary)
            {
                Connector.AddParameter(pair.Key, pair.Value.ParameterType, pair.Value.ParameterValue);
            }
            Connector.ExecuteProcedure("dbo.APP_SP_UpdateUserPhase", out string result);
            return int.Parse(result);
        }
    }
}