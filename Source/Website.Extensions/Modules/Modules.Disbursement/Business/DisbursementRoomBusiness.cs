using System.Collections.Generic;
using System.Data;
using Modules.Disbursement.DataAccess;
using Modules.Disbursement.DataTransfer;
using Website.Library.DataTransfer;
using Website.Library.Global;

namespace Modules.Disbursement.Business
{
    public static class DisbursementRoomBusiness
    {
        public static string Update(DisbursementRoomData roomData)
        {
            return new DisbursementRoomProvider().Update(roomData);
        }

        public static DataTable GetTop500RecentChanges() {
            return new DisbursementRoomProvider().GetTop500RecentChanges();
        }
    }
}
