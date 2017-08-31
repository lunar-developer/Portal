using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Modules.VSaleKit.DataAccess;
using Modules.VSaleKit.DataTransfer;
using System.Data;

namespace Modules.VSaleKit.Business
{
    public static class AssignUserBusiness
    {
        public static DataTable AddSuperUser(SuperUserData superuser)
        {
            return new AssignUserProvider().AddSuperUser(superuser);
        }
        public static DataTable RemoveSuperUser(SuperUserData superuser)
        {
            return new AssignUserProvider().RemoveSuperUser(superuser);
        }
        public static DataTable GetAssignUser(string userId)
        {
            return new AssignUserProvider().GetAssignUser(userId);
        }
    }
}
