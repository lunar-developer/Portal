using Modules.Application.DataAccess;
using Modules.Application.DataTransfer;
using System.Collections.Generic;

namespace Modules.Application.Business
{
    public static class WardBusiness
    {
        public static List<WardData> GetAllWard()
        {
            return new WardProvider().GetAllWard();
        }

        public static WardData GetWard(string wardCode)
        {
            return new WardProvider().GetWard(wardCode);
        }
    }
}
