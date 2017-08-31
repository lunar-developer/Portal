using Modules.Application.Database;
using Modules.Application.DataTransfer;
using System.Collections.Generic;
using System.Data;
using Website.Library.DataAccess;

namespace Modules.Application.DataAccess
{
    public class CorporateEntityTypeProvider : DataProvider
    {
        public List<CorporateEntityTypeData> GetAllCorporateEntityType()
        {
            Connector.ExecuteProcedure<CorporateEntityTypeData, List<CorporateEntityTypeData>>(
                "dbo.APP_SP_GetCorporateEntityType", out List<CorporateEntityTypeData> list);
            return list;
        }
        
        public CorporateEntityTypeData GetCorporateEntityType(string corporateEntityTypeCode)
        {
            Connector.AddParameter(CorporateEntityTypeTable.CorporateEntityTypeCode, SqlDbType.VarChar, corporateEntityTypeCode);
            Connector.ExecuteProcedure("dbo.APP_SP_GetCorporateEntityType", out CorporateEntityTypeData result);
            return result;
        }
    }
}
