using System.Collections.Generic;
using System.Data;
using Modules.Application.Database;
using Modules.Application.DataTransfer;
using Website.Library.DataAccess;

namespace Modules.Application.DataAccess
{
    public class UserPhaseMappingProvider: DataProvider
    {
        public List<UserPhaseMappingData> GetListUserPhaseMapping()
        {
            List<UserPhaseMappingData> outList;
            Connector.ExecuteProcedure<UserPhaseMappingData, List<UserPhaseMappingData>>(UserPhaseMappingTable.SelectStoredProcedure, out outList);
            return outList;
        }
        public UserPhaseMappingData GetUserPhaseMapping(string userPhaseMappingID)
        {
            UserPhaseMappingData outData;
            Connector.AddParameter(UserPhaseMappingTable.UserPhaseMappingID,SqlDbType.Int, userPhaseMappingID);
            Connector.ExecuteProcedure(UserPhaseMappingTable.SelectStoredProcedure, out outData);
            return outData;
        }

        public DataTable GetUserPhaseMappingData(Dictionary<string, string> dictionary)
        {
            DataTable outData;
            if (dictionary != null && dictionary.Count > 0)
            {
                foreach (KeyValuePair<string, string> pair in dictionary)
                {
                    Connector.AddParameter(pair.Key, SqlDbType.VarChar, pair.Value);
                }
            }
            Connector.ExecuteProcedure(UserPhaseMappingTable.SelectStoredProcedure, out outData);
            return outData;
        }
        
        public DataTable DeleteUserPhaseMappingData(Dictionary<string, string> dictionary)
        {
            DataTable dtResult;
            foreach (KeyValuePair<string, string> pair in dictionary)
            {
                Connector.AddParameter(pair.Key, SqlDbType.VarChar, pair.Value);
            }
            Connector.ExecuteProcedure(UserPhaseMappingTable.DeleteStoredProcedured, out dtResult);
            return dtResult;
        }
        public DataTable InsertUserPhaseMappingData(Dictionary<string, string> dictionary)
        {
            DataTable dtResult;
            foreach (KeyValuePair<string, string> pair in dictionary)
            {
                Connector.AddParameter(pair.Key, SqlDbType.VarChar, pair.Value);
            }
            Connector.ExecuteProcedure(UserPhaseMappingTable.InsertStoredProcedured, out dtResult);
            return dtResult;
        }
    }
}
