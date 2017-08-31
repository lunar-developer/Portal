using System.Collections.Generic;
using Modules.Application.DataAccess;
using Modules.Application.Database;
using Modules.Application.DataTransfer;
using Website.Library.Global;

namespace Modules.Application.Business
{
    public static class PolicyBusiness
    {
        public static List<PolicyData> GetAllPolicy()
        {
            return new PolicyProvider().GetAllPolicy();
        }

        public static PolicyData GetPolicy(string policyID)
        {
            return new PolicyProvider().GetPolicy(policyID);
        }

        public static string GetDisplayName(int policyID)
        {
            return GetDisplayName(CacheBase.Receive<PolicyData>(policyID.ToString())) ?? policyID.ToString();
        }

        public static string GetDisplayName(string policyCode)
        {
            return GetDisplayName(CacheBase.Find<PolicyData>(PolicyTable.PolicyCode, policyCode)) ?? policyCode;
        }

        public static string GetDisplayName(PolicyData policy)
        {
            return policy != null ? $"{policy.PolicyCode} - {policy.Name}" : null;
        }
    }
}