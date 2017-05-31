using Website.Library.DataTransfer;

namespace Modules.Application.DataTransfer
{
    public class CountryData : CacheData
    {
        public string CountryCode { get; set; }
        public string Name { get; set; }
        public string IsDisable { get; set; }
    }
}