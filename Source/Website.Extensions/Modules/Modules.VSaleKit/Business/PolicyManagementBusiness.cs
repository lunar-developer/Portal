using System.Collections.Generic;
using System.Data;
using Modules.VSaleKit.DataAccess;
using Modules.Application.DataTransfer;
namespace Modules.VSaleKit.Business
{
    public static class PolicyManagementBusiness
    {
        public static DataTable GetPolicySearch(int policyID)
        {
            return new PolicyManagementProvider().GetPolicySearch(policyID);
        }
        public static DataTable GetPolicyDocument(int policyID)
        {
            return new PolicyManagementProvider().GetPolicyDocument(policyID);
        }
        public static DataTable InsertPolicy(PolicyData policyData, List<string> listKeywork, Dictionary<string,string> dicDocument)
        {
            return new PolicyManagementProvider().InsertPolicy(policyData, listKeywork, dicDocument);
        }
        public static DataTable UpdatePolicy(PolicyData policyData, List<string> listKeywork, Dictionary<string, string> dicDocument)
        {
            return new PolicyManagementProvider().UpdatePolicy(policyData, listKeywork, dicDocument);
        }
        public static DataTable RemovePolicy(int policyID, int modifyUserID, long modifyDateTime)
        {
            return new PolicyManagementProvider().RemovePolicy(policyID, modifyUserID, modifyDateTime);
        }
    }
}