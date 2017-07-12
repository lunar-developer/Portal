using Modules.Application.DataAccess;
using Modules.Application.DataTransfer;
using System.Collections.Generic;

namespace Modules.Application.Business
{
    public static class CorporateStatusBusiness
    {
        public static List<CorporateStatusData> GetAllCorporateStatus()
        {
            return new CorporateStatusProvider().GetAllCorporateStatus();
        }

        public static CorporateStatusData GetCorporateStatus(string corporateStatusCode)
        {
            return new CorporateStatusProvider().GetCorporateStatus(corporateStatusCode);
        }
    }
}
