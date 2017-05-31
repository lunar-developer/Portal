using System.Collections.Generic;
using Modules.Application.DataAccess;
using Modules.Application.DataTransfer;

namespace Modules.Application.Business
{
    public static class CityBusiness
    {
        public static List<CityData> GetAllCity()
        {
            return new CityProvider().GetAllCity();
        }

        public static CityData GetCity(string cityCode)
        {
            return new CityProvider().GetCity(cityCode);
        }
    }
}