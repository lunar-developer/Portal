using Website.Library.DataTransfer;

namespace Modules.Application.DataTransfer
{
    public class DecisionReasonData : CacheData
    {
        public string DecisionReasonCode { get; set; }
        public string DecisionReasonType { get; set; }
        public string HiddenRemark { get; set; }
        public string Remark { get; set; }
        public string IsDisable { get; set; }
    }
}