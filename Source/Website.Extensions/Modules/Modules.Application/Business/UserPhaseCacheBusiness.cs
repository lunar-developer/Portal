using System.Data;
using Modules.Application.DataTransfer;
using Website.Library.Business;
using Website.Library.DataTransfer;
using Website.Library.Extension;

namespace Modules.Application.Business
{
    public class UserPhaseCacheBusiness<T> : BasicCacheBusiness<T> where T : UserPhaseData
    {
        public override OrderedConcurrentDictionary<string, CacheData> Load()
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

        public override CacheData Reload(string key)
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