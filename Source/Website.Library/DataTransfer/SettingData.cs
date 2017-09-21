namespace Website.Library.DataTransfer
{
    public class SettingData : CacheData
    {
        public string SettingName { get; set; }
        public string SettingValue { get; set; }
        public string DefaultValue { get; set; }
        public string ModuleName { get; set; }
        public string Remark { get; set; }
        public string IsDisable { get; set; }
    }
}