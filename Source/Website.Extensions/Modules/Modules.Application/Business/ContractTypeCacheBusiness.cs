using System.Collections.Generic;
using System.Linq;
using Modules.Application.DataTransfer;
using Website.Library.Business;
using Website.Library.DataTransfer;
using Website.Library.Extension;

namespace Modules.Application.Business
{
    public class ContractTypeCacheBusiness<T> : BasicCacheBusiness<T> where T : ContractTypeData
    {
        public override OrderedConcurrentDictionary<string, CacheData> Load()
        {
            OrderedConcurrentDictionary<string, CacheData> dictionary =
                new OrderedConcurrentDictionary<string, CacheData>();
            foreach (ContractTypeData item in ContractTypeBusiness.GetAllContractType())
            {
                dictionary.TryAdd(item.ContractTypeCode, item);
            }
            return dictionary;
        }

        public override CacheData Reload(string contractTypeCode)
        {
            return ContractTypeBusiness.GetContractType(contractTypeCode);
        }

        public override bool Arrange(OrderedConcurrentDictionary<string, CacheData> dataDictionary)
        {
            List<string> listOrderedKeys = dataDictionary.Values
                .Cast<ContractTypeData>()
                .OrderBy(item => int.Parse(item.SortOrder))
                .Select(item => item.ContractTypeCode)
                .ToList();

            return dataDictionary.TryArrange(listOrderedKeys);
        }
    }
}