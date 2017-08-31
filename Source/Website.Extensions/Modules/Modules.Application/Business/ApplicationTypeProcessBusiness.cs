using System.Collections.Generic;
using Modules.Application.DataAccess;
using Modules.Application.DataTransfer;

namespace Modules.Application.Business
{
    public static class ApplicationTypeProcessBusiness
    {
        public static List<ApplicationTypeProcessData> GetAllApplicationTypeProcess()
        {
            return new ApplicationTypeProcessProvider().GetAllApplicationTypeProcess();
        }

        public static ApplicationTypeProcessData GetApplicationTypeProcess(string applicationTypeID)
        {
            return new ApplicationTypeProcessProvider().GetApplicationTypeProcess(applicationTypeID);
        }
    }
}