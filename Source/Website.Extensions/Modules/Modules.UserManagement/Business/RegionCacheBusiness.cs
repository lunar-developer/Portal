using Modules.UserManagement.DataTransfer;
using Website.Library.Business;
using Website.Library.DataTransfer;
using Website.Library.Extension;

namespace Modules.UserManagement.Business
{
    public class RegionCacheBusiness<T> : BasicCacheBusiness<T> where T : RegionData
    {
        public override OrderedConcurrentDictionary<string, CacheData> Load()
        {
            OrderedConcurrentDictionary<string, CacheData> dictionary =
                new OrderedConcurrentDictionary<string, CacheData>();
            foreach (RegionData item in RegionBusiness.GetRegionInfo())
            {
                dictionary.TryAdd(item.RegionID, item);
            }
            return dictionary;
        }

        public override CacheData Reload(string regionID)
        {
            return RegionBusiness.GetRegionInfo(regionID);
        }
    }
}