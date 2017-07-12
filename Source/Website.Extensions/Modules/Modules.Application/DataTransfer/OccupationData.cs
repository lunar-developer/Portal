using Website.Library.DataTransfer;

namespace Modules.Application.DataTransfer
{
    public class OccupationData : CacheData
    {
        public string OccupationCode { get; set; }
        public string OccupationName { get; set; }
        public string IsDisable { get; set; }
    }
}
