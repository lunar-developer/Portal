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
                "dbo.APP_SP_GetContractType", out List<ContractTypeData> list);
            return list;
        }
        
        public ContractTypeData GetContractType(string contractTypeCode)
        {
            Connector.AddParameter(ContractTypeTable.ContractTypeCode, SqlDbType.VarChar, contractTypeCode);
            Connector.ExecuteProcedure("dbo.APP_SP_GetContractType", out ContractTypeData result);
            return result;
        }
    }
}
