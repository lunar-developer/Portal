using System.Collections.Generic;
using System.Data;
using Modules.Disbursement.DataAccess;
using Modules.Disbursement.DataTransfer;
using Website.Library.DataTransfer;
using Website.Library.Global;

namespace Modules.Disbursement.Business
{
    public static class DisbursementBusiness
    {
        public static DataSet GetDisbursementByID(string id)
        {
            return new DisbursementProvider().GetDisbursementByID(id);
        }

        public static long InsertNewRecord(DisbursementData data)
        {
            DataTable dtResult = new DisbursementProvider().InsertNewRecord(data);
            long result = long.Parse(dtResult.Rows[0][0].ToString());
            if (result == -1)
            {
                FunctionBase.LogError(dtResult.Rows[0][1].ToString());
            }
            return result;
        }

        public static bool UpdateRecord(DisbursementData data)
        {
            DataTable dtResult = new DisbursementProvider().UpdateRecord(data);
            bool result = dtResult.Rows[0][0].ToString() == "1";
            if (result == false)
            {
                FunctionBase.LogError(dtResult.Rows[0][1].ToString());
            }
            return result;
        }

        public static bool DeleteRecord(string id)
        {
            return new DisbursementProvider().DeleteRecord(id);
        }

        public static DataTable QueryDisbursement(Dictionary<string, SQLParameterData> parameterDictionary)
        {
            return new DisbursementProvider().QueryDisbursement(parameterDictionary);
        }

        public static int ProcessDisbursement(Dictionary<string, SQLParameterData> parameterDictionary)
        {
            DataTable dtResult = new DisbursementProvider().ProcessDisbursement(parameterDictionary);
            int result = int.Parse(dtResult.Rows[0][0].ToString());
            if (result == -1)
            {
                FunctionBase.LogError(dtResult.Rows[0][1].ToString());
            }
            return result;
        }

        public static int ProcessCancel(Dictionary<string, SQLParameterData> parameterDictionary)
        {
            DataTable dtResult = new DisbursementProvider().ProcessCancel(parameterDictionary);
            int result = int.Parse(dtResult.Rows[0][0].ToString());
            if (result == -1)
            {
                FunctionBase.LogError(dtResult.Rows[0][1].ToString());
            }
            return result;
        }
    }
}