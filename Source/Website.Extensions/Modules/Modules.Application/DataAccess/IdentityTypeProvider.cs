using System.Collections.Generic;
using System.Data;
using Modules.Application.Database;
using Modules.Application.DataTransfer;
using Website.Library.DataAccess;

namespace Modules.Application.DataAccess
{
    public class IdentityTypeProvider : DataProvider
    {
        private static readonly string ScriptGetAllIdentityType =
            $"select * from dbo.{IdentityTypeTable.TableName} with(nolock)";

        public List<IdentityTypeData> GetAllIdentityType()
        {
            Connector.ExecuteSql<IdentityTypeData, List<IdentityTypeData>>(ScriptGetAllIdentityType,
                out List<IdentityTypeData> result);
            return result;
        }

        private static readonly string ScriptGetIdentityType = $@"
            {ScriptGetAllIdentityType}
            where {IdentityTypeTable.IdentityTypeID} = @{IdentityTypeTable.IdentityTypeID}";

        public IdentityTypeData GetIdentityType(string identityTypeID)
        {
            Connector.AddParameter(IdentityTypeTable.IdentityTypeID, SqlDbType.TinyInt, identityTypeID);
            Connector.ExecuteSql(ScriptGetIdentityType, out IdentityTypeData result);
            return result;
        }
    }
}