using System.Collections.Generic;
using Modules.Application.DataAccess;
using Modules.Application.DataTransfer;

namespace Modules.Application.Business
{
    public static class LanguageBusiness
    {
        public static List<LanguageData> GetAllLanguage()
        {
            return new LanguageProvider().GetAllLanguage();
        }

        public static LanguageData GetLanguage(string countryCode)
        {
            return new LanguageProvider().GetLanguage(countryCode);
        }
    }
}