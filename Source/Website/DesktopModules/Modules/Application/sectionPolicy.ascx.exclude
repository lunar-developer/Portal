<%@ Control Language="C#" AutoEventWireup="true" CodeFile="SectionPolicy.ascx.cs" Inherits="DesktopModules.Modules.Applic.SectionPolicy" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<%@ Register TagPrefix="control" TagName="DoubleLabel" Src="~/controls/DoubleLabel.ascx" %>
<asp:UpdatePanel ID="SectionAssessmentPanel"
                 runat="server">
    <ContentTemplate>
        <div class="row">
            <div class="col-sm-6">
                <div class="form-group">
                    <div class="col-sm-4 control-label">
                        <dnn:Label ID="lblApplicID" runat="server" />
                    </div>
                    <div class="col-sm-8 control-value">
                        <asp:TextBox CssClass="form-control c-theme"
                                     ID="ctApplicID"
                                     runat="server" ReadOnly="False" />
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-sm-4 control-label"><dnn:Label ID="lblApplicationType" runat="server" /></div>
                    <div class="col-sm-8 control-value">
                        <asp:DropDownList CssClass = "form-control c-theme"
                                          ID = "ctApplicationType"
                                          runat = "server">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-sm-4 control-label"><dnn:Label ID="lblPriority" runat="server" /></div>
                    <div class="col-sm-8 control-value">
                        <asp:DropDownList CssClass = "form-control c-theme"
                                          ID = "ctPriority"
                                          runat = "server">
                        </asp:DropDownList>
                    </div>
                </div>
            </div>
            <div class="col-sm-6">
                <div class="form-group">
                    <div class="col-sm-4 control-label"><dnn:Label ID="lblApplicStatus" runat="server" /></div>
                    <div class="col-sm-8 control-value">
                        <asp:TextBox CssClass="form-control c-theme"
                                     ID="ctApplicStatus"
                                     runat="server" ReadOnly="False" />
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-sm-4 control-label"><dnn:Label ID="lblDecision" runat="server" /></div>
                    <div class="col-sm-8 control-value">
                        <asp:TextBox CssClass="form-control c-theme"
                                     ID="ctDecision"
                                     runat="server" ReadOnly="True" />
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-sm-4 control-label"><dnn:Label ID="lblIncompleteReason" runat="server" /></div>
                    <div class="col-sm-8 control-value">
                        <asp:TextBox CssClass="form-control c-theme"
                                     ID="ctIncompleteReason"
                                     runat="server" />
                    </div>
                </div>
            </div>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>

