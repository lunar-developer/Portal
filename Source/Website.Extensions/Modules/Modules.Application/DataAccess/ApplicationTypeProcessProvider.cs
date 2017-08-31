using System.Collections.Generic;
using System.Data;
using Modules.Application.Database;
using Modules.Application.DataTransfer;
using Website.Library.DataAccess;

namespace Modules.Application.DataAccess
{
    internal class ApplicationTypeProcessProvider : DataProvider
    {
        public List<ApplicationTypeProcessData> GetAllApplicationTypeProcess()
        {
            Connector.ExecuteSql<ApplicationTypeProcessData, List<ApplicationTypeProcessData>>(
                "dbo.APP_SP_GetApplicationTypeProcess", out List<ApplicationTypeProcessData> list);
            return list;
        }

        public ApplicationTypeProcessData GetApplicationTypeProcess(string applicationTypeID)
        {
            Connector.AddParameter(ApplicationTypeTable.ApplicationTypeID, SqlDbType.Int, applicationTypeID);
            Connector.ExecuteSql("dbo.APP_SP_GetApplicationTypeProcess", out ApplicationTypeProcessData result);
            return result;
        }
    }
}