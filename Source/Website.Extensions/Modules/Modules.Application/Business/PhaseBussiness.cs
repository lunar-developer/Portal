using System.Collections.Generic;
using Modules.Application.DataAccess;
using Modules.Application.DataTransfer;

namespace Modules.Application.Business
{
    public static class PhaseBussiness
    {
        public static List<PhaseData> GetPhase()
        {
            return new PhaseProvider().GetAllPhase();
        }

        public static PhaseData GetPhaseByCode(string code)
        {
            return new PhaseProvider().GetPhaseByCode(code);
        }
    }
}
