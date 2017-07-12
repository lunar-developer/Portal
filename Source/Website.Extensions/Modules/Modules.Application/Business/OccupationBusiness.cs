using Modules.Application.DataAccess;
using Modules.Application.DataTransfer;
using System.Collections.Generic;

namespace Modules.Application.Business
{
    public static class OccupationBusiness
    {
        public static List<OccupationData> GetAllOccupation()
        {
            return new OccupationProvider().GetAllOccupation();
        }

        public static OccupationData GetOccupation(string occupationCode)
        {
            return new OccupationProvider().GetOccupation(occupationCode);
        }
    }
}
