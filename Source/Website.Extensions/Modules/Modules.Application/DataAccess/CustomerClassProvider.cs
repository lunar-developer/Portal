using System.Collections.Generic;
using System.Data;
using Modules.Application.Database;
using Modules.Application.DataTransfer;
using Website.Library.DataAccess;

namespace Modules.Application.DataAccess
{
    public class CustomerClassProvider : DataProvider
    {
        private static readonly string ScriptGetAllCustomerClass =
            $"Select * from dbo.{CustomerClassTable.TableName} with(nolock)";

        public List<CustomerClassData> GetAllCustomerClass()
        {
            Connector.ExecuteSql<CustomerClassData, List<CustomerClassData>>(ScriptGetAllCustomerClass,
                out List<CustomerClassData> result);
            return result;
        }

        private static readonly string ScriptGetCustomerClass = $@"
            {ScriptGetAllCustomerClass}
            where {CustomerClassTable.CustomerClassCode} = @{CustomerClassTable.CustomerClassCode}";

        public CustomerClassData GetCustomerClass(string code)
        {
            Connector.AddParameter(CustomerClassTable.CustomerClassCode, SqlDbType.VarChar, code);
            Connector.ExecuteSql(ScriptGetCustomerClass, out CustomerClassData result);
            return result;
        }
    }
}