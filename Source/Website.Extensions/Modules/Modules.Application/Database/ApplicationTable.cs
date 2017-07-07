namespace Modules.Application.Database
{
    public static class ApplicationTable
    {
        public const string TableName = "APP_Application";


        // Application Info
        public const string UniqueID = "UniqueID";
        public const string ApplicationID = "ApplicationID";
        public const string ApplicationTypeID = "ApplicationTypeID";
        public const string Priority = "Priority";
        public const string ApplicationStatus = "ApplicationStatus";
        public const string DecisionCode = "DecisionCode";
        public const string ModifyUserID = "ModifyUserID";
        public const string ModifyDateTime = "ModifyDateTime";
        public const string ApplicationRemark = "ApplicationRemark";


        // Card Holder Info
        public const string CustomerID = "CustomerID";
        public const string IdentityTypeCode = "IdentityTypeCode";
        public const string CIFNo = "CIFNo";
        public const string IsBasicCard = "IsBasicCard";
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
        public const string IsCorporateCard = "IsCorporateCard";
        public const string CustomerType = "CustomerType";
        public const string CustomerClass = "CustomerClass";


        // Contact Info
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


        public const string SourceBranchCode = "SourceBranchCode";
        public const string PolicyCode = "PolicyCode";


        // Hidden Info
        public const string ProcessID = "ProcessID";
        public const string PhaseID = "PhaseID";
        public const string CurrentUserID = "CurrentUserID";
        public const string PreviousUserID = "PreviousUserID";
        public const string CreateDateTime = "CreateDateTime";
        public const string ExportDate = "ExportDate";
        public const string ProcessUserID = "ProcessUserID";
        public const string ProcessDateTime = "ProcessDateTime";
    }
}