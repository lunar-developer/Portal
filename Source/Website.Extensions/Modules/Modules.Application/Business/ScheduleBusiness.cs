using System.Collections.Generic;
using Modules.Application.DataAccess;
using Modules.Application.DataTransfer;

namespace Modules.Application.Business
{
    public static class ScheduleBusiness
    {
        public static List<ScheduleData> GetAllScheduleData()
        {
            return new ScheduleProvider().GetAllScheduleData();
        }

        public static ScheduleData GetScheduleData(string scheduleName)
        {
            return new ScheduleProvider().GetScheduleData(scheduleName);
        }
    }
}