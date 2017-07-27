using System;
using System.Data;
using Modules.Application.DataTransfer;
using Website.Library.DataTransfer;
using Website.Library.Extension;
using Website.Library.Interface;

namespace Modules.Application.Business
{
    public class UserPhaseCacheBusiness<T> : ICache where T : UserPhaseData
    {
        public Type GetCacheType()
        {
            return typeof(T);
        }

        public OrderedConcurrentDictionary<string, CacheData> Load()
        {
            OrderedConcurrentDictionary<string, CacheData> dictionary =
                new OrderedConcurrentDictionary<string, CacheData>();

            foreach (DataRow row in UserPhaseBusiness.GetAllUserData().Rows)
            {
                UserPhaseData item = new UserPhaseData(row);
                dictionary.TryAdd(item.UserID.ToString(), item);
            }
            return dictionary;
        }

        public CacheData Reload(string userID)
        {
            DataTable dataTable = UserPhaseBusiness.GetUserData(userID);
            return  dataTable.Rows.Count > 0 ? new UserPhaseData(dataTable.Rows[0]) : null;
        }
    }
}