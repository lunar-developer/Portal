using System.Collections.Generic;
using System.Data;
using Modules.VSaleKit.DataAccess;

namespace Modules.VSaleKit.Business
{
    public static class ApplicationFormBusiness
    {
        public static DataTable SearchApplication(Dictionary<string, string> dictionary)
        {
            return new ApplicationFormProvider().SearchApplication(dictionary);
        }

        public static DataSet LoadApplication(string uniqueID, int userID)
        {
            return new ApplicationFormProvider().LoadApplication(uniqueID, userID);
        }

        public static bool ProcessApplication(string uniqueID, string userName, string roleName, string branchCode, string description)
        {
            return new ApplicationFormProvider().ProcessApplication(uniqueID, userName, roleName, branchCode, description);
        }
    }
}