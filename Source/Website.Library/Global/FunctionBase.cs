using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI.WebControls;
using ClosedXML.Excel;
using DotNetNuke.Entities.Portals;
using DotNetNuke.Entities.Tabs;
using DotNetNuke.Entities.Users;
using DotNetNuke.Instrumentation;
using DotNetNuke.Security.Roles;
using DotNetNuke.Services.Log.EventLog;
using DotNetNuke.Services.Social.Notifications;
using OfficeOpenXml;
using ServiceStack.Text;
using Website.Library.Enum;

namespace Website.Library.Global
{
    public static class FunctionBase
    {
        private static readonly ILog Logger;
        private static readonly string GrantImage = "/images/grant.gif";
        private static readonly string ErrorImage = "/images/red-error_16px.gif";


        static FunctionBase()
        {
            try
            {
                Logger = LoggerSource.Instance.GetLogger(typeof(FunctionBase));
            }
            catch
            {
                // ignored
            }
        }


        public static bool ConvertToBool(string value)
        {
            string data = value?.Trim().ToLower();
            return data == "true" || data == "1";
        }

        public static int ConvertToInteger(string value, int defaultValues = 0)
        {
            if (int.TryParse(value, out int result) == false)
            {
                result = defaultValues;
            }
            return result;
        }


        #region FORMAT
        public static string FormatDecimal(string value)
        {
            decimal result;
            return decimal.TryParse(value, out result)
                ? FormatDecimal(result)
                : value;
        }

        public static string FormatDecimal(decimal value)
        {
            return value.ToString(PatternEnum.Money, CultureInfo.InvariantCulture);
        }

        public static string FormatDate(string value)
        {
            bool isDateFormat = value.Length == PatternEnum.Date.Length;
            return DateTime.TryParseExact(value, isDateFormat ? PatternEnum.Date : PatternEnum.DateTime,
                CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date)
                ? date.ToString(isDateFormat ? PatternEnum.DateDisplay : PatternEnum.DateTimeDisplay)
                : string.Empty;
        }

        public static string FormatUser(string userName, string displayName)
        {
            return $"{displayName} ({userName})";
        }

        public static string FormatUserID(string value)
        {
            if (int.TryParse(value, out int userId) == false || userId <= 0)
            {
                return string.Empty;
            }

            UserInfo userInfo = new UserController().GetUser(0, userId);
            return userInfo == null ? string.Empty : $"{userInfo.DisplayName} ({userInfo.Username})";
        }

        public static string FormatUserName(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return value;
            }

            UserInfo userInfo = UserController.GetUserByName(value);
            return userInfo == null ? value : $"{userInfo.DisplayName} ({userInfo.Username})";
        }

        public static string FormatRoleID(string value)
        {
            if (int.TryParse(value, out int roleID) == false
                || roleID <= 0)
            {
                return string.Empty;
            }

            RoleInfo roleInfo = RoleController.Instance.GetRoleById(0, roleID);
            return roleInfo != null
                ? $"{roleInfo.RoleName} - {roleInfo.Description}"
                : value;
        }

        public static string FormatCurrency(string value, string currencyCode = CurrencyEnum.VND)
        {
            double amount = double.Parse(value);
            switch (currencyCode)
            {
                case CurrencyEnum.VND:
                    return amount.ToString("#,##0");

                case CurrencyEnum.USD:
                    return amount.ToString("#,##0.00");

                default:
                    return amount.ToString(string.Empty);
            }
        }

        public static string FormatPhoneNumber(string value)
        {
            if (value.Length >= 10)
            {
                return value.Substring(0, 4) + " " + value.Substring(4, 3) + " " + value.Substring(7);
            }
            return value;
        }

        public static string FormatHourAndMinutes(string value)
        {
            return value.Length == 4
                ? $"{Left(value, 2)}:{Right(value, 2)}"
                : value;
        }

        public static string FormatState(string status, bool expected = true)
        {
            const string img = @"<img src=""{0}""/>";
            bool result = ConvertToBool(status);
            return result == expected ? string.Format(img, GrantImage) : string.Format(img, ErrorImage);
        }
        #endregion

        #region GET CONTENT
        public static DateTime GetDate(string value)
        {
            bool isDateFormat = value.Length == PatternEnum.Date.Length;
            DateTime.TryParseExact(value, isDateFormat ? PatternEnum.Date : PatternEnum.DateTime,
                CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date);
            return date;
        }

        public static string GetConfiguration(string key, string defaultValue = "")
        {
            return ConfigurationManager.AppSettings[key] ?? defaultValue;
        }

        public static string GetAbsoluteUrl(string absoluteUrl = "")
        {
            return (HttpContext.Current.Request.ApplicationPath + absoluteUrl.Replace("~", string.Empty))
                .Replace("//", "/");
        }

        public static string GetClassGuid(Type type)
        {
            return type.GUID.ToString(PatternEnum.GuidDigits);
        }

        public static string GetTabUrl(string uniqueID)
        {
            TabInfo tabInfo = TabController.Instance.GetTabsByPortal(0).Values
                .FirstOrDefault(tab => tab.UniqueId.Equals(new Guid(uniqueID)));
            return tabInfo?.FullUrl;
        }

        public static string ReadFile(string filePath)
        {
            return File.Exists(filePath) ? File.ReadAllText(filePath) : string.Empty;
        }

        public static UserInfo GetUserByUserID(int userID)
        {
            return UserController.GetUserById(PortalSettings.Current.PortalId, userID);
        }

        public static UserInfo GetUserByUserName(string userName)
        {
            return UserController.GetUserByName(PortalSettings.Current.PortalId, userName);
        }

        public static Type GetAssemblyType(string assemblyName, string className)
        {
            try
            {
                return Type.GetType($"{className}, {assemblyName}");
            }
            catch (Exception exception)
            {
                LogError(exception);
                return null;
            }
        }
        #endregion

        #region IMPORT
        public static List<T> ImportCSV<T>(string filePath, char separator = SeparatorEnum.Comma) where T : class
        {
            using (FileStream stream = File.OpenRead(filePath))
            {
                return ImportCSV<T>(stream, separator);
            }
        }

        public static List<T> ImportCSV<T>(Stream stream, char separator = SeparatorEnum.Comma) where T : class
        {
            Type type = typeof(T);
            List<T> listResult = new List<T>();
            List<string> listFields = new List<string>();
            int row = 0;

            using (StreamReader reader = new StreamReader(stream))
            {
                while (reader.EndOfStream == false)
                {
                    string line = reader.ReadLine();
                    if (string.IsNullOrWhiteSpace(line))
                    {
                        continue;
                    }
                    string[] listData = line.Split(separator);

                    // Read header
                    if (row == 0)
                    {
                        row++;
                        listFields = listData.ToList();
                        continue;
                    }

                    // Read data
                    T instance = (T)Activator.CreateInstance(type);
                    for (int i = 0; i < listFields.Count; i++)
                    {
                        string field = listFields[i];
                        if (string.IsNullOrWhiteSpace(field))
                        {
                            continue;
                        }
                        type.GetField(field)?.SetValue(instance, listData[i]);
                        type.GetProperty(field)?.SetValue(instance, listData[i]);
                    }
                    listResult.Add(instance);
                }
            }

            return listResult;
        }

        public static List<T> ImportExcel<T>(Stream stream) where T : class
        {
            Type type = typeof(T);
            List<T> listResult = new List<T>();
            ExcelPackage package = new ExcelPackage(stream);
            ExcelWorksheet workSheet = package.Workbook.Worksheets.First();
            int totalColumns = workSheet.Dimension.End.Column;
            List<string> listFields = workSheet
                .Cells[1, 1, 1, totalColumns]
                .Select(cell => cell.Text).ToList();

            for (int rowNumber = 2; rowNumber <= workSheet.Dimension.End.Row; rowNumber++)
            {
                ExcelRange row = workSheet.Cells[rowNumber, 1, rowNumber, totalColumns];
                T instance = (T)Activator.CreateInstance(type);
                List<string> listData = new List<string>();

                for (int columnNumber = 1; columnNumber <= totalColumns; columnNumber++)
                {
                    listData.Add(row[rowNumber, columnNumber].Text);
                }
                for (int i = 0; i < listFields.Count; i++)
                {
                    string field = listFields[i];
                    if (string.IsNullOrWhiteSpace(field))
                    {
                        continue;
                    }
                    type.GetField(field)?.SetValue(instance, listData[i]);
                    type.GetProperty(field)?.SetValue(instance, listData[i]);
                }
                listResult.Add(instance);
            }

            return listResult;
        }
        #endregion

        #region EXPORT
        public static byte[] ExportToExcel(DataTable dtResult)
        {
            XLWorkbook workbook = new XLWorkbook();
            workbook.Worksheets.Add(dtResult, "Sheet");

            MemoryStream memoryStream = new MemoryStream();
            workbook.SaveAs(memoryStream);
            byte[] bytes = memoryStream.ToArray();
            memoryStream.Close();

            return bytes;
        }
        #endregion

        #region DATA MARK
        public static string MaskCardNo(string cardNo)
        {
            return cardNo.Length < 16
                ? cardNo
                : $"{Left(cardNo, 6)}-{Right(cardNo, 4)}";
        }

        public static string MaskEmail(string value)
        {
            return value.Length < 5
                ? value
                : value.Substring(0, 4) + "*";
        }

        public static string MaskMobile(string value)
        {
            return value.Length < 5
                ? value
                : new string('*', value.Length - 5) + value.Substring(value.Length - 5);
        }
        #endregion

        #region STRING UTILITIES
        public static string Minimize(string content)
        {
            content = content.Replace(Environment.NewLine, string.Empty).Replace(CharacterEnum.Tab, string.Empty);
            return Regex.Replace(content, @"\s+", " ").Replace("> <", "><");
        }

        public static string Left(string value, int length)
        {
            return value.Substring(0, length);
        }

        public static string Right(string value, int length)
        {
            return value.Substring(value.Length - length);
        }

        public static string GetCoalesceString(params string[] parameter)
        {
            foreach (string data in parameter)
            {
                if (string.IsNullOrWhiteSpace(data) == false)
                {
                    return data;
                }
            }
            return string.Empty;
        }

        public static string GetASCIIString(string value, bool isUpperCase = true)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return string.Empty;
            }
            value = RemoveUnicode(value.Trim()).Replace(CharacterEnum.DoubleSpace, CharacterEnum.Space);
            return isUpperCase ? value.ToUpper() : value;
        }

        public static string RemoveUnicode(string value)
        {
            string normalizedString = value.Normalize(NormalizationForm.FormD);
            StringBuilder stringBuilder = new StringBuilder();

            foreach (char c in normalizedString)
            {
                UnicodeCategory unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }

        public static string GetRandomGuid()
        {
            return Guid.NewGuid().ToString(PatternEnum.GuidDigits);
        }

        public static string SeparateCapitalLetters(string value, string separated = CharacterEnum.Space)
        {
            Regex regex = new Regex(@"
                (?<=[A-Z])(?=[A-Z][a-z]) |
                (?<=[^A-Z])(?=[A-Z]) |
                (?<=[A-Za-z])(?=[^A-Za-z])", RegexOptions.IgnorePatternWhitespace);

            return regex.Replace(value, separated);
        }
        #endregion

        #region VALIDATION
        public static bool IsInRole(string roleName)
        {
            UserInfo userInfo = UserController.Instance.GetCurrentUserInfo();
            return userInfo != null && userInfo.IsInRole(roleName);
        }

        public static bool IsInRole(string roleName, int userID)
        {
            UserInfo userInfo = UserController.GetUserById(0, userID);
            return userInfo != null && userInfo.IsInRole(roleName);
        }

        public static bool IsEmail(string email)
        {
            return Regex.IsMatch(email,
                @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$",
                RegexOptions.IgnoreCase);
        }

        public static bool IsNullOrWhiteSpace(params string[] array)
        {
            return array.Any(string.IsNullOrWhiteSpace);
        }

        public static bool IsInArray(string value, params string[] arrayData)
        {
            return arrayData.Any(data => value == data);
        }

        public static bool IsInArray(int value, params int[] arrayData)
        {
            return arrayData.Any(data => value == data);
        }
        #endregion

        #region LOG
        public static void LogError(Exception exception)
        {
            Logger.Error(exception);
        }

        public static void LogError(string message)
        {
            Logger.Error(message);
        }
        #endregion

        #region SERIALIZE / DESERIALIZE
        public static T Deserialize<T>(string data, string contentType = ContentEnum.Json) where T : class
        {
            switch (contentType)
            {
                case ContentEnum.Json:
                    return JsonSerializer.DeserializeFromString(data, typeof(T)) as T;

                case ContentEnum.Xml:
                    return XmlSerializer.DeserializeFromString(data, typeof(T)) as T;

                default:
                    return null;
            }
        }

        public static string Serialize<T>(T data, string contentType = ContentEnum.Json) where T : class
        {
            switch (contentType)
            {
                case ContentEnum.Json:
                    return JsonSerializer.SerializeToString(data);

                case ContentEnum.Xml:
                    return XmlSerializer.SerializeToString(data);

                default:
                    return string.Empty;
            }
        }
        #endregion

        #region SEND NOTIFICATION
        public static void SendNotification(string subject, string body, int fromUserID, string toUserName)
        {
            UserInfo fromUser = GetUserByUserID(fromUserID);
            UserInfo toUser = GetUserByUserName(toUserName);
            SendNotification(subject, body, fromUser, toUser);
        }

        public static void SendNotification(string subject, string body, UserInfo fromUser, UserInfo toUser)
        {
            Notification notification = new Notification
            {
                Subject = subject,
                Body = body,
                From = fromUser.Email,
                SenderUserID = fromUser.UserID,
                NotificationTypeID = 1
            };
            NotificationsController.Instance.SendNotification(
                notification, PortalSettings.Current.PortalId, null, new List<UserInfo> { toUser });
        }
        #endregion
    }
}