using System.Collections.Generic;
using Modules.Application.Database;
using Modules.Application.DataTransfer;
using Website.Library.DataAccess;

namespace Modules.Application.DataAccess
{
    public class UserApplicationProvider: DataProvider
    {
        public List<UserApplicationData> GetUserApplic()
        {
            List<UserApplicationData> outList;
            Connector.ExecuteProcedure<UserApplicationData, List<UserApplicationData>>(UserApplicationTable.StoredProcedure, out outList);
            return outList;
        }
    }
}
