using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using DotNetNuke.UI.Skins.Controls;
using Modules.EmployeeManagement.Business;
using Modules.EmployeeManagement.Database;
using Modules.EmployeeManagement.DataTransfer;
using Modules.EmployeeManagement.Enum;
using Modules.EmployeeManagement.Global;
using OfficeOpenXml;
using Website.Library.Enum;

namespace DesktopModules.Modules.EmployeeManagement
{
    public partial class EmployeeUpload : EmployeeManagementModuleBase
    {
        protected override void OnLoad(EventArgs e)
        {
        }

        private static bool CheckTemplateFormat(IReadOnlyList<string> listColumn)
        {
            Type type = typeof(EmployeeData);
            return listColumn.All(column => type.GetField(column) != null);
        }

        private List<EmployeeData> ImportExcel(ExcelPackage package, out string msg)
        {
            msg = string.Empty;
            ExcelWorksheet workSheet = package.Workbook.Worksheets.First();
            List<EmployeeData> listEmployeeData = new List<EmployeeData>();
            List<string> listColumn = new List<string>();
            int line = 1;
            
            /*get Header*/
            foreach (ExcelRangeBase firstRowCell in workSheet.Cells[1, 1, 1, workSheet.Dimension.End.Column])
            {
                listColumn.Add(firstRowCell.Text);
            }
            if (CheckTemplateFormat(listColumn) == false)
            {
                msg = MessageDefinitionEnum.FileNotFollowTemplate;
                return null;
            }
            /*get Body*/
            Type type = typeof(EmployeeData);

            for (int rowNumber = 2; rowNumber <= workSheet.Dimension.End.Row; rowNumber++)
            {
                ++line;
                EmployeeData employeeData = new EmployeeData();

                for (int columnNumber = 1; columnNumber <= workSheet.Dimension.End.Column; columnNumber++)
                {
                    string column = listColumn[columnNumber - 1];
                    string currentCell = workSheet.Cells[rowNumber, columnNumber].Text;
                    if (columnNumber == ImportColumnEnum.EmployeeID || columnNumber == ImportColumnEnum.FullName)
                        if (string.IsNullOrWhiteSpace(currentCell))
                        {
                            msg = string.Format(MessageDefinitionEnum.ColumnAtLineEmpty, column, line);
                            break;
                        }

                    if (columnNumber == ImportColumnEnum.DateOfBirth || columnNumber == ImportColumnEnum.BeginWorkDate || columnNumber == ImportColumnEnum.ContractDate || columnNumber == ImportColumnEnum.DateOfIssue)
                    {
                        string dateFormat = null;
                        if (!string.IsNullOrWhiteSpace(currentCell))
                        {
                            DateTime parsedDate;
                            string[] arrayDateFormat = { "dd/MM/yyyy", "dd/M/yyyy", "d/MM/yyyy", "d/M/yyyy" };
                            bool isDate = DateTime.TryParseExact(currentCell, arrayDateFormat, 
                                CultureInfo.InvariantCulture, DateTimeStyles.None, out parsedDate);
                            if (!isDate)
                            {
                                msg = string.Format(MessageDefinitionEnum.ColumnAtLineIsNotDateFormat, column, line);
                                break;
                            }
                            dateFormat = parsedDate.ToString(PatternEnum.Date);
                        }
                        type.GetField(column).SetValue(employeeData, dateFormat);
                    }
                    else 
                        type.GetField(column).SetValue(employeeData, workSheet.Cells[rowNumber, columnNumber].Text.Trim());
                }
                listEmployeeData.Add(employeeData);
            }
            return listEmployeeData;
        }

        protected void Upload(object sender, EventArgs e)
        {
            try
            {
                if (fupFile.HasFile == false)
                    return;
                int maxFileSize = 5 * 1024 * 1024; //in MB
                if (fupFile.PostedFile.ContentLength > maxFileSize)
                {
                    ShowMessage(MessageDefinitionEnum.FileExceedSize, ModuleMessage.ModuleMessageType.RedError);
                    return;
                }

                string fileExtension = Path.GetExtension(fupFile.FileName)?.ToLower();
                if (fileExtension != ".xlsx" && fileExtension != ".xls")
                    return;
                
                ExcelPackage package = new ExcelPackage(fupFile.FileContent);
                Dictionary<string, string> dictionary = new Dictionary<string, string>
                {
                    { EmployeeTable.ImportUserID, UserInfo.UserID.ToString()}
                };
                string msg;
                List<EmployeeData>  listEmployeeData = ImportExcel(package, out msg);
                if (msg != string.Empty)
                {
                    ShowMessage(msg,
                        ModuleMessage.ModuleMessageType.RedError);
                    return;
                }
                string message;
                bool result = EmployeeBusiness.InsertEmployee(listEmployeeData, dictionary, out message);
                ShowMessage(message,
                    result
                        ? ModuleMessage.ModuleMessageType.GreenSuccess
                        : ModuleMessage.ModuleMessageType.RedError);
                
            }
            catch (Exception exception)
            {
                ShowException(exception);
            }
        }
    }
}