using Modules.Application.Database;
using Modules.Application.DataTransfer;
using System.Collections.Generic;
using System.Data;
using Website.Library.DataAccess;

namespace Modules.Application.DataAccess
{
    public class DecisionReasonProvider : DataProvider
    {
        public List<DecisionReasonData> GetAllDecisionReason()
        {
            Connector.ExecuteProcedure<DecisionReasonData, List<DecisionReasonData>>(
                DecisionReasonTable.StoreProcedure, out List<DecisionReasonData> list);
            return list;
        }
        
        public DecisionReasonData GetDecisionReason(string decisionReasonCode)
        {
            Connector.AddParameter(DecisionReasonTable.DecisionReasonCode, SqlDbType.VarChar, decisionReasonCode);
            Connector.ExecuteProcedure(DecisionReasonTable.StoreProcedure, out DecisionReasonData result);
            return result;
        }
    }
}