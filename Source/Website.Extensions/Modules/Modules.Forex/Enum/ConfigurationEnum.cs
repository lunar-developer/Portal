namespace Modules.Forex.Enum
{
    public class ConfigurationEnum
    {
        public const string MaxAmountChangePercent = "FX_MaxAmountChangePercent";
        public const string MaxAmountChange = "FX_MaxAmountChange";

        public const string DealerMaxEditLimit = "FX_DealerMaxEditLimit";
        public const string DealerMaxCancelLimit = "FX_DealerMaxCancelLimit";

        public const string DepositPercent = "FX_DepositPercent";

        public const string TransactionCreationControlKey = "TransactionCreationControlKey";
        public const string ExchangeRateCreationControlKey = "ExchangeRateCreationControlKey";
        public const string HistoryControlKey = "transactionHistory";
        public const string TransactionManagementControlKey = "transactionManagementView";
        public const string BidManagementControlKey = "transactionManagementView";

        public const string WorkingBeginMorning = "FX_WorkingBeginMorning";
        public const string WorkingEndMorning = "FX_WorkingEndMorning";
        public const string WorkingBeginAfternoon = "FX_WorkingBeginAfternoon";
        public const string WorkingEndAfternoon = "FX_WorkingEndAfternoon";
        public const string WorkingBeginSaturday = "FX_WorkingBeginSaturday";
        public const string WorkingEndSaturday = "FX_WorkingEndSaturday";

        public const string ReloadBrInboxTime = "FX_ReloadBrInboxTime";
        public const string ReloadHOInboxTime = "FX_ReloadHOInboxTime";
    }
}
