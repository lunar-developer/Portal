using System.Collections.Generic;
using System.Data;
using System.Linq;
using Modules.Application.Database;
using Website.Library.DataTransfer;
using Website.Library.Global;

namespace Modules.Application.DataTransfer
{
    public class UserPhaseData : CacheData
    {
        public int PhaseID { get; set; }
        public int UserID { get; set; }
        public int KPI { get; set; }
        public bool IsDisable { get; set; }

        public string PolicyCode => string.Join(",", PolicyList);
        private readonly List<string> PolicyList;

        public UserPhaseData(DataRow row)
        {
            PhaseID = int.Parse(row[UserPhaseTable.PhaseID].ToString());
            UserID = int.Parse(row[UserPhaseTable.UserID].ToString());
            KPI = int.Parse(row[UserPhaseTable.KPI].ToString());
            IsDisable = FunctionBase.ConvertToBool(row[UserPhaseTable.IsDisable].ToString());
            PolicyList = row[UserPhaseTable.PolicyCode].ToString().Split(',').ToList();
        }

        public List<string> GetListPolicyCode()
        {
            return PolicyList.ToList();
        }
    }
}