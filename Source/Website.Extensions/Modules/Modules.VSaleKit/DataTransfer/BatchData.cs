using System;

namespace Modules.VSaleKit.DataTransfer
{
    [Serializable]
    public class BatchData
    {
        public string LegalType { get; set; }
        public string CustomerID { get; set; }
        public string CustomerName { get; set; }
        public string CreditLimit { get; set; }
        public string Priority { get; set; }
        public string UserName { get; set; }
        public string Description { get; set; }
        public string ProcessCode { get; set; } = string.Empty;
    }
}