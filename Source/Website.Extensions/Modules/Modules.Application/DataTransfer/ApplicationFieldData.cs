using Website.Library.DataTransfer;

namespace Modules.Application.DataTransfer
{
    public class ApplicationFieldData : CacheData
    {
        public string FieldName { get; set; }
        public string TableName { get; set; }
        public string DataType { get; set; }
        public string IsExport { get; set; }
        public string IsBeginField { get; set; }
        public string ExportType { get; set; }
        public string ExportLength { get; set; }
        public string IsConstantField { get; set; }
        public string FieldValue { get; set; }
        public string NextFieldName { get; set; }
        public string IsDisable { get; set; }
    }
}