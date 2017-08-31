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
                string key = UserPhaseBusiness.GetCacheKey(item.PhaseID.ToString(), item.UserID.ToString());
                dictionary.TryAdd(key, item);
            }
            return dictionary;
        }

        public CacheData Reload(string key)
        {
            string[] arrayValues = key.Split('-');
            if (arrayValues.Length != 2)
            {
                return null;
            }

            string phaseID = arrayValues[0];
            string userID = arrayValues[1];
            DataTable dataTable = UserPhaseBusiness.GetUserData(phaseID, userID);
            return  dataTable.Rows.Count > 0 ? new UserPhaseData(dataTable.Rows[0]) : null;
        }
    }
}