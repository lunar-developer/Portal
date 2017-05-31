using System.Collections.Generic;
using Modules.Application.DataAccess;
using Modules.Application.DataTransfer;

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
    }
}