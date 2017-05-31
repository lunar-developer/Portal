using System;
using System.Collections.Generic;
using DotNetNuke.UI.Skins.Controls;
using Modules.Survey.Business;
using Modules.Survey.DataTransfer;
using Website.Library.Global;

namespace DesktopModules.Modules.Survey
{
    public partial class SurveyForm : DesktopModuleBase
    {
        protected override void OnLoad(EventArgs e)
        {
            if (IsPostBack)
            {
                return;
            }
            BindDataSurvey();
        }

        public void BindDataSurvey()
        {
            int surveyID;
            if(int.TryParse(Request.QueryString["id"], out surveyID) == false || surveyID <=0)
            {
                ShowMessage("Không tìm thấy thông tin bạn yêu cầu.");
                return;
            }

            if (new SvResultBusiness().CheckSurveyIsEnabled(surveyID) == false)
            {
                ShowMessage("Chương trình khảo sát này đã kết thúc!");
                return;
            }

            if (new SvResultBusiness().CheckSurveyForClient(surveyID, Request.UserHostAddress))
            {
                ShowMessage("Bạn đã thực hiện chương trình khảo sát này!");
                return;
            }

            SvSurveyData survey = new SvSurveyBusiness().FindSurvey(surveyID);
            if (survey == null)
            {
                ShowMessage("Không tìm thấy thông tin bạn yêu cầu.");
                return;
            }
            string registerSurvey = $@"renderSurvey({survey.Survey});";
            RegisterScript(registerSurvey);
        }

        protected void SaveResultSurvey(object sender, EventArgs eventArgs)
        {
            int idSurvey = int.Parse(Request.QueryString["id"]);
            string requestID = Request.UserHostAddress;
            string result = txtSurveyReult.Text;
            Dictionary<string, string> values = FunctionBase.Deserialize<Dictionary<string, string>>(result);
            int resultAdd = new SvResultBusiness().AddResult(idSurvey, requestID, values);
            if (resultAdd > 0)
            {
                ShowMessage("Cám ơn bạn đã dành thời gian tham gia.", ModuleMessage.ModuleMessageType.GreenSuccess);
            }
            else
            {
                ShowMessage("Không thể kết nối với Server", ModuleMessage.ModuleMessageType.RedError);
            }
        }
    }
}