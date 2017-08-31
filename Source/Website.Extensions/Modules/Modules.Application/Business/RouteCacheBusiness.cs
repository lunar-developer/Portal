using System;
using Modules.Application.DataTransfer;
using Website.Library.DataTransfer;
using Website.Library.Extension;
using Website.Library.Interface;

namespace Modules.Application.Business
{
    public class RouteCacheBusiness<T> : ICache where T : RouteData
    {
        public Type GetCacheType()
        {
            return typeof(T);
        }

        public OrderedConcurrentDictionary<string, CacheData> Load()
        {
            OrderedConcurrentDictionary<string, CacheData> dictionary =
                new OrderedConcurrentDictionary<string, CacheData>();
            foreach (RouteData item in RouteBusiness.GetAllRouteData())
            {
                dictionary.TryAdd(item.RouteID, item);
            }
            return dictionary;
        }

        public CacheData Reload(string routeID)
        {
            return RouteBusiness.GetRouteData(routeID);
        }
    }
}