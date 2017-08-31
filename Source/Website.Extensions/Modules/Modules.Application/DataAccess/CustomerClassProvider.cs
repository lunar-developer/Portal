using System.Collections.Generic;
using System.Data;
using Modules.Application.Database;
using Modules.Application.DataTransfer;
using Website.Library.DataAccess;

namespace Modules.Application.DataAccess
{
    public class CustomerClassProvider : DataProvider
    {
        public List<CustomerClassData> GetAllCustomerClass()
        {
            Connector.ExecuteSql<CustomerClassData, List<CustomerClassData>>("dbo.APP_SP_GetCustomerClass",
                out List<CustomerClassData> result);
            return result;
        }

        public CustomerClassData GetCustomerClass(string code)
        {
            Connector.AddParameter(CustomerClassTable.CustomerClassCode, SqlDbType.VarChar, code);
            Connector.ExecuteSql("dbo.APP_SP_GetCustomerClass", out CustomerClassData result);
            return result;
        }
    }
}