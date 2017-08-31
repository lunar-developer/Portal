using Website.Library.DataTransfer;

namespace Modules.Application.DataTransfer
{
    public class PolicyDocumentData : CacheData
    {
        public string PolicyID { get; set; }
        public string DocumentTypeID { get; set; }
        public string IsRequire { get; set; }
        public string OrderNo { get; set; }
        public string IsDisable { get; set; }
    }
}