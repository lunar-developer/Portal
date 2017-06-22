using System.Collections.Generic;
using Website.Library.DataTransfer;

namespace Modules.UserManagement.DataTransfer
{
    public class BranchManagerData : CacheData
    {
        public string BranchID { get; set; }

        public List<string> ListManager = new List<string>();
    }
}