using Modules.Application.Database;
using Modules.Application.DataTransfer;
using System.Collections.Generic;
using System.Data;
using Website.Library.DataAccess;

namespace Modules.Application.DataAccess
{
    public class ContractTypeProvider : DataProvider
    {
        public List<ContractTypeData> GetAllContractType()
        {
            Connector.ExecuteProcedure<ContractTypeData, List<ContractTypeData>>(
                ContractTypeTable.StoreProcedure, out List<ContractTypeData> list);
            return list;
        }
        
        public ContractTypeData GetContractType(string contractTypeCode)
        {
            Connector.AddParameter(ContractTypeTable.ContractTypeCode, SqlDbType.VarChar, contractTypeCode);
            Connector.ExecuteProcedure(ContractTypeTable.StoreProcedure, out ContractTypeData result);
            return result;
        }
    }
}
