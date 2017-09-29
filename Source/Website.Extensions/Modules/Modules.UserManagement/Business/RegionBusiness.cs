using System.Collections.Generic;
using Modules.UserManagement.DataAccess;
using Modules.UserManagement.DataTransfer;

namespace Modules.UserManagement.Business
{
    public static class RegionBusiness
    {
        public static List<RegionData> GetRegionInfo()
        {
            return new RegionProvider().GetRegionInfo();
        }

        public static RegionData GetRegionInfo(string regionID)
        {
            return new RegionProvider().GetRegionInfo(regionID);
        }
    }
}