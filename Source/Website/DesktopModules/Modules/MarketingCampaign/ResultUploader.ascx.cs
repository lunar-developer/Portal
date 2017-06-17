using DotNetNuke.UI.Skins.Controls;
using System;
using System.Collections.Generic;
using Modules.MarketingCampaign.Business;
using Modules.MarketingCampaign.DataTransfer;
using Modules.MarketingCampaign.Global;
using OfficeOpenXml;
using Website.Library.Global;

namespace DesktopModules.Modules.MarketingCampaign
{
    public partial class ResultUploader : MarketingCampaignBase
	{
        protected override void OnLoad(EventArgs e)
        {
        }

        protected void Upload(object sender, EventArgs e)
        {
            if (fupFile.HasFile)
            {
                try
                {
                    string extension = GetExtension(fupFile?.PostedFile?.FileName);
                    List<ResultData> listResult;
                    string message;
                    if (extension.Equals("xls") || extension.Equals("xlsx"))
                    {
                        ExcelPackage package = new ExcelPackage(fupFile?.FileContent);
                        listResult = ImportExcel<ResultData>(package);
                    }
                    else
                    {
                        listResult = FunctionBase.ImportCSV<ResultData>(fupFile?.FileContent);
                    }
                    bool result = ResultBusiness.InsertResult(listResult, out message);
                    ShowMessage(message,
                        result
                            ? ModuleMessage.ModuleMessageType.GreenSuccess
                            : ModuleMessage.ModuleMessageType.RedError);
                }
                catch (Exception exception)
                {
                    ShowMessage(exception.Message, ModuleMessage.ModuleMessageType.RedError);
                }
            }
            else
            {
                ShowMessage("Không tìm thấy file upload", ModuleMessage.ModuleMessageType.RedError);
            }
        }
    }
}