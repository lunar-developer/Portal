using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Website.Library.DataAccess;
using Modules.VSaleKit.Database;
using Modules.VSaleKit.DataTransfer;
using System.Data;
using Website.Library.Database;

namespace Modules.VSaleKit.DataAccess
{
    class AssignUserProvider : DataProvider
    {
        public DataTable AddSuperUser(SuperUserData superuser)
        {
            DataTable dtResult;
            Connector.AddParameter(SuperUserTable.UserName, SqlDbType.NChar, superuser.UserName);
            Connector.AddParameter(SuperUserTable.ManagerID, SqlDbType.NChar, superuser.ManagerID);
            Connector.AddParameter(SuperUserTable.UserCreate, SqlDbType.NChar, superuser.UserCreate);
            Connector.AddParameter(SuperUserTable.DateCreate, SqlDbType.NChar, superuser.DateCreate);

            Connector.ExecuteProcedure("VSK_InsertSuperUser", out dtResult);

            return dtResult;
        }
        public DataTable RemoveSuperUser(SuperUserData superuser)
        {
            DataTable dtResult;
            Connector.AddParameter(SuperUserTable.UserName, SqlDbType.NChar, superuser.UserName);
            Connector.AddParameter(SuperUserTable.ManagerID, SqlDbType.NChar, superuser.ManagerID);
            Connector.AddParameter(SuperUserTable.DateModif, SqlDbType.NChar, superuser.DateModif);
            Connector.AddParameter(SuperUserTable.UserModif, SqlDbType.NChar, superuser.UserModif);

            Connector.ExecuteProcedure("VSK_RemoveSuperUser", out dtResult);

            return dtResult;
        }
        public DataTable GetAssignUser(string userId)
        {
            DataTable dtResult;
            Connector.AddParameter("UserID", SqlDbType.NChar, userId);
            Connector.ExecuteProcedure("VSK_GetAssignUser", out dtResult);

            return dtResult;
        }
    }
}
