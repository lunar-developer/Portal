<%@ Control Language="C#" AutoEventWireup="false" CodeFile="SurveyList.ascx.cs" Inherits="DesktopModules.Modules.Survey.SurveyList" %>
<%@ Import Namespace="Website.Library.Global" %>
<script src="<%=FunctionBase.GetAbsoluteUrl("/DesktopModules/Modules/Survey/Assets/js/survey.jquery.min.js")%>"></script>
<asp:UpdatePanel ID="updatePanel"
                 runat="server">
   
    <ContentTemplate>
        <div class = "form-horizontal">
            <div class = "form-group">
                <asp:PlaceHolder ID = "phMessage" runat = "server" />
            </div>
        </div>
        <div class="form-horizontal" runat="server" ID="tableListSurvey">
        </div>
    </ContentTemplate>
</asp:UpdatePanel>



