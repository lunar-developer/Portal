using System.Collections.Generic;
using Modules.Application.DataAccess;
using Modules.Application.DataTransfer;
using Website.Library.Global;

namespace Modules.Application.Business
{
    public static class ApplicationStatusBusiness
    {
        public static List<ApplicationStatusData> GetAllApplicationStatus()
        {
            return new ApplicationStatusProvider().GetAllApplicationStatus();
        }

        public static ApplicationStatusData GetApplicationStatus(string applicationStatusID)
        {
            return new ApplicationStatusProvider().GetApplicationStatus(applicationStatusID);
        }

        public static string GetName(string applicationStatusID)
        {
            ApplicationStatusData cacheData = CacheBase.Receive<ApplicationStatusData>(applicationStatusID);
            return cacheData?.Name ?? applicationStatusID;
        }
    }
}