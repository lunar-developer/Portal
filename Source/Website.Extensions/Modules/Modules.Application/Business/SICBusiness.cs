using System.Collections.Generic;
using Modules.Application.DataAccess;
using Modules.Application.DataTransfer;

namespace Modules.Application.Business
{
    public static class SICBusiness
    {
        public static List<SICData> GetList()
        {
            return new SICProvider().GetList();
        }

        public static SICData GetItem(string key)
        {
            return new SICProvider().GetItem(key);
        }
    }
}