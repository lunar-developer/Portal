using System;
using System.Web.UI.WebControls;
using DotNetNuke.UI.Skins.Controls;
using Modules.Survey.Business;
using Modules.Survey.DataTransfer;
using Website.Library.Global;


namespace DesktopModules.Modules.Survey { 
    public partial class SurveyBuilder : DesktopModuleBase
    {
        #region PAGE EVENTS

        protected override void OnLoad(EventArgs e)
        {
            if (IsPostBack)
            {
                return;
            }
            BindDataToListSurvey();
            divCheckBox.Visible = false;
        }

        #endregion

        protected void SaveSurveyContent(object sender, EventArgs eventArgs)
        {

            int resutlAddSurvey = new SvSurveyBusiness().AddSurvey(new SvSurveyData { Survey = surveyContent.Text.Replace("\r\n", string.Empty), IsEnable = "1" });
            if (resutlAddSurvey > 0)
            {
                ShowMessage("Add survey success!", ModuleMessage.ModuleMessageType.GreenSuccess);
                BindDataToListSurvey();
            }
            else
                ShowMessage("Add survey faile!", ModuleMessage.ModuleMessageType.RedError);
        }

        public void BindDataToListSurvey()
        {
            var listSurvey = new SvSurveyBusiness().GetListSurvey();
            ddlSurvey.Items.Add(new ListItem("Choose survey",0.ToString()));
            foreach (var item in listSurvey)
            {
                ddlSurvey.Items.Add(new ListItem(item.Name,item.Id.ToString()));
            }
            ddlSurvey.SelectedIndex = 0;   
        }

        protected void FindSurvey(object sender, EventArgs eventArgs)
        {
            int id = int.Parse(ddlSurvey.SelectedValue);
            editSurveyId.Text = id.ToString();
            SvSurveyData survey = new SvSurveyBusiness().FindSurvey(id);
            
            string script = @"new SurveyEditor.SurveyEditor(""editor"").loadSurvey(" + survey.Survey + ");" +
                            "$('#btnSaveEditSurvey').removeClass('hidden');" +
                            "$('#btnSaveEditor').addClass('hidden');";
            RegisterScript(script);
            if (survey.IsEnable.Equals("True"))
            {
                chkStateSurvey.Checked = true;
                divCheckBox.Visible = true;
            }
            else
            {
                chkStateSurvey.Checked = false;
                divCheckBox.Visible = true;
            }
        }

        protected void GetListSurvey(object sender, EventArgs eventArgs)
        {
            string script = EditUrl("List", 800, 400, true, true, null, "tableId","fd");
            RegisterScript(script);
        }

        protected void SaveEditSurvey(object sender, EventArgs eventArgs)
        {

            var enableSurvey = chkStateSurvey.Checked ? "1" : "0";
            
            int resultUpdate = new SvSurveyBusiness().UpdateSurvey(new SvSurveyData
            {
                Id = editSurveyId.Text,
                Survey = editSurveyContent.Text.Replace("\r\n", string.Empty),
                IsEnable = enableSurvey
            });
            if (resultUpdate > 0)
            {
                ShowMessage("Update survey success!", ModuleMessage.ModuleMessageType.GreenSuccess);
            }
            else
            {
                ShowMessage("Update survey faile!", ModuleMessage.ModuleMessageType.RedError);
            }
        }
    }
}