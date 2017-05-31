using System.Collections.Generic;
using System.Data;
using Modules.UserManagement.DataAccess;
using Modules.UserManagement.DataTransfer;
using Website.Library.DataTransfer;

namespace Modules.UserManagement.Business
{
    public static class BranchBusiness
    {
        public static List<BranchData> GetAllBranchInfo()
        {
            return new BranchProvider().GetAllBranchInfo();
        }

        public static BranchData GetBranchInfo(string branchID)
        {
            return new BranchProvider().GetBranchInfo(branchID);
        }

        public static DataTable GetBranchPermission(string branchID)
        {
            return new BranchProvider().GetBranchPermission(branchID);
        }

        public static bool UpdateBranchPermission(Dictionary<string, SQLParameterData> parametediDictionary)
        {
            return new BranchProvider().UpdateBranchPermission(parametediDictionary);
        }
    }
}