using System.Collections.Generic;
using System.Data;
using Modules.Application.Database;
using Modules.Application.DataTransfer;
using Website.Library.DataAccess;

namespace Modules.Application.DataAccess
{
    public class ProcessProvider : DataProvider
    {
        public List<ProcessData> GetAllProcessData()
        {
            Connector.ExecuteProcedure<ProcessData, List<ProcessData>>("dbo.APP_SP_GetProcess", out List<ProcessData> list);
            return list;
        }

        public ProcessData GetProcesseData(string processID)
        {
            Connector.AddParameter(ProcessTable.ProcessID, SqlDbType.Int, processID);
            Connector.ExecuteProcedure("dbo.APP_SP_GetPhase", out ProcessData result);
            return result;
        }
    }
}