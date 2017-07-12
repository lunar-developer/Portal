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
                CorporateSizeTable.StoreProcedure, out List<CorporateSizeData> list);
            return list;
        }
        
        public CorporateSizeData GetCorporateSize(string corporateSizeCode)
        {
            Connector.AddParameter(CorporateSizeTable.CorporateSizeCode, SqlDbType.VarChar, corporateSizeCode);
            Connector.ExecuteProcedure(CorporateSizeTable.StoreProcedure, out CorporateSizeData result);
            return result;
        }
    }
}
