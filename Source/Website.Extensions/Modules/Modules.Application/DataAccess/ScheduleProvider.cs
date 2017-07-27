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

        public ScheduleData GetScheduleData(string scheduleName)
        {
            Connector.AddParameter(SheduleTable.ScheduleName, SqlDbType.VarChar, scheduleName);
            Connector.ExecuteProcedure("dbo.APP_SP_GetScheduleData", out ScheduleData result);
            return result;
        }
    }
}