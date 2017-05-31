using System.Collections.Generic;
using Modules.Application.DataAccess;
using Modules.Application.DataTransfer;

namespace Modules.Application.Business
{
    public static class PhaseMappingBussiness
    {
        public static List<PhaseMappingListData> GetListPhaseMapping()
        {
            return new PhaseMappingProvider().GetListPhaseMapping();
        }

        public static PhaseMappingListData GetPhaseMapping(string applicationTypeCode)
        {
            return new PhaseMappingProvider().GetPhaseMapping(applicationTypeCode);
        }
    }
}
