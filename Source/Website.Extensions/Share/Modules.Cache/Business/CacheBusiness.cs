using Modules.Application.Business;
using Modules.Application.DataTransfer;
using Modules.EmployeeManagement.Business;
using Modules.EmployeeManagement.DataTransfer;
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

            // Message Queue
            MessageQueueBusiness.Initialize();

            // Caches
            // User Management
            CacheBase.Inject(new UserCacheBusiness<UserData>());
            CacheBase.Inject(new BranchCacheBusiness<BranchData>());
            CacheBase.Inject(new BranchManagerCacheBusiness<BranchManagerData>());

            // Employee
            CacheBase.Inject(new EmployeeBranchCacheBusiness<EmployeeBranchData>());

            // Application
            CacheBase.Inject(new ApplicationFieldCacheBusiness<ApplicationFieldData>());
            CacheBase.Inject(new ApplicationTypeCacheBusiness<ApplicationTypeData>());
            CacheBase.Inject(new IdentityTypeCacheBusiness<IdentityTypeData>());
            CacheBase.Inject(new LanguageCacheBusiness<LanguageData>());
            CacheBase.Inject(new CountryCacheBusiness<CountryData>());
            CacheBase.Inject(new StateCacheBusiness<StateData>());
            CacheBase.Inject(new CityCacheBusiness<CityData>());
            CacheBase.Inject(new CustomerClassCacheBusiness<CustomerClassData>());
            CacheBase.Inject(new PolicyCacheBusiness<PolicyData>());
            CacheBase.Inject(new CompanyCacheBusiness<CompanyData>());
            CacheBase.Inject(new ContractTypeCacheBusiness<ContractTypeData>());
            CacheBase.Inject(new CorporateEntityTypeCacheBusiness<CorporateEntityTypeData>());
            CacheBase.Inject(new CorporateSizeCacheBusiness<CorporateSizeData>());
            CacheBase.Inject(new CorporateStatusCacheBusiness<CorporateStatusData>());
            CacheBase.Inject(new OccupationCacheBusiness<OccupationData>());
            CacheBase.Inject(new SICCacheBusiness<SICData>());
            CacheBase.Inject(new MaritalStatusCacheBusiness<MaritalStatusData>());
            CacheBase.Inject(new HomeOwnershipCacheBussiness<HomeOwnershipData>());
            CacheBase.Inject(new PositionCacheBusiness<PositionData>());
            CacheBase.Inject(new EducationCacheBusiness<EducationData>());
            CacheBase.Inject(new DecisionReasonCacheBusiness<DecisionReasonData>());
            CacheBase.Inject(new IncompleteReasonCacheBusiness<IncompleteReasonData>());
            CacheBase.Inject(new ProcessCacheBusiness<ProcessData>());
            CacheBase.Inject(new PhaseCacheBussiness<PhaseData>());
            CacheBase.Inject(new RouteCacheBusiness<RouteData>());
            CacheBase.Inject(new ScheduleCacheBusiness<ScheduleData>());
            CacheBase.Inject(new UserPhaseCacheBusiness<UserPhaseData>());
            CacheBase.Inject(new DocumentTypeCacheBusiness<DocumentTypeData>());
            CacheBase.Inject(new PolicyDocumentCacheBusiness<PolicyDocumentData>());
            CacheBase.Inject(new ApplicationTypeProcessCacheBusiness<ApplicationTypeProcessData>());
            CacheBase.Inject(new ApplicationStatusCacheBusiness<ApplicationStatusData>());
            ApplicationBusiness.Initialize();


            //CacheBase.Inject(new PhaseCacheBussiness<PhaseData>());
            //CacheBase.Inject(new PhaseMappingCacheBussiness<PhaseMappingListData>());
            //CacheBase.Inject(new UserPhaseMappingCacheBusiness<UserPhaseMappingData>());
            //CacheBase.Inject(new CountryCacheBusiness<CountryData>());
        }
    }
}