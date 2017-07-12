using Modules.Application.DataAccess;
using Modules.Application.DataTransfer;
using System.Collections.Generic;

namespace Modules.Application.Business
{
    public static class CompanyBusiness
    {
        public static List<CompanyData> GetAllCompany()
        {
            return new CompanyProvider().GetAllCompany();
        }

        public static CompanyData GetCompany(string taxCode)
        {
            return new CompanyProvider().GetCompany(taxCode);
        }
    }
}
