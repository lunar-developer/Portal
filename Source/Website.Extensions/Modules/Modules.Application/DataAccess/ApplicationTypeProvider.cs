using System.Collections.Generic;
using System.Data;
using Modules.Application.Database;
using Modules.Application.DataTransfer;
using Website.Library.DataAccess;

namespace Modules.Application.DataAccess
{
    public class ApplicationTypeProvider : DataProvider
    {
        private static readonly string ScriptGetAllApplicationType = $@"
            select *
            from dbo.{ApplicationTypeTable.TableName} with(nolock)";

        public List<ApplicationTypeData> GetAllApplicationType()
        {
            Connector.ExecuteSql<ApplicationTypeData, List<ApplicationTypeData>>(ScriptGetAllApplicationType, 
                out List<ApplicationTypeData> list);
            return list;
        }

        private static readonly string ScriptGetApplicationType = $@"
            {ScriptGetAllApplicationType}
            where {ApplicationTypeTable.ApplicationTypeID} = @{ApplicationTypeTable.ApplicationTypeID}";

        public ApplicationTypeData GetApplicationType(string applicationTypeID)
        {
            Connector.AddParameter(ApplicationTypeTable.ApplicationTypeID, SqlDbType.TinyInt, applicationTypeID);
            Connector.ExecuteSql(ScriptGetApplicationType, out ApplicationTypeData applicationType);
            return applicationType;
        }
    }
}