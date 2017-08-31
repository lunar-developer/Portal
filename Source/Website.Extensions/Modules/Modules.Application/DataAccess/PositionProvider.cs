using System.Collections.Generic;
using System.Data;
using Modules.Application.Database;
using Modules.Application.DataTransfer;
using Website.Library.DataAccess;

namespace Modules.Application.DataAccess
{
    public class PositionProvider : DataProvider
    {
        public List<PositionData> GetList()
        {
            Connector.ExecuteSql<PositionData, List<PositionData>>("dbo.APP_SP_GetPosition", out List<PositionData> outList);
            return outList;
        }


        public PositionData GetItem(string code)
        {
            Connector.AddParameter(PositionTable.PositionCode, SqlDbType.VarChar, code);
            Connector.ExecuteSql("dbo.APP_SP_GetPosition", out PositionData result);
            return result;
        }
    }
}
