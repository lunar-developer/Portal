using System.Collections.Generic;
using System.Data;
using Modules.Application.Database;
using Modules.Application.DataTransfer;
using Website.Library.DataAccess;

namespace Modules.Application.DataAccess
{
    public class IdentityTypeProvider : DataProvider
    {
        public List<IdentityTypeData> GetAllIdentityType()
        {
            Connector.ExecuteSql<IdentityTypeData, List<IdentityTypeData>>("dbo.APP_SP_GetIdentityType",
                out List<IdentityTypeData> result);
            return result;
        }

        public IdentityTypeData GetIdentityType(string identityTypeID)
        {
            Connector.AddParameter(IdentityTypeTable.IdentityTypeID, SqlDbType.TinyInt, identityTypeID);
            Connector.ExecuteSql("dbo.APP_SP_GetIdentityType", out IdentityTypeData result);
            return result;
        }
    }
}