using System.Collections.Generic;
using Modules.Application.DataAccess;
using Modules.Application.DataTransfer;
using Website.Library.Global;

namespace Modules.Application.Business
{
    public static class ApplicationTypeBusiness
    {
        public static List<ApplicationTypeData> GetAllApplicationTypes()
        {
            return new ApplicationTypeProvider().GetAllApplicationTypes();
        }

        public static ApplicationTypeData GetApplicationType(string applicationTypeID)
        {
            return new ApplicationTypeProvider().GetApplicationType(applicationTypeID);
        }

        public static string GetName(string applicationTypeID)
        {
            ApplicationTypeData cacheData = CacheBase.Receive<ApplicationTypeData>(applicationTypeID);
            return cacheData?.Name ?? applicationTypeID;
        }
    }
}