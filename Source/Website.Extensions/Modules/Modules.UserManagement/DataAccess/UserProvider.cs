using System.Collections.Generic;
using System.Data;
using Modules.UserManagement.Database;
using Modules.UserManagement.DataTransfer;
using Website.Library.DataAccess;
using Website.Library.DataTransfer;

namespace Modules.UserManagement.DataAccess
{
    public class UserProvider : DataProvider
    {
        public List<UserData> GetUserExtension()
        {
            Connector.ExecuteProcedure<UserData, List<UserData>>("UM_SP_GetUserExtension", out List<UserData> list);
            return list;
        }

        public UserData GetUserExtension(string userID)
        {
            Connector.AddParameter(UserTable.UserID, SqlDbType.Int, userID);
            Connector.ExecuteProcedure("UM_SP_GetUserExtension", out UserData user);
            return user;
        }

        public DataTable GetUserLog(string userID)
        {
            DataTable dtResult;
            Connector.AddParameter(UserTable.UserID, SqlDbType.VarChar, userID);
            Connector.ExecuteProcedure("UM_SP_GetUserLog", out dtResult);
            return dtResult;
        }

        public DataTable SearchUser(Dictionary<string, SQLParameterData> parameterdiDictionary)
        {
            foreach (KeyValuePair<string, SQLParameterData> pair in parameterdiDictionary)
            {
                Connector.AddParameter(pair.Key, pair.Value.ParameterType, pair.Value.ParameterValue);
            }
            Connector.ExecuteProcedure("UM_SP_SearchUser", out DataTable dtResult);
            return dtResult;
        }

        public DataSet LoadUser(string userID, string viewUserID)
        {
            DataSet dsResult;
            Connector.AddParameter(UserTable.UserID, SqlDbType.VarChar, userID);
            Connector.AddParameter("ViewUserID", SqlDbType.VarChar, viewUserID);
            Connector.ExecuteProcedure("UM_SP_LoadUser", out dsResult);
            return dsResult;
        }

        public int UpdateProfile(Dictionary<string, SQLParameterData> dictionary)
        {
            foreach (KeyValuePair<string, SQLParameterData> pair in dictionary)
            {
                Connector.AddParameter(pair.Key, pair.Value.ParameterType, pair.Value.ParameterValue);
            }
            Connector.ExecuteProcedure("dbo.UM_SP_UpdateProfile", out string result);
            return int.Parse(result);
        }

        public bool UpdateRole(Dictionary<string, SQLParameterData> dictionary)
        {
            foreach (KeyValuePair<string, SQLParameterData> pair in dictionary)
            {
                Connector.AddParameter(pair.Key, pair.Value.ParameterType, pair.Value.ParameterValue);
            }
            Connector.ExecuteProcedure("dbo.UM_SP_UpdateRole", out string result);
            return int.Parse(result) > 0;
        }

        public bool InsertUserLog(Dictionary<string, SQLParameterData> dictionary)
        {
            foreach (KeyValuePair<string, SQLParameterData> pair in dictionary)
            {
                Connector.AddParameter(pair.Key, pair.Value.ParameterType, pair.Value.ParameterValue);
            }
            Connector.ExecuteProcedure("dbo.UM_SP_InsertUserLog", out string result);
            return result == "1";
        }

        public bool ConfirmBranch(Dictionary<string, SQLParameterData> parameterDictionary)
        {
            foreach (KeyValuePair<string, SQLParameterData> pair in parameterDictionary)
            {
                Connector.AddParameter(pair.Key, pair.Value.ParameterType, pair.Value.ParameterValue);
            }
            Connector.ExecuteProcedure("dbo.UM_SP_ConfirmBranch", out string result);
            return result == "1";
        }

        public List<UserData> GetUsersHaveRoles(string listRoleID)
        {
            Connector.AddParameter("RoleID", SqlDbType.VarChar, listRoleID);
            Connector.ExecuteProcedure<UserData, List<UserData>>(
                "dbo.UM_SP_GetUsersHaveRoles", out List<UserData> result);
            return result;
        }
    }
}