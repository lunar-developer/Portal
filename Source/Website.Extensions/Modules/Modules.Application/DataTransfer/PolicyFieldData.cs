using Website.Library.DataTransfer;

namespace Modules.Application.DataTransfer
{
    public class PolicyFieldData : CacheData
    {
        public string PolicyCode { get; set; }
        public string FieldName { get; set; }
        public string IsRequire { get; set; }
        public string IsDisable { get; set; }

        public string CacheID => $"{PolicyCode}-{FieldName}";
    }
}