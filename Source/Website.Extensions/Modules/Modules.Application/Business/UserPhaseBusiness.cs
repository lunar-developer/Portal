using System.Data;
using Modules.Application.DataAccess;

namespace Modules.Application.Business
{
    public static class UserPhaseBusiness
    {
        public static DataTable GetAllUserData()
        {
            return new UserPhaseProvider().GetAllUserData();
        }

        public static DataTable GetUserData(string userID)
        {
            return new UserPhaseProvider().GetUserData(userID);
        }
    }
}