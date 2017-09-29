using System.Collections.Generic;
using System.Data;
using Modules.Forex.Database;
using Modules.Forex.DataTransfer;
using Website.Library.DataAccess;
using Website.Library.DataTransfer;

namespace Modules.Forex.DataAccess
{
    class ReasonMappingCustomerTypeProvider: DataProvider
    {
        private readonly string ScriptAll = $@"Select * from dbo.{ReasonMappingCustomerTypeTable.TableName} with(nolock)";
        public List<ReasonMappingCustomerTypeData> GetAll()
        {
            Connector.ExecuteSql<ReasonMappingCustomerTypeData, List<ReasonMappingCustomerTypeData>>(ScriptAll,
                out List<ReasonMappingCustomerTypeData> outList);
            return outList;
        }
        private readonly string Script = $@"Select * from dbo.{ReasonMappingCustomerTypeTable.TableName} with(nolock)
                                            Where {ReasonMappingCustomerTypeTable.ID} = @{ReasonMappingCustomerTypeTable.ID}";
        public ReasonMappingCustomerTypeData GetItem(string key)
        {
            Connector.ExecuteSql(Script, out ReasonMappingCustomerTypeData data);
            return data;
        }

        
    }
}
