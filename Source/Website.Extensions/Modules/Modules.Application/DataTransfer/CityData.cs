using Website.Library.DataTransfer;

namespace Modules.Application.DataTransfer
{
    public class CityData : CacheData
    {
        public string StateCode { get; set; }
        public string CityCode { get; set; }
        public string CityName { get; set; }
        public string IsDisable { get; set; }
    }
}