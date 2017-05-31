namespace Modules.Application.Database
{
    public static class AbsentCalendarTable
    {
        public const string StoredProcedure = "APP_GetUserAbsentCalendar";
        public const string DeleteStoredProcedure = "APP_DeleteAbsentCalendar";
        public const string InsertStoredProcedure = "APP_InsertAbsentCalendar";
        public const string ID = "ID";
        public const string UserID = "UserID";
        /// <summary>
        /// 1: IsAllDay | 2: IsMorning | 3: IsAfternoon
        /// </summary>
        public const string AbsentType = "AbsentType";

        public const string AbsentTypeName = "AbsentTypeName";
        public const string FromDate = "FromDate";
        public const string FromDateString = "FromDateString";
        public const string ToDate = "ToDate";
        public const string ToDateString = "ToDateString";
        public const string ModifyUserID = "ModifyUserID";
        public const string ModifyDateTime = "ModifyDateTime";

    }
}
