<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserConfiguration.ascx.cs" Inherits="DesktopModules.Modules.Applic.UserConfiguration" %>
<%@ Import Namespace="Website.Library.Global" %>
<%@ Import Namespace="Modules.UserManagement.Database" %>
<%@ Register TagPrefix="dnn" Assembly="DotNetNuke.Web.Deprecated" Namespace="DotNetNuke.Web.UI.WebControls" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<%@ Register TagPrefix="control" Namespace="Modules.Controls" Assembly="Modules.Controls" %>
<style>
     .listBox-Control-btnGroup
     {
         width: 90% !important;
     }
     .listBox-Control-btn
     {
         margin-top: 0px !important;
         border: 0 !important;
         padding-left: 0 !important;
         padding-right: 0 !important;
     }
    .listBox-Control-btn:hover
    {
        background: #EEEEEE;
    }
</style>
<asp:UpdatePanel ID="updatePanel"
                 runat="server">
<ContentTemplate>
<div class="c-content-title-1 clearfix c-margin-b-20 c-title-md">
    <h3 class="c-font-bold c-font-uppercase">PHÂN CÔNG USER</h3>
    <div class="c-bg-blue c-line-left"></div>
</div>
<div class="form-horizontal">
<div class="form-group">
    <asp:PlaceHolder ID="phMessage"
                     runat="server" />
</div>
    <div class="row">
        <div class="col-md-12">
            <div class="form-group" runat="server" ID="rowtxtApplication">
                <div class="row">
                    <div class="col-md-3"></div>
                    <div class="col-md-2 control-label">
                        <dnn:Label ID="lblApplicationTypeTxt"
                               runat="server" />
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox CssClass="form-control c-theme"
                            ID="txtApplicationType" Enabled="False"
                            runat="server" />
                    </div>
                    <div class="col-md-3"></div>
                </div>
            </div>
            <div class="form-group" runat="server" ID="rowddlApplication">
                <div class="row">
                    <div class="col-md-3"></div>
                    <div class="col-md-2 control-label">
                        <dnn:Label ID="lblApplicationType"
                               runat="server" />
                    </div>
                    <div class="col-md-4">
                        <control:Combobox CssClass="form-control c-theme" 
                                      ID="ddlApplicationType"
                                      OnSelectedIndexChanged="ApplicationTypeChange"
                                      AutoPostBack="True"
                                      runat="server" />
                    </div>
                    <div class="col-md-3"></div>
                </div>
            </div>
            <div class="form-group" runat="server" ID="rowtxtPhase">
                <div class="row">
                    <div class="col-md-3"></div>
                    <div class="col-md-2 control-label">
                        <dnn:Label ID="lblPhasetxt"
                               runat="server" />
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox CssClass="form-control c-theme"
                            ID="txtPhase" Enabled="False"
                            runat="server" />
                    </div>
                    <div class="col-md-3"></div>
                </div>
            </div>
            <div class="form-group" runat="server" ID="rowddlPhase">
                <div class="row">
                    <div class="col-md-3"></div>
                    <div class="col-md-2 control-label">
                        <dnn:Label ID="lblPhase"
                               runat="server" />
                    </div>
                    <div class="col-md-4">
                        <control:Combobox CssClass="form-control c-theme" 
                                      ID="ddlPhase"
                                      OnSelectedIndexChanged="PhaseCodeChange"
                                      AutoPostBack="True"
                                      runat="server" />
                    </div>
                    <div class="col-md-3"></div>
                </div>
            </div>
            <div class="form-group" runat="server" ID="rowtxtUser">
                <div class="row">
                    <div class="col-md-3"></div>
                    <div class="col-md-2 control-label">
                        <dnn:Label ID="lblUser"
                               runat="server" />
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox CssClass="form-control c-theme"
                            ID="txtUser" Enabled="False"
                            runat="server" />
                    </div>
                    <div class="col-md-3"></div>
                </div>
            </div>
            <div class="form-group" runat="server" ID="rowddlUser">
                <div class="row">
                    <div class="col-md-3"></div>
                    <div class="col-md-2 control-label">
                        <dnn:Label ID="lblUserList"
                               runat="server" />
                    </div>
                    <div class="col-md-4">
                        <control:Combobox CssClass="form-control c-theme" 
                                      ID="ddlUser"
                                      OnSelectedIndexChanged="UserIDChange"
                                      AutoPostBack="True"
                                      runat="server" />
                    </div>
                    <div class="col-md-3"></div>
                </div>
            </div>
            <div class="form-group">
                <div class="row">
                    <div class="col-md-3"></div>
                    <div class="col-md-2 control-label">
                        <dnn:Label ID="lblKpi"
                               runat="server" />
                    </div>
                    <div class="col-md-4 control-value">
                        <asp:TextBox CssClass="form-control c-theme"
                                 ID="txtKpi"
                                 Rows="2"
                                 runat="server" />
                    </div>
                    <div class="col-md-3"></div>
                </div>
            </div>
            <div runat="server" ID="DivAbSentCalendar"></div>
        </div>
    </div>
    <div class="row">
        <div class="col-md-2"></div>
        <div class="col-md-8">
            <table style="margin: 0 auto;">
                <colgroup>
                    <col width="44%"/>
                    <col width="2%"/>
                    <col width="8%"/>
                    <col width="2%"/>
                    <col width="44%"/>
                </colgroup>
                <thead>
                    <th class="control-label">
                        <dnn:Label ID="lblPolicyNonSelected"
                               runat="server" />
                    </th>
                    <th class=""></th>
                    <th class=""></th>
                    <th class=""></th>
                    <th class="control-label">
                        <dnn:Label ID="lblPolicySelected"
                                   runat="server"/>
                    </th>
                </thead>
                <tbody>
                    <td class="text-left">
                        <div class="form-group">
                            <asp:ListBox runat="server"  CssClass="form-control" ID="ddlPolicyNonSelected"
                                SelectionMode="Multiple" Height="150px" Width="100%">
                
                            </asp:ListBox>
                        </div>
                    </td>
                    <td></td>
                    <td class="text-center">
                        <div class="btn-group-vertical btn-group-sm listBox-Control-btnGroup" style="margin: 0 auto;" role="group">
                            <asp:LinkButton runat="server" CssClass="btn btn-small listBox-Control-btn"
                                OnClick="AddSelectedPolicy">
                                <i class="fa fa-chevron-right" aria-hidden="true"></i>
                            </asp:LinkButton>
                            <asp:LinkButton runat="server" CssClass="btn btn-small listBox-Control-btn"
                                OnClick="SelectAllPolicy">
                                <i class="fa fa-forward" aria-hidden="true"></i>
                            </asp:LinkButton>
                            <asp:LinkButton runat="server" CssClass="btn btn-small listBox-Control-btn"
                                OnClick="RemoveSelectedPolicy">
                                <i class="fa fa-chevron-left" aria-hidden="true"></i>
                            </asp:LinkButton>
                            <asp:LinkButton runat="server" CssClass="btn btn-small listBox-Control-btn"
                                OnClick="RemoveAllPolicy">
                                <i class="fa fa-backward" aria-hidden="true"></i>
                            </asp:LinkButton>
                        </div>
                    </td>
                    <td></td>
                    <td class="text-right">
                        <div class="form-group">
                            <asp:ListBox runat="server" CssClass="form-control" ID="ddlPolicySelected"
                                SelectionMode="Multiple" Height="150px" Width="100%">
                
                            </asp:ListBox>
                        </div>
                    </td>
                </tbody>
            </table>
        </div>
        <div class="col-md-2"></div>
    </div>
    <hr class="c-margin-t-40 separator" />
    <div class="form-group"
         id="DivProfileRemark"
         runat="server">
        <div class="col-sm-2 control-label">
            <dnn:Label ID="lblRemark"
                       runat="server" />
        </div>
        <div class="col-sm-10">
            <asp:TextBox CssClass="form-control c-theme"
                         ID="txtRemark"
                         Rows="2"
                         runat="server"
                         TextMode="MultiLine" />
        </div>
    </div>
    <div class="form-group">
         <div class="col-sm-4"></div>
         <div class="col-sm-4" style="text-align: center;">
                <asp:Button CssClass="btn btn-primary"
                            ID="btnAddUserProcess"
                            OnClick="ChangeUserAssignment"
                            runat="server" />
         </div>
         <div class="col-sm-4"></div>
     </div>
</div>
    <asp:HiddenField ID="hidKeyID"
                 runat="server"
                 Value="-1"
                 Visible="False" /> 
    <asp:HiddenField ID="hidUserID"
                 runat="server"
                 Value="-1"
                 Visible="False" />
    <asp:HiddenField ID="hidRequestType"
                 runat="server"
                 Value="-1"
                 Visible="False" />
    <asp:HiddenField ID="hidPhaseCode"
                 runat="server"
                 Value="-1"
                 Visible="False" />
    <asp:HiddenField ID="hidApplicationTypeCode"
                 runat="server"
                 Value="-1"
                 Visible="False" />

</ContentTemplate>
</asp:UpdatePanel>
