using Website.Library.DataTransfer;

namespace Modules.Application.DataTransfer
{
    public class LanguageData : CacheData
    {
        public string LanguageID { get; set; }
        public string Name { get; set; }
        public string IsDisable { get; set; }
    }
}