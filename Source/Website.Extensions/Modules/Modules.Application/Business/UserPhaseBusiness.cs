using System.Collections.Generic;
using System.Data;
using Modules.Application.DataAccess;
using Website.Library.DataTransfer;

namespace Modules.Application.Business
{
    public static class UserPhaseBusiness
    {
        public static DataTable GetAllUserData()
        {
            return new UserPhaseProvider().GetAllUserData();
        }

        public static DataTable GetAllUserData(string phaseID)
        {
            return new UserPhaseProvider().GetAllUserData(phaseID);
        }

        public static DataTable GetUserData(string phaseID, string userID)
        {
            return new UserPhaseProvider().GetUserData(phaseID, userID);
        }

        public static int InsertUserPhase(Dictionary<string, SQLParameterData> dataDictionary)
        {
            return new UserPhaseProvider().InsertUserPhase(dataDictionary);
        }

        public static int UpdateUserPhase(Dictionary<string, SQLParameterData> dataDictionary)
        {
            return new UserPhaseProvider().UpdateUserPhase(dataDictionary);
        }

        public static string GetCacheKey(string phaseID, string userID)
        {
            return $"{phaseID}-{userID}";
        }
    }
}