using System.Collections.Generic;
using System.Data;
using Modules.Application.Database;
using Modules.Application.DataTransfer;
using Website.Library.DataAccess;

namespace Modules.Application.DataAccess
{
    public class PhaseProvider: DataProvider
    {
        public List<PhaseData> GetAllPhase()
        {
            List<PhaseData> list;
            Connector.ExecuteProcedure<PhaseData, List<PhaseData>>(PhaseTable.StoredProcedure, out list);
            return list;
        }

        public PhaseData GetPhaseByCode(string code)
        {
            PhaseData phase;
            Connector.AddParameter(PhaseTable.PhaseCode, SqlDbType.VarChar, code);
            Connector.ExecuteProcedure(PhaseTable.StoredProcedure, out phase);
            return phase;
        }
    }
}
