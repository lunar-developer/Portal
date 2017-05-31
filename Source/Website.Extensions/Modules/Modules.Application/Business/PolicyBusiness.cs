using System.Collections.Generic;
using Modules.Application.DataAccess;
using Modules.Application.DataTransfer;

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
    }
}