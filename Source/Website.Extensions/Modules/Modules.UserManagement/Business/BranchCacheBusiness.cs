using System;
using Modules.UserManagement.DataTransfer;
using Website.Library.DataTransfer;
using Website.Library.Extension;
using Website.Library.Interface;

namespace Modules.UserManagement.Business
{
    public class BranchCacheBusiness<T> : ICache where T : BranchData
    {
        public Type GetCacheType()
        {
            return typeof(T);
        }

        public OrderedConcurrentDictionary<string, CacheData> Load()
        {
            OrderedConcurrentDictionary<string, CacheData> dictionary =
                new OrderedConcurrentDictionary<string, CacheData>();
            foreach (BranchData item in BranchBusiness.GetAllBranchInfo())
            {
                dictionary.TryAdd(item.BranchID, item);
            }
            return dictionary;
        }

        public CacheData Reload(string branchID)
        {
            return BranchBusiness.GetBranchInfo(branchID);
        }
    }
}