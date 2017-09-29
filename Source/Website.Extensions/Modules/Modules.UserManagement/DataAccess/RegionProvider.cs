using System.Collections.Generic;
using System.Data;
using Modules.UserManagement.Database;
using Modules.UserManagement.DataTransfer;
using Website.Library.DataAccess;

namespace Modules.UserManagement.DataAccess
{
    internal class RegionProvider : DataProvider
    {
        public List<RegionData> GetRegionInfo()
        {
            Connector
                .ExecuteProcedure<RegionData, List<RegionData>>("dbo.UM_SP_GetRegion", out List<RegionData> result);
            return result;
        }

        public RegionData GetRegionInfo(string regionID)
        {
            Connector.AddParameter(RegionTable.RegionID, SqlDbType.Int, regionID);
            Connector.ExecuteProcedure("dbo.UM_SP_GetRegion", out RegionData result);
            return result;
        }
    }
}