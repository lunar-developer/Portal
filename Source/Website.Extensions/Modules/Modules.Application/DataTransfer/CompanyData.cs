using Website.Library.DataTransfer;

namespace Modules.Application.DataTransfer
{
    public class CompanyData : CacheData
    {
        public string CompanyID { get; set; }
        public string TaxCode { get; set; }
        public string FullName { get; set; }
        public string AddressLine01 { get; set; }
        public string AddressLine02 { get; set; }
        public string AddressLine03 { get; set; }
        public string CountryCode { get; set; }
        public string StateCode { get; set; }
        public string CityCode { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string IsDisable { get; set; }
    }
}
