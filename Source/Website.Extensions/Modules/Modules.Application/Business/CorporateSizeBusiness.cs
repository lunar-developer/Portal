using Modules.Application.DataAccess;
using Modules.Application.DataTransfer;
using System.Collections.Generic;

namespace Modules.Application.Business
{
    public static class CorporateSizeBusiness
    {
        public static List<CorporateSizeData> GetAllCorporateSize()
        {
            return new CorporateSizeProvider().GetAllCorporateSize();
        }

        public static CorporateSizeData GetCorporateSize(string corporateSizeCode)
        {
            return new CorporateSizeProvider().GetCorporateSize(corporateSizeCode);
        }
    }
}
