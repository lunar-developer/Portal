using Modules.Application.Database;
using Modules.Application.DataTransfer;
using System.Collections.Generic;
using System.Data;
using Website.Library.DataAccess;

namespace Modules.Application.DataAccess
{
    public class CompanyProvider : DataProvider
    {
        public List<CompanyData> GetAllCompany()
        {
            Connector.ExecuteProcedure<CompanyData, List<CompanyData>>(
                CompanyTable.StoreProcedure, out List<CompanyData> list);
            return list;
        }
        
        public CompanyData GetCompany(string taxCode)
        {
            Connector.AddParameter(CompanyTable.TaxCode, SqlDbType.VarChar, taxCode);
            Connector.ExecuteProcedure(CompanyTable.StoreProcedure, out CompanyData result);
            return result;
        }
    }
}
