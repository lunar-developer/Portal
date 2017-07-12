using Modules.Application.DataTransfer;
using System;
using Website.Library.DataTransfer;
using Website.Library.Extension;
using Website.Library.Interface;

namespace Modules.Application.Business
{
    public class ContractTypeCacheBusiness<T> : ICache where T : ContractTypeData
    {
        public Type GetCacheType()
        {
            return typeof(T);
        }

        public OrderedConcurrentDictionary<string, CacheData> Load()
        {
            OrderedConcurrentDictionary<string, CacheData> dictionary =
                new OrderedConcurrentDictionary<string, CacheData>();
            foreach (ContractTypeData item in ContractTypeBusiness.GetAllContractType())
            {
                dictionary.TryAdd(item.ContractTypeCode, item);
            }
            return dictionary;
        }

        public CacheData Reload(string contractTypeCode)
        {
            return ContractTypeBusiness.GetContractType(contractTypeCode);
        }
    }
}