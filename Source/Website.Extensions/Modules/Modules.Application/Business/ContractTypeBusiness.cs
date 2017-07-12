using Modules.Application.DataAccess;
using Modules.Application.DataTransfer;
using System.Collections.Generic;

namespace Modules.Application.Business
{
    public static class ContractTypeBusiness
    {
        public static List<ContractTypeData> GetAllContractType()
        {
            return new ContractTypeProvider().GetAllContractType();
        }

        public static ContractTypeData GetContractType(string contractTypeCode)
        {
            return new ContractTypeProvider().GetContractType(contractTypeCode);
        }
    }
}
