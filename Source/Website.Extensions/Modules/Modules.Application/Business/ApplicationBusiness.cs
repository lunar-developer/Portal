using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using Modules.Application.DataAccess;
using Modules.Application.Database;
using Modules.Application.DataTransfer;
using Modules.Application.Enum;
using Modules.UserManagement.Business;
using Modules.UserManagement.DataTransfer;
using Website.Library.Business;
using Website.Library.DataTransfer;
using Website.Library.Enum;
using Website.Library.Extension;
using Website.Library.Global;
using RoleEnum = Modules.Application.Enum.RoleEnum;

namespace Modules.Application.Business
{
    public static class ApplicationBusiness
    {
        public static Timer ScheduleTimer;


        static ApplicationBusiness()
        {
            // Initialize Schedule Timer
            ScheduleData scheduleData = CacheBase.Receive<ScheduleData>(ScheduleEnum.AutoAssign);
            long period = long.Parse(scheduleData?.Period ?? "10") * 60 * UnitEnum.Second;
            ScheduleTimer = new Timer(ProcessOnSchedule, null, period, period);

            // Register Queue Auto Receive
            MessageQueueBusiness.InjectConsumer(ConfigurationBase.QueuePortalIn, ProcessOnQueueReceive, "Portal.In");
        }

        public static void Initialize()
        {
        }

        private static void ProcessOnSchedule(object state)
        {
            try
            {
                // Validate Configuration
                ScheduleData scheduleData = CacheBase.Receive<ScheduleData>(ScheduleEnum.AutoAssign);
                if (scheduleData == null || FunctionBase.ConvertToBool(scheduleData.IsDisable))
                {
                    return;
                }


                // Validate Working Day & Time
                DayOfWeek dayOfWeek = DateTime.Now.DayOfWeek;
                int startTime = int.Parse(scheduleData.StartTime);
                int endTime = int.Parse(scheduleData.EndTime);
                int currentTime = int.Parse(DateTime.Now.ToString("HHmm"));
                if (dayOfWeek == DayOfWeek.Sunday
                    || currentTime < startTime
                    || currentTime > endTime
                    || dayOfWeek == DayOfWeek.Saturday && currentTime > 1200)
                {
                    return;
                }


                // Process Auto Assign
                AutoAssign();
            }
            catch (Exception exception)
            {
                FunctionBase.LogError(exception);
            }
        }


        public static long InsertApplication(int userID, Dictionary<string, string> fieldDictionary)
        {
            List<FieldData> listApplication = new List<FieldData>();
            List<FieldData> listInteger = new List<FieldData>();
            List<FieldData> listBigInteger = new List<FieldData>();
            List<FieldData> listString = new List<FieldData>();
            List<FieldData> listUnicodeString = new List<FieldData>();
            List<FieldData> listLog = new List<FieldData>();
            BindData(fieldDictionary, listApplication, listInteger, listBigInteger,
                listString, listUnicodeString, listLog);
            if (listApplication.Count == 0)
            {
                return 0;
            }

            return new ApplicationProvider().InsertApplication(userID,
                listApplication, listInteger, listBigInteger, listString, listUnicodeString, listLog);
        }

        public static long UpdateApplication(int userID, string applicationID,
            Dictionary<string, string> fieldDictionary)
        {
            List<FieldData> listApplication = new List<FieldData>();
            List<FieldData> listInteger = new List<FieldData>();
            List<FieldData> listBigInteger = new List<FieldData>();
            List<FieldData> listString = new List<FieldData>();
            List<FieldData> listUnicodeString = new List<FieldData>();
            List<FieldData> listLog = new List<FieldData>();
            BindData(fieldDictionary, listApplication, listInteger, listBigInteger,
                listString, listUnicodeString, listLog);
            if (listApplication.Count == 0)
            {
                return 0;
            }

            return new ApplicationProvider().UpdateApplication(userID, applicationID,
                listApplication, listInteger, listBigInteger, listString, listUnicodeString, listLog);
        }

        public static DataSet LoadApplication(string applicationID, int userID)
        {
            return new ApplicationProvider().LoadApplication(applicationID, userID);
        }

        public static DataTable SearchApplication(Dictionary<string, SQLParameterData> conditionDictionary)
        {
            return new ApplicationProvider().SearchApplication(conditionDictionary);
        }

        public static long ProcessApplication(Dictionary<string, SQLParameterData> parameterDictionary)
        {
            return new ApplicationProvider().ProcessApplication(parameterDictionary);
        }


        private const string LogTemplate = @"
            <table class=""ScheduleLogTable"">
                <tr>
                    <td colspan=""4"">GIAI ĐOẠN: <b>{0}</b></td>
                </tr>
                <tr>
                    <td colspan=""4"">
                        Tổng số hồ sơ: <b>{1}</b><br>
                        Tổng số Users: <b>{2}</b>
                    </td>
                </tr>
                <tr>
                    <th></th>
                    <th>User</th>
                    <th>Đang Xử Lý</th>
                    <th>Nhận Thêm</th>
                </tr>
                {3}
            </table>
        ";

        public static void AutoAssign()
        {
            DataSet dsResult = new ApplicationProvider().GetAutoAssignData();
            if (dsResult.Tables.Count < 3 || dsResult.Tables[0].Rows.Count == 0)
            {
                return;
            }

            DataTable phaseTable = dsResult.Tables[0];
            DataTable applicationTable = dsResult.Tables[1];
            DataTable userTable = dsResult.Tables[2];
            List<AutoAssignData> listAutoAssignData = new List<AutoAssignData>();
            foreach (DataRow row in phaseTable.Rows)
            {
                string logMessage;
                List<UserAssignData> listUserAssignData = new List<UserAssignData>();

                string currentPhaseID = row[ApplicationTable.PhaseID].ToString();
                string targetPhaseID = row["TargetPhaseID"].ToString();

                // Get Applications
                DataRow[] listApplications = applicationTable.Select($"PhaseID = {currentPhaseID}");
                if (listApplications.Length == 0)
                {
                    logMessage = "Không có hồ sơ cần xử lý.";
                    goto Next;
                }

                // Get Users
                DataRow[] listUsers = userTable.Select($"PhaseID = {targetPhaseID}", "UserID ASC");
                if (listUsers.Length == 0)
                {
                    logMessage = "Không tìm thấy thông tin User xử lý.";
                    goto Next;
                }

                // Calculate User Data
                foreach (DataRow user in listUsers)
                {
                    listUserAssignData.Add(new UserAssignData(user));
                }

                // Assign Applications
                foreach (DataRow application in listApplications)
                {
                    string applicationID = application[ApplicationTable.ApplicationID].ToString();
                    string policyCode = application[ApplicationTable.PolicyCode].ToString();
                    string previousUserID = application[ApplicationTable.PreviousUserID].ToString();

                    // Try to assign to previous user
                    if (previousUserID != "0")
                    {
                        UserAssignData userData =
                            listUserAssignData.FirstOrDefault(item => item.UserID == previousUserID);
                        if (userData != null && userData.Contain(policyCode))
                        {
                            userData.Assign(applicationID);
                            continue;
                        }
                    }
                    
                    // Pick user has minimum applications
                    int index = -1;
                    int total = int.MaxValue;
                    for (int i = 0; i < listUserAssignData.Count; i++)
                    {
                        UserAssignData userData = listUserAssignData[i];
                        if (total <= userData.Total
                            || userData.Contain(policyCode) == false)
                        {
                            continue;
                        }

                        total = userData.Total;
                        index = i;
                    }
                    if (index > -1)
                    {
                        listUserAssignData[index].Assign(applicationID);
                    }
                }


                // Build Log
                List<string> listLog = new List<string>();
                int count = 0;
                for (int i = 0; i < listUserAssignData.Count; i++)
                {
                    UserAssignData userAssignData = listUserAssignData[i];
                    string userName = userAssignData.UserName;
                    int current = userAssignData.TotalApplications;
                    int totalAssigned = userAssignData.TotalAssigned;
                    count += totalAssigned;
                    listLog.Add($"<tr><td>{i + 1}</td><td>{userName}</td><td>{current}</td><td>{totalAssigned}</td></tr>");
                }
                listLog.Add($"<tr><td>#</td><td>Un-Assign</td><td>{listApplications.Length - count}</td><td>0</td></tr>");
                logMessage = string.Format(LogTemplate,
                    PhaseBussiness.GetPhaseName(currentPhaseID).ToUpper(),
                    listApplications.Length, 
                    listUserAssignData.Count,
                    string.Join(string.Empty, listLog));


                Next:
                listAutoAssignData.Add(new AutoAssignData
                {
                    CurrentPhaseID = currentPhaseID,
                    TargetPhaseID = targetPhaseID,
                    LogMessage = logMessage,
                    ListUserAssignData = listUserAssignData.Where(item => item.TotalAssigned > 0).ToList()
                });
            }

            ApplicationProvider provider = new ApplicationProvider();
            provider.AutoAssign(ConfigurationBase.SystemUserID, listAutoAssignData);
        }


        private static void BindData(IReadOnlyDictionary<string, string> fieldDictionary,
            ICollection<FieldData> listApplication,
            ICollection<FieldData> listInteger,
            ICollection<FieldData> listBigInteger,
            ICollection<FieldData> listString,
            ICollection<FieldData> listUnicodeString,
            ICollection<FieldData> listLog)
        {
            List<ApplicationFieldData> listField = CacheBase.Receive<ApplicationFieldData>();
            foreach (ApplicationFieldData fieldInfo in listField)
            {
                if (fieldDictionary.ContainsKey(fieldInfo.FieldName) == false)
                {
                    continue;
                }

                FieldData fieldData = new FieldData
                {
                    FieldName = fieldInfo.FieldName,
                    FieldValue = PrebuildData(fieldDictionary[fieldInfo.FieldName], fieldInfo.DataType)
                };
                switch (fieldInfo.TableName)
                {
                    case "APP_ApplicationFieldInteger":
                        listInteger.Add(fieldData);
                        break;

                    case "APP_ApplicationFieldBigInteger":
                        listBigInteger.Add(fieldData);
                        break;

                    case "APP_ApplicationFieldString":
                        listString.Add(fieldData);
                        break;

                    case "APP_ApplicationFieldUnicodeString":
                        listUnicodeString.Add(fieldData);
                        break;

                    default:
                        listApplication.Add(fieldData);
                        break;
                }

                FieldData logData = new FieldData()
                {
                    FieldName = fieldInfo.FieldName,
                    FieldValue = PrebuildData(fieldDictionary[fieldInfo.FieldName], "nvarchar")
                };
                listLog.Add(logData);
            }
        }

        private static string PrebuildData(string value, string dataType)
        {
            switch (dataType.ToLower())
            {
                case "varchar":
                    return $"'{value}'";

                case "nvarchar":
                    return $"N'{value}'";

                default:
                    return string.IsNullOrWhiteSpace(value) ? "''" : value;
            }
        }


        public static List<UserData> GetListUserApproval()
        {
            return UserBusiness.GetUsersHaveRoles(RoleEnum.UserCredit, RoleEnum.Approval);
        }

        public static List<UserData> GetListUserAssessment()
        {
            return UserBusiness.GetUsersHaveRoles(RoleEnum.Assessment);
        }

        public static List<UserData> GetListUserCredit()
        {
            return UserBusiness.GetUsersHaveRoles(RoleEnum.UserCredit);
        }

        public static string GetEmbossingName(string value, int maxLength = 20)
        {
            value = FunctionBase.GetASCIIString(value);
            if (value.Length <= maxLength)
            {
                return value;
            }

            string[] array = value.Split(' ');
            if (array.Length == 1)
            {
                value = FunctionBase.Right(value, maxLength);
            }
            else
            {
                // Keep the First and Last word
                int length = value.Length;
                for (int i = 1; i < array.Length - 1; i++)
                {
                    length -= array[i].Length - 1;
                    array[i] = array[i][0].ToString();

                    if (length <= maxLength)
                    {
                        break;
                    }
                }
                value = string.Join(CharacterEnum.Space, array);
                if (value.Length > maxLength)
                {
                    value = FunctionBase.Right(value, maxLength);
                }
            }
            return value;
        }


        public static void ProcessOnQueueReceive(string data, string contentType)
        {
            RequestData requestData = FunctionBase.Deserialize<RequestData>(data, contentType);
            InsensitiveDictionary<string> dataDictionary =
                FunctionBase.Deserialize<InsensitiveDictionary<string>>(requestData.Data);

            string uniqueID = dataDictionary["ApplicID"];
            string functionName = requestData.Function;
            switch (functionName)
            {
                case FunctionEnum.SupplyDocument:
                    SupplyDocument(uniqueID);
                    break;

                default:
                    
                    LinkApplication(uniqueID);
                    break;
            }
        }

        public static void LinkApplication(string uniqueID)
        {
            DataTable dtResult = new ApplicationProvider().GetVSaleKitApplication(uniqueID);
            long result = CreateApplication(dtResult);
            if (result > 0)
            {
                new ApplicationProvider().LinkApplication(uniqueID, FunctionEnum.CreateApplication, result.ToString());
            }
        }

        private static long CreateApplication(DataTable dtResult)
        {
            if (dtResult.Rows.Count == 0)
            {
                return 0;
            }

            Dictionary<string, string> fieldDictionary = new Dictionary<string, string>();
            DataRow row = dtResult.Rows[0];
            int userID = int.Parse(row["CreateUserID"].ToString());
            string createDateTime = DateTime.Now.ToString(PatternEnum.DateTime);
            
            fieldDictionary.Add(ApplicationTable.UniqueID, row[ApplicationTable.UniqueID].ToString().Trim());
            fieldDictionary.Add(ApplicationTable.CIFNo, string.Empty);
            fieldDictionary.Add(ApplicationTable.CustomerID, row[ApplicationTable.CustomerID].ToString().Trim());
            fieldDictionary.Add(ApplicationTable.IdentityTypeCode, row[ApplicationTable.IdentityTypeCode].ToString().Trim());
            fieldDictionary.Add(ApplicationTable.FullName, row[ApplicationTable.FullName].ToString().Trim());
            fieldDictionary.Add(ApplicationTable.EmbossName, string.Empty);
            fieldDictionary.Add(ApplicationTable.BirthDate, "19870101");
            fieldDictionary.Add(ApplicationTable.ApplicationTypeID, row[ApplicationTable.ApplicationTypeID].ToString());
            fieldDictionary.Add(ApplicationTable.SourceBranchCode, row[ApplicationTable.SourceBranchCode].ToString().Trim());
            fieldDictionary.Add(ApplicationTable.ProgramCode, string.Empty);
            fieldDictionary.Add(ApplicationTable.PolicyCode, row[ApplicationTable.PolicyCode].ToString().Trim());
            fieldDictionary.Add(ApplicationTable.DecisionCode, string.Empty);
            fieldDictionary.Add(ApplicationTable.ExportDate, "0");
            fieldDictionary.Add(ApplicationTable.ApplicationStatus, ApplicationStatusEnum.WaitForInput01);
            fieldDictionary.Add(ApplicationTable.ApplicationRemark, row[ApplicationTable.ApplicationRemark].ToString().Trim());
            fieldDictionary.Add(ApplicationTable.ProcessID, row[ApplicationTable.ProcessID].ToString());
            fieldDictionary.Add(ApplicationTable.PhaseID, PhaseEnum.WaitForInput);
            fieldDictionary.Add(ApplicationTable.CurrentUserID, "0");
            fieldDictionary.Add(ApplicationTable.PreviousUserID, "0");
            fieldDictionary.Add(ApplicationTable.CreateDateTime, createDateTime);
            fieldDictionary.Add(ApplicationTable.ModifyUserID, "0");
            fieldDictionary.Add(ApplicationTable.ModifyDateTime, "0");
            fieldDictionary.Add(ApplicationTable.ProposeLimit, row[ApplicationTable.CreditLimit].ToString());
            fieldDictionary.Add(ApplicationTable.CreditLimit, row[ApplicationTable.CreditLimit].ToString());
            fieldDictionary.Add(ApplicationTable.Priority, row[ApplicationTable.Priority].ToString());
            fieldDictionary.Add(ApplicationTable.Mobile01, row[ApplicationTable.Mobile01].ToString());

            // Insert Application
            return InsertApplication(userID, fieldDictionary);
        }

        public static void SupplyDocument(string uniqueID)
        {
            new ApplicationProvider().SupplyDocument(uniqueID, FunctionEnum.SupplyDocument);
        }

        public static string SendToQueue(
            string functionName, 
            string data, 
            ref string contentType,
            out string responseData,
            int timeout = 10)
        {
            try
            {
                MessageQueueData messageData = new MessageQueueData(functionName, data, timeout);
                MessageQueueBusiness.SendToQueue(ConfigurationBase.QueuePortalServiceIn, ref messageData);
                messageData.AutoEvent.WaitOne();
                contentType = messageData.ContentType;
                responseData = messageData.ResponseData;
                return messageData.ResponseCode;
            }
            catch (Exception exception)
            {
                FunctionBase.LogError(exception);
                responseData = ResponseEnum.GetDescription(ResponseEnum.Error);
                return ResponseEnum.Error;
            }
        }

        public static InsensitiveDictionary<string> QueryCustomerByIDNo(string customerID, out string message)
        {
            InsensitiveDictionary<string> dataDictionary = new InsensitiveDictionary<string>
            {
                { ApplicationTable.CustomerID, customerID }
            };
            string contentType = ContentEnum.Json;
            string requestData = FunctionBase.Serialize(dataDictionary, contentType);
            string responseCode = SendToQueue(FunctionEnum.QueryCustomerByID, requestData, ref contentType, out string responseData);
            switch (responseCode)
            {
                case ResponseEnum.Success:
                    dataDictionary = FunctionBase.Deserialize<InsensitiveDictionary<string>>(responseData);
                    if (dataDictionary == null || dataDictionary.Count == 0)
                    {
                        message = "Không tìm thấy thông tin khách hàng.";
                        return null;
                    }

                    message = string.Empty;
                    return dataDictionary;

                default:
                    message = responseData;
                    return null;
            }
        }

        public static CustomerData QueryAccount(string cifNo, out string message)
        {
            InsensitiveDictionary<string> dataDictionary = new InsensitiveDictionary<string>
            {
                { ApplicationTable.CIFNo, cifNo }
            };
            string contentType = ContentEnum.Json;
            string requestData = FunctionBase.Serialize(dataDictionary, contentType);
            string responseCode = SendToQueue(FunctionEnum.QueryAccount, requestData, ref contentType, out string responseData);
            switch (responseCode)
            {
                case ResponseEnum.Success:
                    CustomerData customer = FunctionBase.Deserialize<CustomerData>(responseData);
                    if (customer == null)
                    {
                        message = "Không tìm thấy thông tin khách hàng.";
                        return null;
                    }

                    message = string.Empty;
                    return customer;

                default:
                    message = responseData;
                    return null;
            }
        }

        public static InsensitiveDictionary<string> QueryCollateral(string collateralID, out string message)
        {
            InsensitiveDictionary<string> dataDictionary = new InsensitiveDictionary<string>
            {
                { ApplicationTable.CollateralID, collateralID }
            };
            string contentType = ContentEnum.Json;
            string requestData = FunctionBase.Serialize(dataDictionary, contentType);
            string responseCode = SendToQueue(FunctionEnum.QueryCollateral, requestData, ref contentType, out string responseData);
            switch (responseCode)
            {
                case ResponseEnum.Success:
                    dataDictionary = FunctionBase.Deserialize<InsensitiveDictionary<string>>(responseData);
                    if (dataDictionary == null || dataDictionary.Count == 0)
                    {
                        message = "Không tìm thấy thông tin tài sản đảm bảo.";
                        return null;
                    }

                    message = string.Empty;
                    return dataDictionary;

                default:
                    message = responseData;
                    return null;
            }
        }
    }
}