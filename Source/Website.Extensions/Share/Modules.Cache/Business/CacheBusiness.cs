using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Modules.Application.Business;
using Modules.Skins.Jango.Global;
using Website.Library.Business;
using Website.Library.Global;
using Website.Library.Interface;

namespace Modules.Cache.Business
{
    public class CacheBusiness
    {
        public static void Initialize()
        {
            // Bundles
            BundleBusiness.Include(JangoSkinBase.SkinBundles);
            BundleBusiness.Initialize();

            // Message Queue
            MessageQueueBusiness.Initialize();

            // Caches
            List<string> listAssemblyName = new List<string>
            {
                "Modules.UserManagement",
                "Modules.EmployeeManagement",
                "Modules.Application",
                "Modules.Forex"
            };
            List<Assembly> listAssembly = AppDomain.CurrentDomain.GetAssemblies()
                .Where(assembly => listAssemblyName.Contains(assembly.GetName().Name)).ToList();
            foreach (Assembly assembly in listAssembly)
            {
                List<Type> listCaches = assembly.ExportedTypes
                    .Where(type =>
                    {
                        if (type.BaseType == null || type.BaseType.IsGenericType == false)
                        {
                            return false;
                        }
                        Type genericTypeDefinition = type.BaseType.GetGenericTypeDefinition();
                        return genericTypeDefinition != null
                            && genericTypeDefinition.IsAssignableFrom(typeof(BasicCacheBusiness<>));
                    }).ToList();

                foreach (Type type in listCaches)
                {
                    Type genericType = type.MakeGenericType(type.GetGenericArguments()[0].BaseType);
                    ICache cache = (ICache) Activator.CreateInstance(genericType);
                    CacheBase.Inject(cache);
                }
            }

            // Application (Schedules & Queue Handlers)
            ApplicationBusiness.Initialize();
        }
    }
}