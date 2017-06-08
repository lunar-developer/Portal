using System.Collections.Generic;
using System.Data;
using Modules.UserManagement.DataAccess;
using Modules.UserManagement.DataTransfer;
using Website.Library.DataTransfer;
using Website.Library.Global;

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


        public static string GetBranchName(string branchID)
        {
            BranchData branchInfo = CacheBase.Receive<BranchData>(branchID);
            return branchInfo == null
                ? branchID
                : GetBranchName(branchInfo);
        }

        private static string GetBranchName(BranchData branchInfo)
        {
            return string.IsNullOrWhiteSpace(branchInfo.BranchCode)
                ? branchInfo.BranchName
                : $"{branchInfo.BranchCode} - {branchInfo.BranchName}";
        }
    }
}