using Website.Library.DataTransfer;

namespace Modules.Application.DataTransfer
{
    public class SICData : CacheData
    {
        public string SICCode { get; set; }
        public string SICName { get; set; }
        public string IsDisable { get; set; }
    }
}