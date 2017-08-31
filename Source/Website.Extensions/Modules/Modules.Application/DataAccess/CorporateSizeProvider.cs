using Modules.Application.Database;
using Modules.Application.DataTransfer;
using System.Collections.Generic;
using System.Data;
using Website.Library.DataAccess;

namespace Modules.Application.DataAccess
{
    public class CorporateSizeProvider : DataProvider
    {
        public List<CorporateSizeData> GetAllCorporateSize()
        {
            Connector.ExecuteProcedure<CorporateSizeData, List<CorporateSizeData>>(
                "dbo.APP_SP_GetCorporateSize", out List<CorporateSizeData> list);
            return list;
        }
        
        public CorporateSizeData GetCorporateSize(string corporateSizeCode)
        {
            Connector.AddParameter(CorporateSizeTable.CorporateSizeCode, SqlDbType.VarChar, corporateSizeCode);
            Connector.ExecuteProcedure("dbo.APP_SP_GetCorporateSize", out CorporateSizeData result);
            return result;
        }
    }
}
