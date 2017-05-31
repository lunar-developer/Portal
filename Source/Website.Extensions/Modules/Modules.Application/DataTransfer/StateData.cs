using Website.Library.DataTransfer;

namespace Modules.Application.DataTransfer
{
    public class StateData : CacheData
    {
        public string CountryCode { get; set; }
        public string StateCode { get; set; }
        public string StateName { get; set; }
        public string IsDisable { get; set; }
    }
}