using System.Collections.Generic;
using System.Data;
using Modules.Application.DataAccess;
using Modules.Application.DataTransfer;

namespace Modules.Application.Business
{
    public static class AbsentCalendarBusiness
    {
        public static List<AbsentCalendarData> GetListAbsentCalendar(string userID)
        {
            return new AbsentCalendarProvider().GetListAbsentCalendar(userID);
        }
        public static DataTable GetAbsentDataTable(string userID)
        {
            return new AbsentCalendarProvider().GetAbsentDataTable(userID);
        }
        public static bool DeleteAbsentCalendar(Dictionary<string, string> dictionary, out string message)
        {
            DataTable dtResult = new AbsentCalendarProvider().DeleteAbsentCalendar(dictionary);
            message = dtResult.Rows[0][1].ToString();
            return dtResult.Rows[0][0].ToString() == "1";
        }
        public static bool InsertAbsentCalendar(Dictionary<string, string> dictionary, out string message)
        {
            DataTable dtResult = new AbsentCalendarProvider().InsertAbsentCalendar(dictionary);
            message = dtResult.Rows[0][1].ToString();
            return dtResult.Rows[0][0].ToString() == "1";
        }
    }
}
