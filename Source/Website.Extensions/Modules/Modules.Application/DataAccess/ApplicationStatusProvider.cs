using System.Collections.Generic;
using System.Data;
using Modules.Application.Database;
using Modules.Application.DataTransfer;
using Website.Library.DataAccess;

namespace Modules.Application.DataAccess
{
    internal class ApplicationStatusProvider : DataProvider
    {
        public List<ApplicationStatusData> GetAllApplicationStatus()
        {
            Connector.ExecuteSql<ApplicationStatusData, List<ApplicationStatusData>>(
                "dbo.APP_SP_GetApplicationStatus", out List<ApplicationStatusData> list);
            return list;
        }

        public ApplicationStatusData GetApplicationStatus(string applicationStatusID)
        {
            Connector.AddParameter(ApplicationStatusTable.ApplicationStatusID, SqlDbType.Int, applicationStatusID);
            Connector.ExecuteSql("dbo.APP_SP_GetApplicationStatus", out ApplicationStatusData applicationStatus);
            return applicationStatus;
        }
    }
}