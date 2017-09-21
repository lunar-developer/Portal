using Modules.Application.Database;
using Modules.Application.DataTransfer;
using System.Collections.Generic;
using System.Data;
using Website.Library.DataAccess;

namespace Modules.Application.DataAccess
{
    public class WardProvider : DataProvider
    {
        public List<WardData> GetAllWard()
        {
            Connector.ExecuteProcedure<WardData,List<WardData>>("dbo.APP_SP_GetWard", out List<WardData> result);
            return result;
        }

        public WardData GetWard(string wardCode)
        {
            Connector.AddParameter(WardTable.WardCode, SqlDbType.VarChar, wardCode);
            Connector.ExecuteProcedure("dbo.APP_SP_GetWard", out WardData result);
            return result;
        }
    }
}
