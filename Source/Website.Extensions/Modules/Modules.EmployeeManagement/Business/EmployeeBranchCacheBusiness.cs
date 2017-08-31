using System;
using Modules.EmployeeManagement.DataTransfer;
using Website.Library.DataTransfer;
using Website.Library.Extension;
using Website.Library.Interface;
using System.Collections.Generic;

namespace Modules.EmployeeManagement.Business
{
    public class EmployeeBranchCacheBusiness<T> : ICache where T : EmployeeBranchData
    {
        public Type GetCacheType()
        {
            return typeof(T);
        }

        public OrderedConcurrentDictionary<string,CacheData> Load()
        {
            OrderedConcurrentDictionary<string, CacheData> dictionary =
                new OrderedConcurrentDictionary<string, CacheData>();

            List<EmployeeBranchData> listBranch = EmployeeBusiness.GetAllBranch();
            for (int i = 0; i < listBranch.Count; i++)
            {
                EmployeeBranchData branch = listBranch[i];
                dictionary.TryAdd((i + 1).ToString(), branch);
            }
            return dictionary;
        }

        public CacheData Reload(string key)
        {
            return null;
        }
    }
}
