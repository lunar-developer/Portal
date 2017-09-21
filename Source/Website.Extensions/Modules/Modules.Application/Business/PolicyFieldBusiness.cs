using System.Collections.Generic;
using Modules.Application.DataAccess;
using Modules.Application.DataTransfer;

namespace Modules.Application.Business
{
    public static class PolicyFieldBusiness
    {
        public static List<PolicyFieldData> GetAllPolicyField()
        {
            return new PolicyFieldProvider().GetAllPolicyField();
        }

        public static PolicyFieldData GetPolicyField(string policyCode, string fieldName)
        {
            return new PolicyFieldProvider().GetPolicyField(policyCode, fieldName);
        }

        public static string GetCacheKey(string policyCode, string fieldName)
        {
            return $"{policyCode}-{fieldName}";
        }
    }
}