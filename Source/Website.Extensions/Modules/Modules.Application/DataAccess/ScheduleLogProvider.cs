using System.Collections.Generic;
using System.Data;
using Modules.Application.Database;
using Modules.Application.DataTransfer;
using Website.Library.DataAccess;

namespace Modules.Application.DataAccess
{
    public class ScheduleLogProvider : DataProvider
    {
        private readonly string QueryScript = $@"
            SELECT * FROM dbo.{ScheduleLogTable.TableName} with(nolock)
            Where {ScheduleLogTable.ScheduleCode} = @{ScheduleLogTable.ScheduleCode}
            Order by {ScheduleLogTable.CreateDateTime} desc";

        public List<ScheduleLogData> GetList(string scheduleCode)
        {
            Connector.AddParameter(ScheduleLogTable.ScheduleCode, SqlDbType.VarChar, scheduleCode);
            Connector.ExecuteSql<ScheduleLogData, List<ScheduleLogData>>(QueryScript, out List<ScheduleLogData> outList);
            return outList;
        }

        private readonly string DeleteScript = $@"
            delete from dbo.{ScheduleLogTable.TableName}
            where {ScheduleLogTable.ScheduleCode} = @{ScheduleLogTable.ScheduleCode}";

        public bool Purge(string scheduleCode)
        {
            Connector.AddParameter(ScheduleLogTable.ScheduleCode, SqlDbType.VarChar, scheduleCode);
            Connector.ExecuteSql(DeleteScript);
            return true;
        }
    }
}