using Modules.UserManagement.DataTransfer;
using Website.Library.Business;
using Website.Library.DataTransfer;
using Website.Library.Extension;

namespace Modules.UserManagement.Business
{
    public class BranchCacheBusiness<T> : BasicCacheBusiness<T> where T : BranchData
    {
        public override OrderedConcurrentDictionary<string, CacheData> Load()
        {
            OrderedConcurrentDictionary<string, CacheData> dictionary =
                new OrderedConcurrentDictionary<string, CacheData>();
            foreach (BranchData item in BranchBusiness.GetAllBranchInfo())
            {
                dictionary.TryAdd(item.BranchID, item);
            }
            return dictionary;
        }

        public override CacheData Reload(string branchID)
        {
            return BranchBusiness.GetBranchInfo(branchID);
        }
    }
}