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
            List<UserData> list;
            Connector.ExecuteProcedure<UserData, List<UserData>>("UM_GetUserExtension", out list);
            return list;
        }

        public UserData GetUserExtension(string userID)
        {
            UserData user;
            Connector.AddParameter(UserTable.UserID, SqlDbType.Int, userID);
            Connector.ExecuteProcedure("UM_GetUserExtension", out user);
            return user;
        }

        public DataTable GetUserLog(string userID)
        {
            DataTable dtResult;
            Connector.AddParameter(UserTable.UserID, SqlDbType.VarChar, userID);
            Connector.ExecuteProcedure("UM_GetUserLog", out dtResult);
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
            Connector.ExecuteProcedure("UM_LoadUser", out dsResult);
            return dsResult;
        }

        public int UpdateProfile(Dictionary<string, string> dictionary, out string message)
        {
            DataTable dtResult;
            foreach (KeyValuePair<string, string> pair in dictionary)
            {
                Connector.AddParameter(pair.Key, SqlDbType.NVarChar, pair.Value);
            }
            Connector.ExecuteProcedure("dbo.UM_UpdateProfile", out dtResult);

            message = dtResult.Rows[0][1].ToString();
            return int.Parse(dtResult.Rows[0][0].ToString());
        }

        public bool UpdateRole(Dictionary<string, string> dictionary, out string message)
        {
            DataTable dtResult;
            foreach (KeyValuePair<string, string> pair in dictionary)
            {
                Connector.AddParameter(pair.Key, SqlDbType.NVarChar, pair.Value);
            }
            Connector.ExecuteProcedure("dbo.UM_UpdateRole", out dtResult);

            message = dtResult.Rows[0][1].ToString();
            return dtResult.Rows[0][0].ToString() != "0";
        }

        public bool InsertUserLog(Dictionary<string, string> dictionary)
        {
            string result;
            foreach (KeyValuePair<string, string> pair in dictionary)
            {
                Connector.AddParameter(pair.Key, SqlDbType.NVarChar, pair.Value);
            }
            Connector.ExecuteProcedure("dbo.UM_InsertUserLog", out result);
            return result == "1";
        }
    }
}