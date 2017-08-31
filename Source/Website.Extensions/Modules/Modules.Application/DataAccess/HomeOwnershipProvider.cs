using System.Collections.Generic;
using System.Data;
using Modules.Application.Database;
using Modules.Application.DataTransfer;
using Website.Library.DataAccess;

namespace Modules.Application.DataAccess
{
    public class HomeOwnershipProvider:DataProvider
    {
        public List<HomeOwnershipData> GetList()
        {
            Connector.ExecuteSql<HomeOwnershipData, List<HomeOwnershipData>>(
                "dbo.APP_SP_GetHomeOwnership", out List<HomeOwnershipData> outList);
            return outList;
        }


        public HomeOwnershipData GetItem(string code)
        {
            Connector.AddParameter(HomeOwnershipTable.HomeOwnershipCode, SqlDbType.VarChar, code);
            Connector.ExecuteSql("dbo.APP_SP_GetHomeOwnership", out HomeOwnershipData result);
            return result;
        }
    }
}