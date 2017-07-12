using Website.Library.DataTransfer;

namespace Modules.Application.DataTransfer
{
    public class PositionData : CacheData
    {
        public string PositionCode { get; set; }
        public string PositionName { get; set; }
        public string IsDisable { get; set; }
    }
}