using Website.Library.DataTransfer;

namespace Modules.Application.DataTransfer
{
    public class ContractTypeData : CacheData
    {
        public string ContractTypeCode { get; set; }
        public string ContractTypeName { get; set; }
        public string IsDisable { get; set; } 
    }
}
