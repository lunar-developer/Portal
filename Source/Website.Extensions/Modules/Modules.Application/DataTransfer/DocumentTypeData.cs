using Website.Library.DataTransfer;

namespace Modules.Application.DataTransfer
{
    public class DocumentTypeData : CacheData
    {
        public string DocumentTypeID { get; set; }
        public string DocumentCode { get; set; }
        public string Name { get; set; }
        public string Remark { get; set; }
        public string IsDisable { get; set; }
    }
}