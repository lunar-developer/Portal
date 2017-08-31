using System.Collections.Generic;
using Modules.Application.DataAccess;
using Modules.Application.DataTransfer;
using Website.Library.Global;

namespace Modules.Application.Business
{
    public static class RouteBusiness
    {
        public static List<RouteData> GetAllRouteData()
        {
            return new RouteProvider().GetAllRouteData();
        }

        public static RouteData GetRouteData(string routeID)
        {
            return new RouteProvider().GetRouteData(routeID);
        }

        public static string GetRouteName(string routeID)
        {
            RouteData routeInfo = CacheBase.Receive<RouteData>(routeID);
            if (routeInfo == null)
            {
                return routeID;
            }

            string currentPhaseName = PhaseBussiness.GetPhaseName(routeInfo.PhaseID);
            string targetPhaseName = PhaseBussiness.GetPhaseName(routeInfo.TargetPhaseID);
            string processName = ProcessBusiness.GetProcessName(routeInfo.ProcessID);
            return $"{processName} - {currentPhaseName} => {targetPhaseName}";
        }
    }
}