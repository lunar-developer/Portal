using Website.Library.DataTransfer;

namespace Modules.Application.DataTransfer
{
    public class IncompleteReasonData : CacheData
    {
        public string IncompleteReasonCode { get; set; }
        public string IncompleteReasonName { get; set; }
        public string IsDisable { get; set; }
    }
}