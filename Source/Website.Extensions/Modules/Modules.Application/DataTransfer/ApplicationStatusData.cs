using Website.Library.DataTransfer;

namespace Modules.Application.DataTransfer
{
    public class ApplicationStatusData : CacheData
    {
        public string ApplicationStatusID { get; set; }
        public string StatusCode { get; set; }
        public string Name { get; set; }
    }
}