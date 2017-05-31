using System.Collections.Generic;
using System.Data;
using Modules.VSaleKit.Database;
using Website.Library.DataAccess;
using Website.Library.Database;

namespace Modules.VSaleKit.DataAccess
{
    public class ApplicationFormProvider : DataProvider
    {
        public DataTable SearchApplication(Dictionary<string, string> dictionary)
        {
            foreach (KeyValuePair<string, string> pair in dictionary)
            {
                Connector.AddParameter(pair.Key, SqlDbType.VarChar, pair.Value);
            }
            Connector.ExecuteProcedure("VSK_SP_SearchApplication", out DataTable dtResult);
            return dtResult;
        }

        public DataSet LoadApplication(string uniqueID, int userID)
        {
            Connector.AddParameter(ApplicationFormTable.UniqueID, SqlDbType.VarChar, uniqueID);
            Connector.AddParameter(BaseTable.UserID, SqlDbType.Int, userID);
            Connector.ExecuteProcedure("dbo.VSK_SP_GetApplication", out DataSet result);
            return result;
        }

        public bool ProcessApplication(string uniqueID, string userName, string roleName, string branchCode, string description)
        {
            Connector.AddParameter("ApplicID", SqlDbType.VarChar, uniqueID);
            Connector.AddParameter("UserName", SqlDbType.VarChar, userName);
            Connector.AddParameter("UserType", SqlDbType.VarChar, roleName);
            Connector.AddParameter("BranchID", SqlDbType.VarChar, branchCode);
            Connector.AddParameter("Description", SqlDbType.VarChar, description);
            Connector.AddParameter("UserReceiveNotification", SqlDbType.VarChar, string.Empty);
            Connector.AddParameter("Token", SqlDbType.VarChar, string.Empty);
            Connector.AddParameter("StatusID", SqlDbType.VarChar, string.Empty);
            Connector.AddParameter("ErrorCode", SqlDbType.VarChar, string.Empty);
            Connector.ExecuteProcedure("VSaleKit.dbo.Approval");
            return true;
        }
    }
}