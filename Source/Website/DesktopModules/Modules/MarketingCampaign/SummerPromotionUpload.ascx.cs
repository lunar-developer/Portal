using System;
using System.Collections.Generic;
using DotNetNuke.UI.Skins.Controls;
using Modules.MarketingCampaign.Business;
using Modules.MarketingCampaign.DataTransfer;
using Modules.MarketingCampaign.Global;
using OfficeOpenXml;
using Website.Library.Global;

namespace DesktopModules.Modules.MarketingCampaign
{
    public partial class SummerPromotionUpload : MarketingCampaignBase
    {
        protected override void OnLoad(EventArgs e)
        {
            if (IsPostBack == false)
            {
            }
        }

        protected void Upload(object sender, EventArgs e)
        {
            if (fupFile.HasFile)
            {
                try
                {
                    string extension = MarketingCampaignBase.GetExtension(fupFile?.PostedFile?.FileName);
                    List<SummerPromotionData> listResult;
                    string message;
                    if (extension.Equals("xls") || extension.Equals("xlsx"))
                    {
                        ExcelPackage package = new ExcelPackage(fupFile?.FileContent);
                        listResult = MarketingCampaignBase.ImportExcel<SummerPromotionData>(package);
                    }
                    else
                    {
                        listResult = FunctionBase.ImportCSV<SummerPromotionData>(fupFile?.FileContent);
                    }
                    bool result = SummerPromotionBusiness.InsertResult(listResult, out message);
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