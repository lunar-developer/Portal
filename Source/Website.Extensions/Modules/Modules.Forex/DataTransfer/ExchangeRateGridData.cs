using Website.Library.DataTransfer;

namespace Modules.Forex.DataTransfer
{
    public class ExchangeRateGridData:CacheData
    {
        /// <summary>
        /// CurrencyCode, Ex: USD => USD/VND
        /// </summary>
        public string CurrencyCode { get; set; }
        public string BuyRateFT { get; set; }
        /// <summary>
        /// Down | Up | Normal
        /// </summary>
        public string BuyRateFTStatus { get; set; }
        public string DealTimeBuyFT { get; set; }
        public string IsDisableBuyFT { get; set; }
        public string SellRateFT { get; set; }
        /// <summary>
        /// Down | Up | Normal
        /// </summary>
        public string SellRateFTStatus { get; set; }
        public string DealTimeSellFT { get; set; }
        public string IsDisableSellFT { get; set; }
        public string BuyRateCash { get; set; }
        /// <summary>
        /// Down | Up | Normal
        /// </summary>
        public string BuyRateCashStatus { get; set; }
        public string DealTimeBuyCash { get; set; }
        public string IsDisableBuyCash { get; set; }
        public string SellRateCash { get; set; }
        /// <summary>
        /// Down | Up | Normal
        /// </summary>
        public string SellRateCashStatus { get; set; }
        public string DealTimeSellCash { get; set; }
        public string IsDisableSellCash { get; set; }
    }
}
