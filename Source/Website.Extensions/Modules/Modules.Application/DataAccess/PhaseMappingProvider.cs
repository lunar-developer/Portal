using System.Collections.Generic;
using System.Data;
using Modules.Application.Database;
using Modules.Application.DataTransfer;
using Website.Library.DataAccess;
using Website.Library.Global;

namespace Modules.Application.DataAccess
{
    public class PhaseMappingProvider: DataProvider
    {
        private List<PhaseMappingData> GetListAllItemPhaseMapping(string applicationTypeCode)
        {
            List<PhaseMappingData> outList;
            Connector.AddParameter(PhaseMappingTable.ApplicationTypeCode, SqlDbType.VarChar,applicationTypeCode);
            Connector.ExecuteProcedure<PhaseMappingData, List<PhaseMappingData>>(PhaseMappingTable.StoredProcedure, out outList);
            return outList;
        }

        public PhaseMappingListData GetPhaseMapping(string applicationTypeCode)
        {
            List<PhaseMappingData> phaseMappingList = GetListAllItemPhaseMapping(applicationTypeCode);
            if (phaseMappingList == null || phaseMappingList.Count == 0) return null;
            PhaseMappingListData phaseMapping = new PhaseMappingListData
            {
                ApplicationTypeCode = applicationTypeCode,
                PhaseListMapping = phaseMappingList
            };
            return phaseMapping;
        }

        public List<PhaseMappingListData> GetListPhaseMapping()
        {
            List<PhaseMappingListData> outList = new List<PhaseMappingListData>();
            List<ApplicationTypeData> appTypeList = CacheBase.Receive<ApplicationTypeData>();
            
            if (appTypeList != null && appTypeList.Count > 0)
            {
                foreach (ApplicationTypeData item in appTypeList)
                {
                    List<PhaseMappingData> phaseMappingList = GetListAllItemPhaseMapping(item.ApplicationTypeID);
                    PhaseMappingListData addItem = new PhaseMappingListData
                    {
                        ApplicationTypeCode = item.ApplicationTypeID,
                        PhaseListMapping = phaseMappingList
                    };
                    outList.Add(addItem);
                }
            }
            return outList;
        }
    }
}
