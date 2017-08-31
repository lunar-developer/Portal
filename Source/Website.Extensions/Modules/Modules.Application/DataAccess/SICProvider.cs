using System.Collections.Generic;
using System.Data;
using Modules.Application.Database;
using Modules.Application.DataTransfer;
using Website.Library.DataAccess;

namespace Modules.Application.DataAccess
{
    public class SICProvider : DataProvider
    {
        public List<SICData> GetList()
        {
            Connector.ExecuteSql<SICData, List<SICData>>("dbo.APP_SP_GetSIC", out List<SICData> outList);
            return outList;
        }


        public SICData GetItem(string code)
        {
            Connector.AddParameter(SICTable.SICCode, SqlDbType.VarChar, code);
            Connector.ExecuteSql("dbo.APP_SP_GetSIC", out SICData result);
            return result;
        }
    }
}