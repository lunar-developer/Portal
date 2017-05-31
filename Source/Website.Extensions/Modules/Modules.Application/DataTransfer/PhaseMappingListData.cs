using System.Collections.Generic;
using Website.Library.DataTransfer;

namespace Modules.Application.DataTransfer
{
    public class PhaseMappingListData: CacheData
    {
        public string ApplicationTypeCode;
        public List<PhaseMappingData> PhaseListMapping;
    }
}
