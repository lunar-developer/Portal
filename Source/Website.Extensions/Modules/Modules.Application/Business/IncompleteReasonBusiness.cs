using Modules.Application.DataAccess;
using Modules.Application.DataTransfer;
using System.Collections.Generic;

namespace Modules.Application.Business
{
    public static class IncompleteReasonBusiness
    {
        public static List<IncompleteReasonData> GetAllIncompleteReason()
        {
            return new IncompleteReasonProvider().GetAllIncompleteReason();
        }

        public static IncompleteReasonData GetIncompleteReason(string incompleteReasonCode)
        {
            return new IncompleteReasonProvider().GetIncompleteReason(incompleteReasonCode);
        }
    }
}