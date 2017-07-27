using System.Collections.Generic;
using System.Data;
using Modules.Application.Database;
using Modules.Application.DataTransfer;
using Website.Library.DataAccess;

namespace Modules.Application.DataAccess
{
    public class PhaseProvider: DataProvider
    {
        public List<PhaseData> GetAllPhaseData()
        {
            Connector.ExecuteProcedure<PhaseData, List<PhaseData>>("dbo.APP_SP_GetPhase", out List<PhaseData> list);
            return list;
        }

        public PhaseData GetPhaseData(string phaseID)
        {
            Connector.AddParameter(PhaseTable.PhaseID, SqlDbType.Int, phaseID);
            Connector.ExecuteProcedure("dbo.APP_SP_GetPhase", out PhaseData phase);
            return phase;
        }
    }
}