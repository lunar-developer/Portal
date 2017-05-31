using System.Collections.Generic;
using System.Data;
using Modules.Application.Database;
using Modules.Application.DataTransfer;
using Website.Library.DataAccess;

namespace Modules.Application.DataAccess
{
    public class AbsentCalendarProvider : DataProvider
    {
        public List<AbsentCalendarData> GetListAbsentCalendar(string userID)
        {
            List<AbsentCalendarData> outList;
            Connector.AddParameter(AbsentCalendarTable.UserID, SqlDbType.VarChar, userID);
            Connector.ExecuteProcedure<AbsentCalendarData, List<AbsentCalendarData>>(AbsentCalendarTable.StoredProcedure, out outList);
            return outList;
        }

        public DataTable GetAbsentDataTable(string userID)
        {
            DataTable outList;
            Connector.AddParameter(AbsentCalendarTable.UserID, SqlDbType.VarChar, userID);
            Connector.ExecuteProcedure(AbsentCalendarTable.StoredProcedure, out outList);
            return outList;
        }

        public DataTable DeleteAbsentCalendar(Dictionary<string, string> dictionary)
        {
            DataTable dtResult;
            foreach (KeyValuePair<string, string> pair in dictionary)
            {
                Connector.AddParameter(pair.Key, SqlDbType.VarChar, pair.Value);
            }
            Connector.ExecuteProcedure(AbsentCalendarTable.DeleteStoredProcedure, out dtResult);
            return dtResult;
        }
        public DataTable InsertAbsentCalendar(Dictionary<string, string> dictionary)
        {
            DataTable dtResult;
            foreach (KeyValuePair<string, string> pair in dictionary)
            {
                Connector.AddParameter(pair.Key, SqlDbType.VarChar, pair.Value);
            }
            Connector.ExecuteProcedure(AbsentCalendarTable.InsertStoredProcedure, out dtResult);
            return dtResult;
        }
    }
}
