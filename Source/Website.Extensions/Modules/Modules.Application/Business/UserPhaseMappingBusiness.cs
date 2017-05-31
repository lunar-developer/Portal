using System.Collections.Generic;
using System.Data;
using Modules.Application.DataAccess;
using Modules.Application.DataTransfer;

namespace Modules.Application.Business
{
    public static class UserPhaseMappingBusiness
    {
        public static List<UserPhaseMappingData> GetListUserPhaseMapping()
        {
            return new UserPhaseMappingProvider().GetListUserPhaseMapping();
        }
        public static UserPhaseMappingData GetUserPhaseMapping(string userPhaseMappingID)
        {
            return new UserPhaseMappingProvider().GetUserPhaseMapping(userPhaseMappingID);
        }

        public static DataTable GetUserPhaseMappingData(Dictionary<string, string> dictionary)
        {
            return new UserPhaseMappingProvider().GetUserPhaseMappingData(dictionary);
        }
        
        public static bool DeleteUserPhaseMappingData(Dictionary<string, string> dictionary, out string message)
        {
            DataTable dtResult = new UserPhaseMappingProvider().DeleteUserPhaseMappingData(dictionary);
            message = dtResult.Rows[0][1].ToString();
            return dtResult.Rows[0][0].ToString() == "1";
        }
        public static bool InsertUserPhaseMappingData(Dictionary<string, string> dictionary,out int id, out string message)
        {
            DataTable dtResult = new UserPhaseMappingProvider().InsertUserPhaseMappingData(dictionary);
            message = dtResult.Rows[0][1].ToString();
            id = dtResult.Rows.Count == 0 ? -1 : int.Parse(dtResult.Rows[0][0].ToString());
            return id > 0;
        }
    }
}
