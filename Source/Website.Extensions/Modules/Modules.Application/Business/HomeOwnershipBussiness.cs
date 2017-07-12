using System.Collections.Generic;
using Modules.Application.DataAccess;
using Modules.Application.DataTransfer;

namespace Modules.Application.Business
{
    public static class HomeOwnershipBussiness
    {
        public static List<HomeOwnershipData> GetList()
        {
            return new HomeOwnershipProvider().GetList();
        }

        public static HomeOwnershipData GetItem(string key)
        {
            return new HomeOwnershipProvider().GetItem(key);
        }
    }
}