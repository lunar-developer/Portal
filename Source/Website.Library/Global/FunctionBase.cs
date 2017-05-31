using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using ClosedXML.Excel;
using DotNetNuke.Entities.Tabs;
using DotNetNuke.Entities.Users;
using DotNetNuke.Instrumentation;
using ServiceStack.Text;
using Website.Library.Enum;

namespace Website.Library.Global
{
    public static class FunctionBase
    {
        private static readonly ILog Logger = LoggerSource.Instance.GetLogger(typeof(FunctionBase));


        public static bool ConvertToBool(string value)
        {
            string data = value.Trim().ToLower();
            return data == "true" || data == "1";
        }


        /*
         * FORMAT
         */

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
            DateTime date;
            bool isDateFormat = value.Length == PatternEnum.Date.Length;
            return DateTime.TryParseExact(value, isDateFormat ? PatternEnum.Date : PatternEnum.DateTime,
                CultureInfo.InvariantCulture, DateTimeStyles.None, out date)
                ? date.ToString(isDateFormat ? PatternEnum.DateDisplay : PatternEnum.DateTimeDisplay)
                : value;
        }

        public static string FormatUserID(string value)
        {
            int userId;
            if (int.TryParse(value, out userId) == false || userId <= 0)
            {
                return value;
            }

            UserInfo userInfo = new UserController().GetUser(0, userId);
            return userInfo == null ? value : $"{userInfo.DisplayName} ({userInfo.Username})";
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


        /*
         * GET CONTENT
         */

        public static DateTime GetDate(string value)
        {
            bool isDateFormat = value.Length == PatternEnum.Date.Length;
            DateTime.TryParseExact(value, isDateFormat ? PatternEnum.Date : PatternEnum.DateTime,
                CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date);
            return date;
        }

        public static string GetConfiguration(string key)
        {
            return ConfigurationManager.AppSettings[key] ?? string.Empty;
        }

        public static string GetAbsoluteUrl(string absoluteUrl)
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


        /*
         * IMPORT
         */

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
                    T instance = (T) Activator.CreateInstance(type);
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


        /*
         * EXPORT
         */

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


        /*
         * MASK DATA
         */

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

        public static string Minimize(string content)
        {
            content = content.Replace(Environment.NewLine, string.Empty).Replace(CharacterEnum.Tab, string.Empty);
            return Regex.Replace(content, @"\s+", " ").Replace("> <", "><");
        }


        /*
         * STRING UTILITIES
         */

        public static string Left(string value, int length)
        {
            return value.Substring(0, length);
        }

        public static string Right(string value, int length)
        {
            return value.Substring(value.Length - length);
        }


        /*
         * VALIDATE
         */

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

        public static bool IsInArray(string value, params string[] arrayData)
        {
            return arrayData.Any(data => value == data);
        }


        /*
         * LOG
         */


        public static void LogError(Exception exception)
        {
            Logger.Error(exception);
        }

        public static void LogError(string message)
        {
            Logger.Error(message);
        }


        /*
         * Convert
         */
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
    }
}