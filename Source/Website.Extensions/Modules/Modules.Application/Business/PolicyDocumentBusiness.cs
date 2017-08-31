using System.Collections.Generic;
using Modules.Application.DataAccess;
using Modules.Application.DataTransfer;

namespace Modules.Application.Business
{
    public static class PolicyDocumentBusiness
    {
        public static List<PolicyDocumentData> GetAllDocumentType()
        {
            return new PolicyDocumentProvider().GetAllDocumentType();
        }

        public static PolicyDocumentData GetDocumentType(string policyID, string documentTypeID)
        {
            return new PolicyDocumentProvider().GetDocumentType(policyID, documentTypeID);
        }

        public static string GetCacheKey(string policyID, string documentTypeID)
        {
            return $"{policyID}-{documentTypeID}";
        }
    }
}