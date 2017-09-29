using System.Collections.Generic;
using System.Data;
using Website.Library.DataAccess;
using Website.Library.DataTransfer;

namespace Modules.SystemExceptionManagement.DataAccess

{
    public class SystemExceptionProvider : DataProvider
    {
        public DataTable SearchError(Dictionary<string, SQLParameterData> parameterDictionary)
        {
            foreach (KeyValuePair<string, SQLParameterData> pair in parameterDictionary)
            {
                Connector.AddParameter(pair.Key, pair.Value.ParameterType, pair.Value.ParameterValue);
            }
            Connector.ExecuteProcedure("SE_SP_SearchError", out DataTable dtResult);
            return dtResult;
        }
        public DataTable GetErrorCode(Dictionary<string, SQLParameterData> parameterDictionary)
        {
            foreach (KeyValuePair<string, SQLParameterData> pair in parameterDictionary)
            {
                Connector.AddParameter(pair.Key, pair.Value.ParameterType, pair.Value.ParameterValue);
            }
            Connector.ExecuteProcedure("SE_SP_GetErrorCode", out DataTable dtResult);
            return dtResult;
        }
    }
}