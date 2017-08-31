using System;
using System.Data;
using Modules.Disbursement.DataTransfer;
using Website.Library.DataAccess;

namespace Modules.Disbursement.DataAccess
{
    public class DisbursementRoomProvider : DataProvider
    {
        public string Update(DisbursementRoomData message)
        {
            const string sql = @"INSERT INTO dbo.DB_Room (ID, RateLDR, Room, Rate, CreatedBy,
                            CreatedAt) VALUES (@id, @rateLdr, @room, @rate, @createdBy, @createdAt);";

            Connector.AddParameter("id", SqlDbType.BigInt, long.Parse(message.ID));
            Connector.AddParameter("rateLdr", SqlDbType.VarChar, message.RateLdr);
            Connector.AddParameter("room", SqlDbType.NVarChar, message.Room);
            Connector.AddParameter("rate", SqlDbType.Float, message.Rate);
            Connector.AddParameter("createdBy", SqlDbType.NVarChar, message.CreatedBy);
            Connector.AddParameter("createdAt", SqlDbType.VarChar, message.CreatedAt);

            string value;
            try
            {
                Connector.ExecuteSql(sql);
                value = "";
            }
            catch (Exception e)
            {
                value = e.ToString();
            }
            return value;
        }

        public DataTable GetTopOne()
        {
            const string sql = "SELECT Top 1 Cast(RateLDR as decimal) as RateLDR, Room, Rate, CreatedBy, CreatedAt FROM dbo.DB_Room ORDER BY ID DESC";
            Connector.ExecuteSql(sql, out DataTable result);
            return result;
        }

        public DataTable GetTop500RecentChanges()
        {
            const string sql = "SELECT Top 500 Cast(RateLDR as decimal) as RateLDR, Room, Rate, CreatedBy, CreatedAt FROM dbo.DB_Room ORDER BY ID DESC";
            Connector.ExecuteSql(sql, out DataTable result);
            return result;
        }
    }
}
