using System.Collections.Generic;
using Modules.Application.DataAccess;
using Modules.Application.DataTransfer;

namespace Modules.Application.Business
{
    public static class CountryBusiness
    {
        public static List<CountryData> GetAllCountry()
        {
            return new CountryProvider().GetAllCountry();
        }

        public static CountryData GetCountry(string countryCode)
        {
            return new CountryProvider().GetCountry(countryCode);
        }
    }
}