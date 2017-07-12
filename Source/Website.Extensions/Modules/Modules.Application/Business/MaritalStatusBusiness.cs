using System.Collections.Generic;
using Modules.Application.DataAccess;
using Modules.Application.DataTransfer;

namespace Modules.Application.Business
{
    public static class MaritalStatusBusiness
    {
        public static List<MaritalStatusData> GetList()
        {
            return new MaritalStatusProvider().GetList();
        }

        public static MaritalStatusData GetItem(string key)
        {
            return new MaritalStatusProvider().GetItem(key);
        }
    }
}