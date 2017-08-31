using System.Collections.Generic;
using System.Data;
using Modules.Application.Database;
using Modules.Application.DataTransfer;
using Website.Library.DataAccess;

namespace Modules.Application.DataAccess
{
    public class RouteProvider: DataProvider 
    {
        public List<RouteData> GetAllRouteData()
        {
            Connector.ExecuteProcedure<RouteData, List<RouteData>>("dbo.APP_SP_GetRoute", out List<RouteData> list);
            return list;
        }

        public RouteData GetRouteData(string routeID)
        {
            Connector.AddParameter(RouteTable.RouteID, SqlDbType.Int, routeID);
            Connector.ExecuteProcedure("dbo.APP_SP_GetRoute", out RouteData result);
            return result;
        }
    }
}