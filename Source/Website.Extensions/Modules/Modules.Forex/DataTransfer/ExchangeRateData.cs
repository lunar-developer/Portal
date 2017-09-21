using Website.Library.DataTransfer;

namespace Modules.Forex.DataTransfer
{
    public class ExchangeRateData:CacheData
    {
        public string TransactionTypeID;
        public string CurrencyCode;
        public string Rate;
        public string RateStatus;
        public string DealTime;
        public string IsDisable;
        public string Remark;
    }
}
