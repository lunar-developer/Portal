using System;

namespace Modules.VSaleKit.DataTransfer
{
    [Serializable]
    public class BatchExceptionData
    {
        public string LineNumber { get; set; }
        public string FieldName { get; set; }
        public string Error { get; set; }
    }
}