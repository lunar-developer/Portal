using DotNetNuke.UI.Skins.Controls;
using System;
using System.Collections.Generic;
using Modules.MarketingCampaign.Business;
using Modules.MarketingCampaign.DataTransfer;
using Website.Library.Global;

namespace DesktopModules.Modules.MarketingCampaign
{
    public partial class ResultUploader : DesktopModuleBase
	{
        protected override void OnLoad(EventArgs e)
        {
            if (IsPostBack == false)
            {
            }
        }

        protected void Upload(object sender, EventArgs e)
        {
            try
            {
                string message;
                List<ResultData> listResult = FunctionBase.ImportCSV<ResultData>(fupFile.FileContent);
                bool result = ResultBusiness.InsertResult(listResult, out message);
                ShowMessage(message, result ? ModuleMessage.ModuleMessageType.GreenSuccess : ModuleMessage.ModuleMessageType.RedError);
            }
            catch (Exception exception)
            {
                ShowMessage(exception.Message, ModuleMessage.ModuleMessageType.RedError);
            }
        }
    }
}