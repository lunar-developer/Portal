using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modules.VSaleKit.DataTransfer
{
    public class ApplicProcessHistoryInfo
    {
        public string UserCreate { get; set; }
        public string CreateDate { get; set; }
        public string ProcessId { get; set; }
        public string Status { get; set; }
        public string PreUserProcess { get; set; }
        public string PreStatus { get; set; }
        public string Description { get; set; }
        public string DisplayName { get; set; }
        public string ActionDesc { get; set; }
    }
}
