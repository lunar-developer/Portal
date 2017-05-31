namespace Modules.Application.DataTransfer
{
    public class AbsentCalendarData
    {
        public string ID;
        public string UserID;
        /// <summary>
        /// 1: IsAllDay | 2: IsMorning | 3: IsAfternoon
        /// </summary>
        public string AbsentType;

        public string AbsentTypeName;
        public string FromDate;
        public string FromDateString;
        public string ToDate;
        public string ToDateString;
        public string ModifyUserID;
        public string ModifyDateTime;
    }
}
