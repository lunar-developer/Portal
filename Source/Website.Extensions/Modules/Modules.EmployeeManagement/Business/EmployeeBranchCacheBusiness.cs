using System.Collections.Generic;
using Modules.EmployeeManagement.DataTransfer;
using Website.Library.Business;
using Website.Library.DataTransfer;
using Website.Library.Extension;

namespace Modules.EmployeeManagement.Business
{
    public class EmployeeBranchCacheBusiness<T> : BasicCacheBusiness<T> where T : EmployeeBranchData
    {
        public override OrderedConcurrentDictionary<string,CacheData> Load()
        {
            OrderedConcurrentDictionary<string, CacheData> dictionary =
                new OrderedConcurrentDictionary<string, CacheData>();

            List<EmployeeBranchData> listBranch = EmployeeBusiness.LoadEmployeeBranch();
            for (int i = 0; i < listBranch.Count; i++)
            {
                EmployeeBranchData branch = listBranch[i];
                dictionary.TryAdd((i + 1).ToString(), branch);
            }
            return dictionary;
        }
    }
}
