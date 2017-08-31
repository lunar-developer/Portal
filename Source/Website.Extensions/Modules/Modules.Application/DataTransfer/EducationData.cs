using Website.Library.DataTransfer;

namespace Modules.Application.DataTransfer
{
    public class EducationData : CacheData
    {
        public string EducationCode { get; set; }
        public string EducationName { get; set; }
        public string SortOrder { get; set; }
        public string IsDisable { get; set; }
    }
}