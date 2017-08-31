using Modules.VSaleKit.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using OfficeOpenXml;
using Website.Library.Global;

namespace Modules.VSaleKit.Global
{
    public class Utilities
    {
        public static bool Validate(string str, string express)
        {
            try
            {
                Regex regex = new Regex(express);
                bool ex1 = regex.IsMatch(str);
                return ex1;
            }
            catch
            {
                return false;
            }
        }
        public static string VskGetRoleName(string[] roles)
        {
            foreach(string r in roles)
            {
                switch(r)
                {
                    case RoleEnum.Collaboration:
                        return RoleEnum.Collaboration;
                    case RoleEnum.Sale:
                        return RoleEnum.Sale;
                    case RoleEnum.Leader:
                        return RoleEnum.Leader;
                    case RoleEnum.Manager:
                        return RoleEnum.Manager;
                    case RoleEnum.Director:
                        return RoleEnum.Director;
                    default:
                        break;
                }
            }
            return null;
        }
        public static string FormatDate2String(string date, string sourceFormat, string DestFormat)
        {
            if (date == null || date.Trim() == "")
            {
                return date;
            }
            else
            {
                return DateTime.ParseExact(date, sourceFormat, null).ToString(DestFormat);
            }
        }
        public static string CreateNotificationTitle(string status)
        {
            string ms = "";
            switch (status.Trim())
            {
                case "1":
                    ms = "Bạn có hồ sơ chờ xử lý";
                    break;
                case "3":
                    ms = "Hồ sơ đã bị trả lại";
                    break;
                case "7":
                    ms = "Hồ sơ đã được thẩm định phê duyệt";
                    break;
            }
            return ms;
        }

        public static string CreateNotificationMsg(string status)
        {
            string ms = "";
            switch (status.Trim())
            {
                case "1":
                    ms = "Có một hồ sơ đang chờ bạn duyệt";
                    break;
                case "3":
                    ms = "Hồ sơ đã bị trả lại và đợi bạn xử lý";
                    break;
                case "7":
                    ms = "Hồ sơ đã được thẩm định phê duyệt";
                    break;
            }
            return ms;
        }

        public static string RemoveUnicode(string str)
        {
            try
            {
                if (str == null)
                    return "";
                return FunctionBase.RemoveUnicode(str);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static string RemoveCharacterSpace(string str)
        {
            int index = -1;
            string msg = str;
            try
            {
                if (str == null)
                {
                    return str;
                }
                msg = msg.Trim();
                while (true)
                {
                    msg = msg.Replace("  ", " ");
                    index = msg.IndexOf("  ");
                    if (index < 0)
                    {
                        break;
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return msg;
        }
        public static string RemoveUnicodeAndSpace(string str)
        {
            return RemoveUnicode(RemoveCharacterSpace(str));
        }

        public static List<T> ImportExcel<T>(ExcelPackage package) where T : class
        {
            Type type = typeof(T);
            List<T> listResult = new List<T>();
            ExcelWorksheet workSheet = package.Workbook.Worksheets.First();
            List<string> listFields = new List<string>();
            int cntColumn = 0;
            
            /*get Header*/
            foreach (var firstRowCell in workSheet.Cells[1, 1, 1, workSheet.Dimension.End.Column])
            {
                listFields.Add(firstRowCell.Text);
                cntColumn++;
            }

            for (var rowNumber = 2; rowNumber <= workSheet.Dimension.End.Row; rowNumber++)
            {
                ExcelRange row = workSheet.Cells[rowNumber, 1, rowNumber, cntColumn];
                T instance = (T)Activator.CreateInstance(type);
                string[] listData = new string[cntColumn];
                //int c = 0;
                //foreach (var cell in row)
                for(int i = 0; i<cntColumn;i++)
                {
                    listData[i] = row[rowNumber,i+1].Text;// cell.Text;
                    //c++;
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
    }
}
