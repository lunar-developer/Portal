using System.Collections.Generic;
using System.Data;
using Modules.Application.Database;
using Modules.Application.DataTransfer;
using Website.Library.DataAccess;

namespace Modules.Application.DataAccess
{
    internal class ApplicationTypeProvider : DataProvider
    {
        public List<ApplicationTypeData> GetAllApplicationTypes()
        {
            Connector.ExecuteSql<ApplicationTypeData, List<ApplicationTypeData>>("dbo.APP_SP_GetApplicationType", 
                out List<ApplicationTypeData> list);
            return list;
        }

        public ApplicationTypeData GetApplicationType(string applicationTypeID)
        {
            Connector.AddParameter(ApplicationTypeTable.ApplicationTypeID, SqlDbType.Int, applicationTypeID);
            Connector.ExecuteSql("dbo.APP_SP_GetApplicationType", out ApplicationTypeData applicationType);
            return applicationType;
        }
    }
}