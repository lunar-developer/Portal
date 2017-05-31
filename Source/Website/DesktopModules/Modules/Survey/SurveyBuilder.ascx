<%@ Control Language="C#" AutoEventWireup="false" CodeFile="SurveyBuilder.ascx.cs" Inherits="DesktopModules.Modules.Survey.SurveyBuilder" %>
<%@ Import Namespace="Website.Library.Global" %>
<%@ Register Src="~/controls/LabelControl.ascx" TagName="Label" TagPrefix="dnn" %>

<link href="<%= FunctionBase.GetAbsoluteUrl("/DesktopModules/Modules/Survey/Assets/css/survey.css") %>" rel="stylesheet" />
<link href="<%= FunctionBase.GetAbsoluteUrl("/DesktopModules/Modules/Survey/Assets/css/surveyeditor.css") %>" rel="stylesheet" />

<script src="<%= FunctionBase.GetAbsoluteUrl("/DesktopModules/Modules/Survey/Assets/js/ace.min.js") %>"></script>
<script src="<%= FunctionBase.GetAbsoluteUrl("/DesktopModules/Modules/Survey/Assets/js/worker-json.js") %>"></script>
<script src="<%= FunctionBase.GetAbsoluteUrl("/DesktopModules/Modules/Survey/Assets/js/mode-json.js") %>"></script>
<script src="<%= FunctionBase.GetAbsoluteUrl("/DesktopModules/Modules/Survey/Assets/js/knockout.js") %>"></script>
<script src="<%= FunctionBase.GetAbsoluteUrl("/DesktopModules/Modules/Survey/Assets/js/survey-knockout@0.12.7.js") %>"></script>
<script src="<%= FunctionBase.GetAbsoluteUrl("/DesktopModules/Modules/Survey/Assets/js/surveyeditor.js") %>"></script>
      
      
<div class="col-lg-12">
    <asp:UpdatePanel ID="UpdatePanel"
                     runat="server">
        <ContentTemplate>
            <div class="form-horizontal">
                <div class="form-group">
                    <asp:PlaceHolder ID="phMessage"
                                     runat="server" />
                </div>
            </div>
            <div class="form-horizontal">
            <div class="form-group">
                <div class="col-sm-3">
                    <asp:DropDownList CssClass="form-control c-theme"
                                      ID="ddlSurvey"
                                      runat="server">
                    </asp:DropDownList>
                </div>
                <div class="col-sm-3">
                    <asp:Button CssClass="btn btn-primary c-margin-t-0"
                                ID="btnEditSurvey"
                                OnClick="FindSurvey"
                                runat="server"
                                Text="Edit Survey" />
                    <asp:Button CssClass="btn btn-success c-margin-t-0"
                                ID="btnListSurvey"
                                OnClick="GetListSurvey"
                                runat="server"
                                Text="List Survey" />
                </div>
                <div id="divCheckBox"
                     runat="server">
                    <div class="col-sm-2 control-label">
                        <dnn:Label ID="lblStateSurvey"
                                   runat="server"
                                   Text="Enable Survey" />
                    </div>
                    <div class="col-sm-2">
                        <div class="c-checkbox has-info">
                            <asp:CheckBox ID="chkStateSurvey"
                                          runat="server" />
                            <label for="<%= chkStateSurvey.ClientID %>">
                            <span class="inc"></span>
                            <span class="check"></span>
                            <span class="box"></span>
                        </div>
                    </div>
                    <div class="col-sm-4"></div>
                </div>
            </div>
            <asp:TextBox CssClass="form-control hidden"
                         ID="surveyContent"
                         runat="server">
            </asp:TextBox>
            <asp:Button CssClass="btn btn-primary hidden c-margin-t-0"
                        ID="btnReceiveSurvey"
                        OnClick="SaveSurveyContent"
                        runat="server"
                        Text="Export" />
            <asp:TextBox CssClass="form-control hidden"
                         ID="editSurveyContent"
                         runat="server">
            </asp:TextBox>
            <asp:TextBox CssClass="form-control hidden"
                         ID="editSurveyId"
                         runat="server">
            </asp:TextBox>
            <asp:Button CssClass="btn btn-primary hidden c-margin-t-0"
                        ID="btnSaveEditSurvey"
                        OnClick="SaveEditSurvey"
                        runat="server"
                        Text="Export" />
            <div class="survey-editor"
                 id="editor">
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</div>
<script type="text/javascript">
    addPageLoaded(function()
    {
        new SurveyEditor.SurveyEditor("editor");
    }, false);
</script>