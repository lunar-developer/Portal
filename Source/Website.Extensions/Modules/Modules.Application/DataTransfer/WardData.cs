using Website.Library.DataTransfer;

namespace Modules.Application.DataTransfer
{
    public class WardData : CacheData
    {
        public string WardCode { get; set; }
        public string CityCode { get; set; }
        public string WardName { get; set; }
        public string IsDisable { get; set; }
    }
}
