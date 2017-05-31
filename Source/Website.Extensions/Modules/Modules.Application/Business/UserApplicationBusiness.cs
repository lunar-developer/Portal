using System.Collections.Generic;
using Modules.Application.DataAccess;
using Modules.Application.DataTransfer;

namespace Modules.Application.Business
{
    public static class UserApplicationBusiness
    {
        public static List<UserApplicationData> GetUserApplic()
        {
            return new UserApplicationProvider().GetUserApplic();
        }
    }
}
