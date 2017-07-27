using System.Collections.Generic;
using Modules.Application.DataAccess;
using Modules.Application.DataTransfer;
using Website.Library.Global;

namespace Modules.Application.Business
{
    public static class PhaseBussiness
    {
        public static List<PhaseData> GetAllPhaseData()
        {
            return new PhaseProvider().GetAllPhaseData();
        }

        public static PhaseData GetPhaseData(string phaseID)
        {
            return new PhaseProvider().GetPhaseData(phaseID);
        }

        public static string GetPhaseName(string phaseID)
        {
            PhaseData phaseData = CacheBase.Receive<PhaseData>(phaseID);
            return phaseData?.Name ?? phaseID;
        }

        public static string GetPhaseStatus(string phaseID)
        {
            PhaseData phaseData = CacheBase.Receive<PhaseData>(phaseID);
            return phaseData?.ApplicationStatus ?? "0";
        }
    }
}