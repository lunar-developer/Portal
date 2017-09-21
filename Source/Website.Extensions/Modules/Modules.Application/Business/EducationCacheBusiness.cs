using System.Collections.Generic;
using System.Linq;
using Modules.Application.DataTransfer;
using Website.Library.Business;
using Website.Library.DataTransfer;
using Website.Library.Extension;

namespace Modules.Application.Business
{
    public class EducationCacheBusiness<T> : BasicCacheBusiness<T> where T : EducationData
    {
        public override OrderedConcurrentDictionary<string, CacheData> Load()
        {
            OrderedConcurrentDictionary<string, CacheData> dictionary =
                new OrderedConcurrentDictionary<string, CacheData>();
            foreach (EducationData item in EducationBusiness.GetList())
            {
                dictionary.TryAdd(item.EducationCode, item);
            }
            return dictionary;
        }

        public override CacheData Reload(string key)
        {
            return EducationBusiness.GetItem(key);
        }

        public override bool Arrange(OrderedConcurrentDictionary<string, CacheData> dataDictionary)
        {
            List<string> listOrderedKeys = dataDictionary.Values
                .Cast<EducationData>()
                .OrderBy(item => int.Parse(item.SortOrder))
                .Select(item => item.EducationCode)
                .ToList();

            return dataDictionary.TryArrange(listOrderedKeys);
        }
    }
}