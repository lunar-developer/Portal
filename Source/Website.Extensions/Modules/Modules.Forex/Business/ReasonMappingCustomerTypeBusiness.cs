using System.Collections.Generic;
using Modules.Forex.DataAccess;
using Modules.Forex.DataTransfer;

namespace Modules.Forex.Business
{
    public class ReasonMappingCustomerTypeBusiness
    {
        public static List<ReasonMappingCustomerTypeData> GetAll()
        {
            return new ReasonMappingCustomerTypeProvider().GetAll();
        }

        public static ReasonMappingCustomerTypeData GetItem(string key)
        {
            return new ReasonMappingCustomerTypeProvider().GetItem(key);
        }

        
    }
}
