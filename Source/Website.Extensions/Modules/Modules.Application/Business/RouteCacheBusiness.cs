using Modules.Application.DataTransfer;
using Website.Library.Business;
using Website.Library.DataTransfer;
using Website.Library.Extension;

namespace Modules.Application.Business
{
    public class RouteCacheBusiness<T> : BasicCacheBusiness<T> where T : RouteData
    {
        public override OrderedConcurrentDictionary<string, CacheData> Load()
        {
            OrderedConcurrentDictionary<string, CacheData> dictionary =
                new OrderedConcurrentDictionary<string, CacheData>();
            foreach (RouteData item in RouteBusiness.GetAllRouteData())
            {
                dictionary.TryAdd(item.RouteID, item);
            }
            return dictionary;
        }

        public override CacheData Reload(string routeID)
        {
            return RouteBusiness.GetRouteData(routeID);
        }
    }
}