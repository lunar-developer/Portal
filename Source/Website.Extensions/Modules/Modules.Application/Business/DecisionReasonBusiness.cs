using Modules.Application.DataAccess;
using Modules.Application.DataTransfer;
using System.Collections.Generic;

namespace Modules.Application.Business
{
    public static class DecisionReasonBusiness
    {
        public static List<DecisionReasonData> GetAllDecisionReason()
        {
            return new DecisionReasonProvider().GetAllDecisionReason();
        }

        public static DecisionReasonData GetDecisionReason(string decisionReasonCode)
        {
            return new DecisionReasonProvider().GetDecisionReason(decisionReasonCode);
        }
    }
}