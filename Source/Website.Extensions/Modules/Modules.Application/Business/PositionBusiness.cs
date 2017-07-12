using System.Collections.Generic;
using Modules.Application.DataAccess;
using Modules.Application.DataTransfer;

namespace Modules.Application.Business
{
    public static class PositionBusiness
    {
        public static List<PositionData> GetList()
        {
            return new PositionProvider().GetList();
        }

        public static PositionData GetItem(string key)
        {
            return new PositionProvider().GetItem(key);
        }
    }
}