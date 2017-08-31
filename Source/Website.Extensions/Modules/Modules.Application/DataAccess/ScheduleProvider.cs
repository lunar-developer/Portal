using System.Collections.Generic;
using System.Data;
using Modules.Application.Database;
using Modules.Application.DataTransfer;
using Website.Library.DataAccess;

namespace Modules.Application.DataAccess
{
    public class ScheduleProvider : DataProvider
    {
        public List<ScheduleData> GetAllScheduleData()
        {
            Connector.ExecuteProcedure<ScheduleData, List<ScheduleData>>("dbo.APP_SP_GetScheduleData", out List<ScheduleData> result);
            return result;
        }

        public ScheduleData GetScheduleData(string scheduleCode)
        {
            Connector.AddParameter(SheduleTable.ScheduleCode, SqlDbType.VarChar, scheduleCode);
            Connector.ExecuteProcedure("dbo.APP_SP_GetScheduleData", out ScheduleData result);
            return result;
        }
    }
}