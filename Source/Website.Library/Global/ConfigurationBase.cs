using Website.Library.Enum;

namespace Website.Library.Global
{
    public static class ConfigurationBase
    {
        public static int SystemUserID =
            FunctionBase.ConvertToInteger(FunctionBase.GetConfiguration(ConfigEnum.SystemUserID), 10);
        public static string SystemUserName =
            FunctionBase.GetConfiguration(ConfigEnum.SystemUserName, "system@email.com");

        public static string QueuePortalServiceIn = FunctionBase.GetConfiguration(ConfigEnum.QueuePortalServiceIn);
        public static string QueuePortalServiceOut = FunctionBase.GetConfiguration(ConfigEnum.QueuePortalServiceOut);
        public static string QueuePortalIn = FunctionBase.GetConfiguration(ConfigEnum.QueuePortalIn);
    }
}