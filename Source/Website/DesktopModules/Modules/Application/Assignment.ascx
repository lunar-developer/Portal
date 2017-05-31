<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Assignment.ascx.cs" Inherits="DesktopModules.Modules.Applic.Assignment" %>
<%@ Import Namespace="Website.Library.Global" %>
<%@ Import Namespace="Modules.UserManagement.Database" %>
<%@ Import Namespace="Modules.Applic.Database" %>
<%@ Import Namespace="Modules.Applic.Enum" %>
<%@ Register TagPrefix="dnn" Assembly="DotNetNuke.Web.Deprecated" Namespace="DotNetNuke.Web.UI.WebControls" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<%@ Register TagPrefix="control" Namespace="Modules.Controls" Assembly="Modules.Controls" %>
<asp:UpdatePanel ID="updatePanel"
                 runat="server">
<ContentTemplate>
<div class="c-content-title-1 clearfix c-margin-b-20 c-title-md">
    <h3 class="c-font-bold c-font-uppercase">DANH SÁCH PHÂN CÔNG THỰC HIỆN HỒ SƠ</h3>
    <div class="c-bg-blue c-line-left"></div>
</div>
<div class="form-horizontal">
<div class="form-group">
    <asp:PlaceHolder ID="phMessage"
                     runat="server" />
</div>
    <div class="form-group">
        <div class="row">
            <div class="col-md-2"></div>
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
            <div class="col-md-4"></div>
        </div>
    </div>
    <div class="form-group">
        <div class="row">
            <div class="col-md-2"></div>
            <div class="col-md-2 control-label">
                <dnn:Label ID="lblPhase"
                       runat="server" />
            </div>
            <div class="col-md-4">
                <control:Combobox CssClass="form-control c-theme" 
                              ID="ddlPhase"
                              OnSelectedIndexChanged="PhaseChange"
                              AutoPostBack="True"
                              runat="server" />
            </div>
            <div class="col-md-4"></div>
        </div>
    </div>
    <div id="DivUserProcessContent"
         runat="server">
        <div class="dnnPanels"
             id="DivUserList"
             runat="server">
            <br />
            <div class="form-group">
                <div class="col-sm-12 text-center">
                    <control:Grid AllowPaging="true"
                                 AutoGenerateColumns="false"
                                 CssClass="dnnGrid"
                                 EnableViewState="true"
                                 ID="gridData"
                                 OnPageIndexChanged="OnPageIndexChanging"
                                 OnPageSizeChanged="OnPageSizeChanging"
                                 PageSize="10"
                                 runat="server"
                                 Visible="false"
                                 HorizontalAlign="Center">
                        <MasterTableView>
                            <Columns>
                                <dnn:DnnGridBoundColumn DataField="UserID" Visible="False"
                                                        HeaderText="UserID">
                                    <HeaderStyle Width="0%" />
                                </dnn:DnnGridBoundColumn>
                                <dnn:DnnGridBoundColumn DataField="FullName"
                                                        HeaderText="FullName">
                                    <HeaderStyle Width="25%" HorizontalAlign="Center" VerticalAlign="Middle" />
                                </dnn:DnnGridBoundColumn>
                                <dnn:DnnGridBoundColumn DataField="Username" Visible="False"
                                                        HeaderText="Username">
                                    <HeaderStyle Width="0%" />
                                </dnn:DnnGridBoundColumn>
                                <dnn:DnnGridBoundColumn DataField="ApplicationTypeCode" Visible="False"
                                                        HeaderText="ApplicationTypeCode">
                                    <HeaderStyle Width="0%" />
                                </dnn:DnnGridBoundColumn>
                                <dnn:DnnGridTemplateColumn HeaderText="ApplicationTypeName">
                                    <HeaderStyle Width="20%" HorizontalAlign="Center" VerticalAlign="Middle"/>
                                    <ItemTemplate>
                                        <asp:LinkButton CommandArgument="<%#Eval(UserPhaseMappingTable.UserPhaseMappingID).ToString() %>"
                                                        CssClass="c-edit-link c-theme-color"
                                                        OnClick="EditAssignItem"
                                                        runat="server">
                                            <%#Eval(UserPhaseMappingTable.ApplicationTypeName).ToString() %>
                                        </asp:LinkButton>
                                    </ItemTemplate>
                                </dnn:DnnGridTemplateColumn>
                                <dnn:DnnGridBoundColumn DataField="PhaseCode" Visible="False"
                                                        HeaderText="PhaseCode">
                                    <HeaderStyle Width="0%" />
                                </dnn:DnnGridBoundColumn>
                                <dnn:DnnGridTemplateColumn HeaderText="PhaseName">
                                    <HeaderStyle Width="20%" HorizontalAlign="Center" VerticalAlign="Middle"/>
                                    <ItemTemplate>
                                        <asp:LinkButton CommandArgument="<%#Eval(UserPhaseMappingTable.UserPhaseMappingID).ToString() %>"
                                                        CssClass="c-edit-link c-theme-color"
                                                        OnClick="EditAssignPhase"
                                                        runat="server">
                                            <%#Eval(UserPhaseMappingTable.PhaseName).ToString() %>
                                        </asp:LinkButton>
                                    </ItemTemplate>
                                </dnn:DnnGridTemplateColumn>
                                <dnn:DnnGridTemplateColumn HeaderText="PolicyCode">
                                    <HeaderStyle Width="25%" HorizontalAlign="Center" VerticalAlign="Middle"/>
                                    <ItemTemplate>
                                        <asp:LinkButton CommandArgument="<%#Eval(UserPhaseMappingTable.UserPhaseMappingID).ToString() %>"
                                                        CssClass="c-edit-link c-theme-color"
                                                        OnClick="EditAssignPolicy"
                                                        runat="server"
                                                        Visible="<%#EditItemVisble(Eval(UserPhaseMappingTable.PolicyCode).ToString())%>">
                                            <%#Eval(UserPhaseMappingTable.PolicyCode).ToString() %>
                                        </asp:LinkButton>
                                    </ItemTemplate>
                                </dnn:DnnGridTemplateColumn>
                                <dnn:DnnGridBoundColumn DataField="UserPhaseMappingID" Visible="False"
                                                        HeaderText="UserPhaseMappingID">
                                    <HeaderStyle Width="0%" />
                                </dnn:DnnGridBoundColumn>
                                <dnn:DnnGridTemplateColumn Visible="True" HeaderText="#">
                                <HeaderStyle Width="5%" HorizontalAlign="Center" VerticalAlign="Middle" />
                                <ItemTemplate>
                                        <asp:LinkButton CommandArgument="<%#Eval(UserPhaseMappingTable.UserPhaseMappingID).ToString() %>"
                                                        CssClass="c-edit-link c-theme-color"
                                                        OnClick="EditAssignPolicy"
                                                        runat="server">
                                            <img alt = "Edit" src="<%#EditIcon%>">
                                        </asp:LinkButton>
                                </ItemTemplate>
                                </dnn:DnnGridTemplateColumn>
                                <dnn:DnnGridTemplateColumn Visible="True" HeaderText="#">
                                <HeaderStyle Width="5%" HorizontalAlign="Center" VerticalAlign="Middle" />
                                <ItemTemplate>
                                        <asp:LinkButton CommandArgument="<%#Eval(UserPhaseMappingTable.UserPhaseMappingID).ToString() %>"
                                                        CssClass="c-edit-link c-theme-color"
                                                        OnClick="DeleteAssignItem"
                                                        runat="server">
                                            <img alt = "Delete" src="<%#DeleteIcon%>">
                                        </asp:LinkButton>
                               </ItemTemplate>
                           </dnn:DnnGridTemplateColumn>
                            </Columns>
                        </MasterTableView>
                    </control:Grid>
                </div>
            </div>
        </div>
        <hr class="c-margin-t-40 separator" />
        <div class="form-group">
            <div class="col-sm-4"></div>
            <div class="col-sm-4" style="text-align: center;">
                <asp:Button CssClass="btn btn-primary"
                            ID="btnAddUser"
                            OnClick="AddUser"
                            runat="server"
                            Text="Thêm mới" />
            </div>
            <div class="col-sm-4"></div>
        </div>
    </div>
</div> 
</ContentTemplate>
</asp:UpdatePanel>