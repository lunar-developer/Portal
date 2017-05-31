using System.Collections.Generic;
using Modules.Application.DataAccess;
using Modules.Application.DataTransfer;

namespace Modules.Application.Business
{
    public static class ApplicationTypeBusiness
    {
        public static List<ApplicationTypeData> GetAllApplicationType()
        {
            return new ApplicationTypeProvider().GetAllApplicationType();
        }

        public static ApplicationTypeData GetApplicationType(string applicationTypeID)
        {
            return new ApplicationTypeProvider().GetApplicationType(applicationTypeID);
        }
    }
}