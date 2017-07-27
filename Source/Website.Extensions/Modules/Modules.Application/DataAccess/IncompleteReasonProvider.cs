using Modules.Application.Database;
using Modules.Application.DataTransfer;
using System.Collections.Generic;
using System.Data;
using Website.Library.DataAccess;

namespace Modules.Application.DataAccess
{
    public class IncompleteReasonProvider : DataProvider
    {
        public List<IncompleteReasonData> GetAllIncompleteReason()
        {
            Connector.ExecuteProcedure<IncompleteReasonData, List<IncompleteReasonData>>(
                IncompleteReasonTable.StoreProcedure, out List<IncompleteReasonData> list);
            return list;
        }
        
        public IncompleteReasonData GetIncompleteReason(string incompleteReasonCode)
        {
            Connector.AddParameter(IncompleteReasonTable.IncompleteReasonCode, SqlDbType.VarChar, incompleteReasonCode);
            Connector.ExecuteProcedure(IncompleteReasonTable.StoreProcedure, out IncompleteReasonData result);
            return result;
        }
    }
}