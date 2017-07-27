using System.Collections.Generic;
using System.Data;
using System.Linq;
using Modules.Application.Database;
using Website.Library.DataTransfer;

namespace Modules.Application.DataTransfer
{
    public class UserPhaseData : CacheData
    {
        public int UserID { get; set; }
        public int PhaseID { get; set; }
        public int KPI { get; set; }

        public string PolicyCode => string.Join(",", PolicyList);
        private readonly List<string> PolicyList;

        public UserPhaseData(DataRow row)
        {
            UserID = int.Parse(row[UserPhaseTable.UserID].ToString());
            PhaseID = int.Parse(row[UserPhaseTable.PhaseID].ToString());
            KPI = int.Parse(row[UserPhaseTable.KPI].ToString());
            PolicyList = row[UserPhaseTable.PolicyCode].ToString().Split(',').ToList();
        }
    }
}