<%@ Control Language="C#" AutoEventWireup="false" CodeFile="SurveyForm.ascx.cs" Inherits="DesktopModules.Modules.Survey.SurveyForm" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.Web.Client.ClientResourceManagement" Assembly="DotNetNuke.Web.Client" %>
<dnn:DnnJsInclude FilePath="DesktopModules/Modules/Survey/Assets/js/survey.jquery.min.js"
                  ForceBundle="True"
                  ForceVersion="True"
                  runat="server" />
<dnn:DnnCssInclude FilePath="DesktopModules/Modules/Survey/Assets/css/style.css"
                   ForceBundle="True"
                   ForceVersion="True"
                   runat="server" />

<asp:UpdatePanel ID="updatePanel"
                 runat="server">
    <ContentTemplate>
        <div class="form-horizontal">
            <div class="form-group">
                <asp:PlaceHolder ID="phMessage"
                                 runat="server" />
            </div>
        </div>
        <div id="surveyContainer"></div>
        <asp:TextBox CssClass="form-control hidden"
                     ID="txtSurveyReult"
                     runat="server">
        </asp:TextBox>
        <asp:Button CssClass="btn btn-primary hidden"
                    ID="btnSaveReult"
                    OnClick="SaveResultSurvey"
                    runat="server"
                    Text="Save" />
    </ContentTemplate>
</asp:UpdatePanel>

<script type="text/javascript">
    function renderSurvey(json)
    {
        Survey.defaultBootstrapCss.navigationButton = "btn btn-primary";
        Survey.Survey.cssType = "bootstrap";
        Survey.JsonObject.metaData.addProperty("radiogroup",
            {
                "name": "renderAs",
                "default": "standard",
                "choices": ["standard", "icheck"]
            });
        var surveyJson = json;
        var survey = new Survey.Model(surveyJson);

        function sendDataToServer(survey)
        {
            getJQueryControl("txtSurveyReult").val(JSON.stringify(survey.data));
            getControl("btnSaveReult").click();
        }

        $("#surveyContainer").Survey({
            model: survey,
            onComplete: sendDataToServer
        });
    }
</script>