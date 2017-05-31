using Modules.Application.Business;
using Modules.Application.DataTransfer;
using Modules.Skins.Jango.Global;
using Modules.UserManagement.Business;
using Modules.UserManagement.DataTransfer;
using Website.Library.Business;
using Website.Library.Global;

namespace Modules.Cache.Business
{
    public class CacheBusiness
    {
        public static void Initialize()
        {
            // Bundles
            BundleBusiness.Include(JangoSkinBase.SkinBundles);
            BundleBusiness.Initialize();

            // Caches
            CacheBase.Inject(new BranchCacheBusiness<BranchData>());
            CacheBase.Inject(new UserCacheBusiness<UserData>());

            CacheBase.Inject(new ApplicationTypeCacheBusiness<ApplicationTypeData>());
            CacheBase.Inject(new IdentityTypeCacheBusiness<IdentityTypeData>());
            CacheBase.Inject(new LanguageCacheBusiness<LanguageData>());
            CacheBase.Inject(new CountryCacheBusiness<CountryData>());
            CacheBase.Inject(new CustomerClassCacheBusiness<CustomerClassData>());
            CacheBase.Inject(new PolicyCacheBusiness<PolicyData>());
            CacheBase.Inject(new ApplicationFieldCacheBusiness<ApplicationFieldData>());
            CacheBase.Inject(new StateCacheBusiness<StateData>());
            CacheBase.Inject(new CityCacheBusiness<CityData>());

            //CacheBase.Inject(new PhaseCacheBussiness<PhaseData>());
            //CacheBase.Inject(new PhaseMappingCacheBussiness<PhaseMappingListData>());
            //CacheBase.Inject(new UserPhaseMappingCacheBusiness<UserPhaseMappingData>());
            //CacheBase.Inject(new CountryCacheBusiness<CountryData>());
        }
    }
}