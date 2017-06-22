using System.Collections.Generic;
using System.Data;
using Modules.UserManagement.Database;
using Modules.UserManagement.DataTransfer;
using Website.Library.DataAccess;
using Website.Library.DataTransfer;

namespace Modules.UserManagement.DataAccess
{
    public class UserRequestProvider : DataProvider
    {
        public int CreateRequest(Dictionary<string, SQLParameterData> parameterDictionary)
        {
            foreach (KeyValuePair<string, SQLParameterData> pair in parameterDictionary)
            {
                Connector.AddParameter(pair.Key, pair.Value.ParameterType, pair.Value.ParameterValue);
            }
            Connector.ExecuteProcedure("dbo.UM_SP_InsertRequest", out string result);
            return int.Parse(result);
        }

        public int UpdateRequest(Dictionary<string, SQLParameterData> parameterDictionary)
        {
            foreach (KeyValuePair<string, SQLParameterData> pair in parameterDictionary)
            {
                Connector.AddParameter(pair.Key, pair.Value.ParameterType, pair.Value.ParameterValue);
            }
            Connector.ExecuteProcedure("dbo.UM_SP_UpdateRequest", out string result);
            return int.Parse(result);
        }

        public int ProcessRequest(Dictionary<string, SQLParameterData> parameterDictionary)
        {
            foreach (KeyValuePair<string, SQLParameterData> pair in parameterDictionary)
            {
                Connector.AddParameter(pair.Key, pair.Value.ParameterType, pair.Value.ParameterValue);
            }
            Connector.ExecuteProcedure("dbo.UM_SP_ProcessRequest", out string result);
            return int.Parse(result);
        }

        public UserRequestData LoadRequest(string requestID)
        {
            Connector.AddParameter(UserRequestTable.UserRequestID, SqlDbType.Int, requestID);
            Connector.ExecuteProcedure("dbo.UM_SP_LoadRequest", out UserRequestData result);
            return result;
        }

        public DataTable SearchRequest(Dictionary<string, SQLParameterData> parameterDictionary)
        {
            foreach (KeyValuePair<string, SQLParameterData> pair in parameterDictionary)
            {
                Connector.AddParameter(pair.Key, pair.Value.ParameterType, pair.Value.ParameterValue);
            }
            Connector.ExecuteProcedure("dbo.UM_SP_SearchRequest", out DataTable result);
            return result;
        }
    }
}