using System.Collections.Generic;
using Modules.Application.DataAccess;
using Modules.Application.DataTransfer;
using Website.Library.Global;

namespace Modules.Application.Business
{
    public static class StateBusiness
    {
        public static List<StateData> GetAllState()
        {
            return new StateProvider().GetAllState();
        }

        public static StateData GetState(string stateCode)
        {
            return new StateProvider().GetState(stateCode);
        }

        public static string GetStateName(string stateCode)
        {
            StateData cacheData = CacheBase.Receive<StateData>(stateCode);
            return cacheData != null
                ? $"{cacheData.StateCode} - {cacheData.StateName}"
                : stateCode;
        }
    }
}