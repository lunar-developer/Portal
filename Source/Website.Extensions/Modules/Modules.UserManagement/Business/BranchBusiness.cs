﻿using System.Collections.Generic;
using System.Data;
using Modules.UserManagement.DataAccess;
using Modules.UserManagement.Database;
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

        public static DataSet GetBranchPermissionAndTemplate(string branchID)
        {
            return new BranchProvider().GetBranchPermissionAndTemplate(branchID);
        }

        public static bool UpdateBranchPermission(Dictionary<string, SQLParameterData> parametediDictionary)
        {
            return new BranchProvider().UpdateBranchPermission(parametediDictionary);
        }

        public static DataTable GetBranchManager(string branchID = null)
        {
            return new BranchProvider().GetBranchManager(branchID);
        }


        public static string GetBranchName(string branchID)
        {
            BranchData branchInfo = CacheBase.Receive<BranchData>(branchID);
            return branchInfo == null
                ? branchID
                : GetBranchName(branchInfo);
        }

        public static string GetBranchNameByBranchCode(string branchCode)
        {
            BranchData branchInfo = CacheBase.Find<BranchData>(BranchTable.BranchCode, branchCode.Trim());
            return branchInfo == null
                ? branchCode
                : GetBranchName(branchInfo);
        }

        private static string GetBranchName(BranchData branchInfo)
        {
            return string.IsNullOrWhiteSpace(branchInfo.BranchCode)
                ? branchInfo.BranchName
                : $"{branchInfo.BranchCode} - {branchInfo.BranchName}";
        }

        public static UserData GetUserManager(string branchID)
        {
            BranchManagerData cacheData = CacheBase.Receive<BranchManagerData>(branchID);
            if (cacheData == null || cacheData.ListManager.Count == 0)
            {
                return null;
            }

            string userID = cacheData.ListManager[0];
            return CacheBase.Receive<UserData>(userID);
        }

        public static string GetManagerName(string branchID)
        {
            UserData manager = GetUserManager(branchID);
            return manager == null ? string.Empty : FunctionBase.FormatUserID(manager.UserID);
        }
    }
}