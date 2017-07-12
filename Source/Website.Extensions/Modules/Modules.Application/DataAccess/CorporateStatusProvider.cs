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
                CorporateStatusTable.StoreProcedure, out List<CorporateStatusData> list);
            return list;
        }
        
        public CorporateStatusData GetCorporateStatus(string corporateStatusCode)
        {
            Connector.AddParameter(CorporateStatusTable.CorporateStatusCode, SqlDbType.VarChar, corporateStatusCode);
            Connector.ExecuteProcedure(CorporateStatusTable.StoreProcedure, out CorporateStatusData result);
            return result;
        }
    }
}
