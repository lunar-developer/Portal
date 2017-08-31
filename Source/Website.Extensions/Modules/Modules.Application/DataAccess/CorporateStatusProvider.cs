using Modules.Application.Database;
using Modules.Application.DataTransfer;
using System.Collections.Generic;
using System.Data;
using Website.Library.DataAccess;

namespace Modules.Application.DataAccess
{
    public class CorporateStatusProvider : DataProvider
    {
        public List<CorporateStatusData> GetAllCorporateStatus()
        {
            Connector.ExecuteProcedure<CorporateStatusData, List<CorporateStatusData>>(
                "dbo.APP_SP_GetCorporateStatus", out List<CorporateStatusData> list);
            return list;
        }
        
        public CorporateStatusData GetCorporateStatus(string corporateStatusCode)
        {
            Connector.AddParameter(CorporateStatusTable.CorporateStatusCode, SqlDbType.VarChar, corporateStatusCode);
            Connector.ExecuteProcedure("dbo.APP_SP_GetCorporateStatus", out CorporateStatusData result);
            return result;
        }
    }
}
