using Modules.Application.Database;
using Modules.Application.DataTransfer;
using System.Collections.Generic;
using System.Data;
using Website.Library.DataAccess;

namespace Modules.Application.DataAccess
{
    public class OccupationProvider : DataProvider
    {
        public List<OccupationData> GetAllOccupation()
        {
            Connector.ExecuteProcedure<OccupationData, List<OccupationData>>(
                "dbo.APP_SP_GetOccupation", out List<OccupationData> list);
            return list;
        }
        
        public OccupationData GetOccupation(string occupationCode)
        {
            Connector.AddParameter(OccupationTable.OccupationCode, SqlDbType.VarChar, occupationCode);
            Connector.ExecuteProcedure("dbo.APP_SP_GetOccupation", out OccupationData result);
            return result;
        }
    }
}
