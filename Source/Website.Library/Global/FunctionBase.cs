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
using ClosedXML.Excel;
using DotNetNuke.Entities.Portals;
using DotNetNuke.Entities.Tabs;
using DotNetNuke.Entities.Users;
using DotNetNuke.Instrumentation;
using DotNetNuke.Security.Roles;
using DotNetNuke.Services.Social.Notifications;
using OfficeOpenXml;
using ServiceStack.Text;
using Website.Library.Enum;

namespace Website.Library.Global
{
    public static class FunctionBase
    {
        private static readonly ILog Logger;
        public static readonly string IconSuccess = "<i class=\"fa fa-check icon-success\"></i>";
        public static readonly string IconFailure = "<i class=\"fa fa-close icon-danger\"></i>";


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
            bool result = ConvertToBool(status);
            return result == expected ? IconSuccess : IconFailure;
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

        public static string GetRoleName(string roleID)
        {
            RoleInfo roleInfo = RoleController.Instance.GetRoleById(0, int.Parse(roleID));
            return roleInfo == null ? string.Empty : $"{roleInfo.RoleName} - {roleInfo.Description}";
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
                NotificationTypeID = 6
            };
            NotificationsController.Instance.SendNotification(
                notification, PortalSettings.Current.PortalId, null, new List<UserInfo> { toUser });
        }
        #endregion

        #region Reserve

        //public static string DocTienBangChu(string amount, string strTail)
        //{
        //    string[] ChuSo = new string[10] { " không", " một", " hai", " ba", " bốn", " năm", " sáu", " bảy", " tám", " chín" };
        //    string[] Tien = new string[6] { "", " nghìn", " triệu", " tỷ", " nghìn tỷ", " triệu tỷ" };

        //    amount = amount.Replace(",", "");
        //    if (amount.Contains("."))
        //        amount = amount.Remove(amount.IndexOf('.'));

        //    long SoTien;
        //    if (long.TryParse(amount, out SoTien) == false)
        //    {
        //        return amount;
        //    }

        //    int lan, i;
        //    long so;
        //    string KetQua = "", tmp = "";
        //    int[] ViTri = new int[6];
        //    if (SoTien < 0) return "So tien am !";
        //    if (SoTien == 0) return "Khong dong !";
        //    if (SoTien > 0)
        //    {
        //        so = SoTien;
        //    }
        //    else
        //    {
        //        so = -SoTien;
        //    }
        //    //Ki?m tra s? quá l?n
        //    if (SoTien > 8999999999999999)
        //    {
        //        SoTien = 0;
        //        return "";
        //    }
        //    ViTri[5] = (int)(so / 1000000000000000);
        //    so = so - long.Parse(ViTri[5].ToString()) * 1000000000000000;
        //    ViTri[4] = (int)(so / 1000000000000);
        //    so = so - long.Parse(ViTri[4].ToString()) * +1000000000000;
        //    ViTri[3] = (int)(so / 1000000000);
        //    so = so - long.Parse(ViTri[3].ToString()) * 1000000000;
        //    ViTri[2] = (int)(so / 1000000);
        //    ViTri[1] = (int)((so % 1000000) / 1000);
        //    ViTri[0] = (int)(so % 1000);
        //    if (ViTri[5] > 0)
        //    {
        //        lan = 5;
        //    }
        //    else if (ViTri[4] > 0)
        //    {
        //        lan = 4;
        //    }
        //    else if (ViTri[3] > 0)
        //    {
        //        lan = 3;
        //    }
        //    else if (ViTri[2] > 0)
        //    {
        //        lan = 2;
        //    }
        //    else if (ViTri[1] > 0)
        //    {
        //        lan = 1;
        //    }
        //    else
        //    {
        //        lan = 0;
        //    }
        //    for (i = lan; i >= 0; i--)
        //    {
        //        tmp = DocSo3ChuSo(ViTri[i]);
        //        KetQua += tmp;
        //        if (ViTri[i] != 0) KetQua += Tien[i];
        //        if ((i > 0) && (!string.IsNullOrEmpty(tmp))) KetQua += ",";//&& (!string.IsNullOrEmpty(tmp))
        //    }
        //    if (KetQua.Substring(KetQua.Length - 1, 1) == ",") KetQua = KetQua.Substring(0, KetQua.Length - 1);
        //    KetQua = KetQua.Trim() + strTail;
        //    return KetQua.Substring(0, 1).ToUpper() + KetQua.Substring(1);
        //}

        //public static string DocSo3ChuSo(int baso)
        //{
        //    string[] ChuSo = new string[10] { " không", " một", " hai", " ba", " bốn", " năm", " sáu", " bảy", " tám", " chín" };
        //    string[] Tien = new string[6] { "", " nghìn", " triệu", " tỷ", " nghìn tỷ", " triệu tỷ" };

        //    int tram, chuc, donvi;
        //    string KetQua = "";
        //    tram = (int)(baso / 100);
        //    chuc = (int)((baso % 100) / 10);
        //    donvi = baso % 10;
        //    if ((tram == 0) && (chuc == 0) && (donvi == 0)) return "";
        //    if (tram != 0)
        //    {
        //        KetQua += ChuSo[tram] + " trăm"; //tram
        //        if ((chuc == 0) && (donvi != 0)) KetQua += " linh";
        //    }
        //    if ((chuc != 0) && (chuc != 1))
        //    {
        //        KetQua += ChuSo[chuc] + " mươi"; //muoi
        //        if ((chuc == 0) && (donvi != 0)) KetQua = KetQua + " linh";
        //    }
        //    if (chuc == 1) KetQua += " mười"; //mu?i
        //    switch (donvi)
        //    {
        //        case 1:
        //            if ((chuc != 0) && (chuc != 1))
        //            {
        //                KetQua += " một"; //m?t
        //            }
        //            else
        //            {
        //                KetQua += ChuSo[donvi];
        //            }
        //            break;
        //        case 5:
        //            if (chuc == 0)
        //            {
        //                KetQua += ChuSo[donvi];
        //            }
        //            else
        //            {
        //                KetQua += " lăm"; //lam
        //            }
        //            break;
        //        default:
        //            if (donvi != 0)
        //            {
        //                KetQua += ChuSo[donvi];
        //            }
        //            break;
        //    }
        //    return KetQua;
        //}

        //public static string RemoveDiacriticsExt(string stIn, PatternType pattern)
        //{
        //    string PATTERN_IDENTITY = @"[^a-zA-Z0-9.]";
        //    string PATTERN_PASSPORT = @"[^a-zA-Z0-9]";
        //    string PATTERN_EMBOSSNAME = @"[^a-zA-Z0-9 ]";
        //    string PATTERN_FULLNAME = @"[^a-zA-Z0-9\-_ ]";
        //    string PATTERN_ADDRESS = @"[^a-zA-Z0-9\\\-/,._ ]";
        //    string PATTERN_PHONE = @"[^0-9]";
        //    string PATTERN_NPHONE = @"[^0-9,]";
        //    string PATTERN_EMAIL = @"[^a-zA-Z0-9@._\-]";

        //    stIn = stIn.Trim();

        //    string strFormD = stIn.Normalize(NormalizationForm.FormD);
        //    StringBuilder sb = new StringBuilder();
        //    for (int ich = 0; ich < strFormD.Length; ich++)
        //    {
        //        UnicodeCategory uc = CharUnicodeInfo.GetUnicodeCategory(strFormD[ich]);
        //        if (uc != UnicodeCategory.NonSpacingMark)
        //        {
        //            sb.Append(strFormD[ich]);
        //        }
        //    }
        //    Regex regex = new Regex(@"\p{IsCombiningDiacriticalMarks}+");
        //    string strFormC = (sb.ToString().Normalize(NormalizationForm.FormC));
        //    string str = regex.Replace(strFormC, String.Empty).Replace('\u0111', 'd').Replace('\u0110', 'D');

        //    string result = string.Empty;
        //    switch (pattern)
        //    {
        //        case PatternType.EmbossName:
        //            result = Regex.Replace(Regex.Replace(str, PATTERN_EMBOSSNAME, string.Empty), "[ ]{2,}", " ");
        //            break;
        //        case PatternType.TextAndNumber:
        //            result = Regex.Replace(str, PATTERN_PASSPORT, string.Empty);
        //            break;
        //        case PatternType.Identity:
        //            result = Regex.Replace(str, PATTERN_IDENTITY, string.Empty);
        //            break;
        //        case PatternType.Name:
        //            result = Regex.Replace(Regex.Replace(str, PATTERN_FULLNAME, string.Empty), "[ ]{2,}", " ");
        //            break;
        //        case PatternType.Address:
        //            result = Regex.Replace(Regex.Replace(str, PATTERN_ADDRESS, string.Empty), "[ ]{2,}", " ");
        //            break;
        //        case PatternType.Number:
        //            result = Regex.Replace(str, PATTERN_PHONE, string.Empty);
        //            break;
        //        case PatternType.NPhone:
        //            result = Regex.Replace(str, PATTERN_NPHONE, string.Empty);
        //            break;
        //        case PatternType.Email:
        //            result = Regex.Replace(str, PATTERN_EMAIL, string.Empty);
        //            break;
        //        case PatternType.All:
        //            result = str;
        //            break;
        //    }
        //    return result.ToUpper();
        //}

        #endregion
    }
}