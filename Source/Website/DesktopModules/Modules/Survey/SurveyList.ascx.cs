using System;
using System.Collections.Generic;
using System.Net;
using System.Web;
using DocumentFormat.OpenXml.Office2010.ExcelAc;
using DotNetNuke.UI.Skins.Controls;
using Modules.Survey.Business;
using Modules.Survey.DataTransfer;
using Website.Library.Global;

namespace DesktopModules.Modules.Survey
{
    public partial class SurveyList : DesktopModuleBase
    {
        protected override void OnLoad(EventArgs e)
        {
            if (IsPostBack)
            {
                return;
            }
            BindListSurvey();
        }

        public void BindListSurvey()
        {
            var listSurvey = new SvSurveyBusiness().GetListSurveyToAnswer();
            string html = $@"<table class=""table table-hover"">
                                            <thead>
                                                <tr>
                                                    <th>STT</th>
                                                    <th>Survey Name</th>
                                                    <th>Link survey</th>
                                                </tr>
                                            </thead>
                                                <tbody>";
            int stt = 1;
            foreach (var item in listSurvey)
            {
                html += $@"<tr>
                            <td>{stt}</td>
                            <td>{item.Name}</td>
                            <td><a href=""{FunctionBase.GetAbsoluteUrl("/Survey-Form/id=")+item.Id}"" title="""">{FunctionBase.GetAbsoluteUrl("/Survey-Form/id=") + item.Id}</a></td>
                           </tr>";
                stt++;
            }
            html += $@" </tbody></table>";
            tableListSurvey.InnerHtml = html;
        }
    }
}