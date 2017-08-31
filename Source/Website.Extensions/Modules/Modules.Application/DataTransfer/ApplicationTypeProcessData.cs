using Website.Library.DataTransfer;

namespace Modules.Application.DataTransfer
{
    public class ApplicationTypeProcessData : CacheData
    {
        public string ApplicationTypeID { get; set; }
        public string ProcessID { get; set; }
    }
}