using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using OfficeOpenXml;
using Website.Library.Global;

namespace Modules.MarketingCampaign.Global
{
    public class MarketingCampaignBase: DesktopModuleBase
    {
        public static string GetExtension(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName)) return string.Empty;
            string[] part = fileName.Split('.');
            return part.Length == 0 ? string.Empty : part[part.Length - 1]?.ToLower();
        }

        public static DataTable ToDataTable(ExcelPackage package)
        {
            ExcelWorksheet workSheet = package.Workbook.Worksheets.First();
            DataTable table = new DataTable();

            /*get Header*/
            foreach (var firstRowCell in workSheet.Cells[1, 1, 1, workSheet.Dimension.End.Column])
            {
                table.Columns.Add(firstRowCell.Text);
            }
            /*get Body*/
            for (var rowNumber = 2; rowNumber <= workSheet.Dimension.End.Row; rowNumber++)
            {
                var row = workSheet.Cells[rowNumber, 1, rowNumber, workSheet.Dimension.End.Column];
                var newRow = table.NewRow();
                foreach (var cell in row)
                {
                    newRow[cell.Start.Column - 1] = cell.Text;
                }
                table.Rows.Add(newRow);
            }
            return table;
        }
        public static List<T> ImportExcel<T>(ExcelPackage package) where T : class
        {
            Type type = typeof(T);
            List<T> listResult = new List<T>();
            List<string> listFields = new List<string>();
            //
            ExcelWorksheet workSheet = package.Workbook.Worksheets.First();

            /*get Header*/
            foreach (var firstRowCell in workSheet.Cells[1, 1, 1, workSheet.Dimension.End.Column])
            {
                listFields.Add(firstRowCell?.Text?.Trim());
            }

            /*get Body*/
            for (var rowNumber = 2; rowNumber <= workSheet.Dimension.End.Row; rowNumber++)
            {
                var row = workSheet.Cells[rowNumber, 1, rowNumber, workSheet.Dimension.End.Column];
                List<string> listData = new List<string>();
                foreach (var cell in row)
                {
                    listData.Add(cell.Text?.Trim());
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
            return listResult;
        }

        private static readonly string Event01Url =
            FunctionBase.GetAbsoluteUrl("/DesktopModules/Modules/MarketingCampaign/Assets/Template/TemplateQQHD.xlsx");
        private static readonly string Event02Url =
            FunctionBase.GetAbsoluteUrl("/DesktopModules/Modules/MarketingCampaign/Assets/Template/TemplateCTDHMuaHeRucNang.xlsx");

        protected string LinkTemplateEvent01 => Event01Url;
        protected string LinkTemplateEvent02 => Event02Url;
    }
}
