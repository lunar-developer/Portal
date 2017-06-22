using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Modules.UserManagement.Database;
using Modules.UserManagement.DataTransfer;
using Website.Library.DataTransfer;
using Website.Library.Extension;
using Website.Library.Interface;

namespace Modules.UserManagement.Business
{
    public class BranchManagerCacheBusiness<T> : ICache where T : BranchManagerData
    {
        public Type GetCacheType()
        {
            return typeof(T);
        }
        
        public OrderedConcurrentDictionary<string, CacheData> Load()
        {
            List<BranchManagerData> listBranchManagerData = BindData(BranchBusiness.GetBranchManager());
            OrderedConcurrentDictionary<string, CacheData> dictionary =
                new OrderedConcurrentDictionary<string, CacheData>();
            foreach (BranchManagerData item in listBranchManagerData)
            {
                dictionary.TryAdd(item.BranchID, item);
            }
            return dictionary;
        }

        public CacheData Reload(string branchID)
        {
            List<BranchManagerData> listBranchManagerData = BindData(BranchBusiness.GetBranchManager(branchID));
            return listBranchManagerData.Count == 1 ? listBranchManagerData[0] : null;
        }

        private List<BranchManagerData> BindData(DataTable dataTable)
        {
            List<BranchManagerData> listBranchManagerData = new List<BranchManagerData>();
            foreach (DataRow row in dataTable.Rows)
            {
                string branchID = row[BranchTable.BranchID].ToString();
                string userID = row[UserTable.UserID].ToString();

                BranchManagerData item =
                    listBranchManagerData.FirstOrDefault(iterator => iterator.BranchID == branchID);
                if (item == null)
                {
                    item = new BranchManagerData
                    {
                        BranchID = branchID
                    };
                    listBranchManagerData.Add(item);
                }
                if (item.ListManager.Contains(userID) == false)
                {
                    item.ListManager.Add(userID);
                }
            }

            return listBranchManagerData;
        }
    }
}