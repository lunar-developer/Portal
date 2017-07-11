namespace Modules.Application.Database
{
    public static class ApplicationTable
    {
        public const string TableName = "APP_Application";


        // APPLICATION INFO
        public const string UniqueID = "UniqueID";
        public const string ApplicationID = "ApplicationID";
        public const string ApplicationTypeID = "ApplicationTypeID";
        public const string Priority = "Priority";
        public const string ApplicationStatus = "ApplicationStatus";
        public const string DecisionCode = "DecisionCode";
        public const string ModifyUserID = "ModifyUserID";
        public const string ModifyDateTime = "ModifyDateTime";
        public const string ApplicationRemark = "ApplicationRemark";


        // CARD HOLDER INFO
        public const string CustomerID = "CustomerID";
        public const string IdentityTypeCode = "IdentityTypeCode";
        public const string CIFNo = "CIFNo";
        public const string CardTypeIndicator = "CardTypeIndicator";
        public const string BasicCardNumber = "BasicCardNumber";
        public const string FullName = "FullName";
        public const string EmbossName = "EmbossName";
        public const string OldCustomerID = "OldCustomerID";
        public const string InsuranceNumber = "InsuranceNumber";
        public const string Gender = "Gender";
        public const string TitleOfAddress = "TitleOfAddress";

        public const string Language = "Language";
        public const string Nationality = "Nationality";
        public const string BirthDate = "BirthDate";
        public const string Mobile01 = "Mobile01";
        public const string Mobile02 = "Mobile02";
        public const string Email01 = "Email01";
        public const string Email02 = "Email02";
        public const string CorporateCardIndicator = "CorporateCardIndicator";
        public const string CustomerType = "CustomerType";
        public const string CustomerClass = "CustomerClass";


        // CONTACT INFO
        public const string HomeAddress01 = "HomeAddress01";
        public const string HomeAddress02 = "HomeAddress02";
        public const string HomeAddress03 = "HomeAddress03";
        public const string HomeCountry = "HomeCountry";
        public const string HomeState = "HomeState";
        public const string HomeCity = "HomeCity";
        public const string HomePhone01 = "HomePhone01";
        public const string HomeRemark = "HomeRemark";

        public const string Alternative01Address01 = "Alternative01Address01";
        public const string Alternative01Address02 = "Alternative01Address02";
        public const string Alternative01Address03 = "Alternative01Address03";
        public const string Alternative01Country = "Alternative01Country";
        public const string Alternative01State = "Alternative01State";
        public const string Alternative01City = "Alternative01City";
        public const string Alternative01Phone01 = "Alternative01Phone01";
        public const string Alternative01Remark = "Alternative01Remark";

        public const string Alternative02Address01 = "Alternative02Address01";
        public const string Alternative02Address02 = "Alternative02Address02";
        public const string Alternative02Address03 = "Alternative02Address03";
        public const string Alternative02Country = "Alternative02Country";
        public const string Alternative02State = "Alternative02State";
        public const string Alternative02City = "Alternative02City";
        public const string Alternative02Phone01 = "Alternative02Phone01";
        public const string Alternative02Remark = "Alternative02Remark";


        // AUTO PAY INFO
        public const string PaymentMethod = "PaymentMethod";
        public const string PaymentSource = "PaymentSource";
        public const string PaymentCIFNo = "PaymentCIFNo";
        public const string PaymentAccountName = "PaymentAccountName";
        public const string PaymentAccountNo = "PaymentAccountNo";
        public const string PaymentBankCode = "PaymentBankCode";
        public const string AutoPayIndicator = "AutoPayIndicator";


        // FINANCE INFO
        public const string StaffIndicator = "StaffIndicator";
        public const string StaffID = "StaffID";
        public const string CompanyTaxNo = "CompanyTaxNo";
        public const string CompanyName = "CompanyName";
        public const string CompanyAddress01 = "CompanyAddress01";
        public const string CompanyAddress02 = "CompanyAddress02";
        public const string CompanyCountry = "CompanyCountry";
        public const string CompanyState = "CompanyState";
        public const string CompanyCity = "CompanyCity";
        public const string CompanyAddress03 = "CompanyAddress03";
        public const string CompanyPhone01 = "CompanyPhone01";
        public const string DepartmentName = "DepartmentName";
        public const string WorkingYear = "WorkingYear";
        public const string WorkingMonth = "WorkingMonth";
        public const string Position = "Position";
        public const string Title = "Title";
        public const string CompanyRemark = "CompanyRemark";

        public const string ContractType = "ContractType";
        public const string JobCategory = "JobCategory";
        public const string BusinessType = "BusinessType";
        public const string TotalStaff = "TotalStaff";
        public const string BusinessSize = "BusinessSize";
        public const string SIC = "SIC";
        public const string NetIncome = "NetIncome";
        public const string TotalExpense = "TotalExpense";
        public const string NumOfDependent = "NumOfDependent";
        public const string HasOtherBankCreditCard = "HasOtherBankCreditCard";
        public const string TotalBankHasCreditCard = "TotalBankHasCreditCard";
        public const string HomeOwnership = "HomeOwnership";
        public const string Education = "Education";
        public const string SecretPhrase = "SecretPhrase";


        // REFERENCE INFO
        public const string MarialStatus = "MarialStatus";
        public const string ContactSpouseType = "ContactSpouseType";
        public const string ContactSpouseName = "ContactSpouseName";
        public const string ContactSpouseID = "ContactSpouseID";
        public const string ContactSpouseMobile = "ContactSpouseMobile";
        public const string ContactSpouseCompanyName = "ContactSpouseCompanyName";
        public const string ContactSpouseRemark = "ContactSpouseRemark";

        public const string Contact01Type = "Contact01Type";
        public const string Contact01Name = "Contact01Name";
        public const string Contact01ID = "Contact01ID";
        public const string Contact01Mobile = "Contact01Mobile";
        public const string Contact01CompanyName = "Contact01CompanyName";
        public const string Contact01Remark = "Contact01Remark";


        // SALE INFO
        public const string ApplicationSourceCode = "ApplicationSourceCode";
        public const string SaleMethod = "SaleMethod";
        public const string ProgramCode = "ProgramCode";
        public const string Contact01Type = "Contact01Type";
        public const string Contact01Type = "Contact01Type";
        public const string Contact01Type = "Contact01Type";
        public const string Contact01Type = "Contact01Type";
        public const string Contact01Type = "Contact01Type";



        // HIDDEN INFO
        public const string ProcessID = "ProcessID";
        public const string PhaseID = "PhaseID";
        public const string RouteID = "RouteID";
        public const string CurrentUserID = "CurrentUserID";
        public const string PreviousUserID = "PreviousUserID";
        public const string CreateDateTime = "CreateDateTime";
        public const string ExportDate = "ExportDate";
        public const string ProcessUserID = "ProcessUserID";
        public const string ProcessDateTime = "ProcessDateTime";

        // Other Info
        public const string ActionName = "ActionName";


        public const string SourceBranchCode = "SourceBranchCode";
        public const string PolicyCode = "PolicyCode";
    }
}