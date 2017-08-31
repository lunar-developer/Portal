using System.Collections.Generic;
using Modules.Application.DataAccess;
using Modules.Application.DataTransfer;

namespace Modules.Application.Business
{
    public class ScheduleLogBusiness
    {
        public static List<ScheduleLogData> GetList(string scheduleCode)
        {
            return new ScheduleLogProvider().GetList(scheduleCode);
        }

        public static bool Purge(string scheduleCode)
        {
            return new ScheduleLogProvider().Purge(scheduleCode);
        }
    }
}