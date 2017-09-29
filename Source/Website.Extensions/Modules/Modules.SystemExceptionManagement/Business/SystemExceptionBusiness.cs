using Modules.SystemExceptionManagement.DataAccess;
using System.Collections.Generic;
using System.Data;
using Website.Library.DataTransfer;

namespace Modules.SystemExceptionManagement.Business
{
    public static class SystemExceptionBusiness
    {
        public static DataTable SearchError(Dictionary<string, SQLParameterData> parameterDictionary)
        {
            return new SystemExceptionProvider().SearchError(parameterDictionary);
        }
        public static DataTable GetErrorCode(Dictionary<string, SQLParameterData> parameterDictionary)
        {
            return new SystemExceptionProvider().GetErrorCode(parameterDictionary);
        }
    }
}