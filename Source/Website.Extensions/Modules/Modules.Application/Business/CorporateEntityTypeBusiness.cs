using Modules.Application.DataAccess;
using Modules.Application.DataTransfer;
using System.Collections.Generic;

namespace Modules.Application.Business
{
    public static class CorporateEntityTypeBusiness
    {
        public static List<CorporateEntityTypeData> GetAllCorporateEntityType()
        {
            return new CorporateEntityTypeProvider().GetAllCorporateEntityType();
        }

        public static CorporateEntityTypeData GetCorporateEntityType(string corporateEntityTypeCode)
        {
            return new CorporateEntityTypeProvider().GetCorporateEntityType(corporateEntityTypeCode);
        }
    }
}
