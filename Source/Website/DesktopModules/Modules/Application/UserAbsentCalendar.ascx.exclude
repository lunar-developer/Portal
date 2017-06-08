<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserAbsentCalendar.ascx.cs" Inherits="DesktopModules.Modules.Applic.UserAbsentCalendar" %>
<%@ Import Namespace="Website.Library.Global" %>
<%@ Import Namespace="Modules.UserManagement.Database" %>
<%@ Import Namespace="Modules.Applic.Database" %>
<%@ Register TagPrefix="dnn" Assembly="DotNetNuke.Web.Deprecated" Namespace="DotNetNuke.Web.UI.WebControls" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<%@ Register TagPrefix="control" Namespace="Modules.Controls" Assembly="Modules.Controls" %>
<asp:UpdatePanel ID="updatePanel"
                 runat="server">
<ContentTemplate>
<div class="c-content-title-1 clearfix c-margin-b-20 c-title-md">
    <h3 class="c-font-bold c-font-uppercase">ĐĂNG KÝ NGHỈ PHÉP</h3>
    <div class="c-bg-blue c-line-left"></div>
</div>
<div class="form-horizontal">
<div class="form-group">
    <asp:PlaceHolder ID="phMessage"
                     runat="server" />
</div>
   <div class="form-group" runat="server" ID="DivUserApplicList">
       <div class="row">
           <div class="col-md-3"></div>
           <div class="col-md-2 control-label">
                <dnn:Label ID="lblUserApplicList"
                               runat="server" />
           </div>
           <div class="col-md-4">
                <control:Combobox CssClass="form-control c-theme" 
                                      ID="ddlApplicUser"
                                      AutoPostBack="True"
                                      runat="server" />
           </div>
           <div class="col-md-3"></div>
       </div>
   </div>
   <div class="form-group" runat="server" ID="DivUser">
       <div class="row">
           <div class="col-md-3"></div>
           <div class="col-md-2 control-label">
                <dnn:Label ID="lblUser"
                               runat="server" />
           </div>
           <div class="col-md-4">
                <asp:TextBox CssClass="form-control c-theme"
                            ID="txtUser" 
                            runat="server" />
           </div>
           <div class="col-md-3"></div>
       </div>
   </div>
    
   
   <div runat="server" ID="DivAbSentCalendar">
        <div class="form-group">
            <div class="col-md-3"></div>
            <div class="col-md-6 text-center">
                <hr class="c-margin-t-40 separator" />
                <h4 class="c-font-uppercase">Thông tin ngày phép</h4>
                <control:Grid AllowPaging="true" HorizontalAlign="Center"
                                 AutoGenerateColumns="false"
                                 CssClass="dnnGrid"
                                 EnableViewState="true"
                                 ID="gridUserAbsent"
                                 OnPageIndexChanged="OnPageAbsentIndexChanging"
                                 OnPageSizeChanged="OnPageAbsentSizeChanging"
                                 PageSize="10"
                                 runat="server"
                                 Visible="false">
                   <MasterTableView>
                       <Columns>
                           <dnn:DnnGridBoundColumn DataField="ID" Visible="False"
                                                        HeaderText="ID">
                                <HeaderStyle Width="0%" />
                           </dnn:DnnGridBoundColumn>
                           <dnn:DnnGridBoundColumn DataField="UserID" Visible="False"
                                                        HeaderText="UserID">
                                <HeaderStyle Width="0%" />
                           </dnn:DnnGridBoundColumn>
                           <dnn:DnnGridBoundColumn DataField="AbsentType" Visible="False" 
                                                        HeaderText="AbsentType">
                                <HeaderStyle Width="0%" />
                           </dnn:DnnGridBoundColumn>
                           <dnn:DnnGridBoundColumn DataField="AbsentTypeName" Visible="True" 
                                                        HeaderText="AbsentTypeName">
                                <HeaderStyle Width="20%" HorizontalAlign="Center" VerticalAlign="Middle" />
                           </dnn:DnnGridBoundColumn>
                           <dnn:DnnGridBoundColumn DataField="FromDate" Visible="False"
                                                        HeaderText="FromDate">
                                <HeaderStyle Width="0%" />
                           </dnn:DnnGridBoundColumn>
                           <dnn:DnnGridBoundColumn DataField="ToDate" Visible="False"
                                                        HeaderText="ToDate">
                                <HeaderStyle Width="0%" />
                           </dnn:DnnGridBoundColumn>
                           <dnn:DnnGridBoundColumn DataField="FromDateString" Visible="True"
                                                        HeaderText="FromDateString">
                                <HeaderStyle Width="30%" HorizontalAlign="Center" VerticalAlign="Middle" />
                           </dnn:DnnGridBoundColumn>
                           <dnn:DnnGridBoundColumn DataField="ToDateString" Visible="True"
                                                        HeaderText="ToDateString">
                                <HeaderStyle Width="30%" HorizontalAlign="Center" VerticalAlign="Middle" />
                           </dnn:DnnGridBoundColumn>
                           <dnn:DnnGridBoundColumn DataField="ModifyUserID" Visible="False"
                                                        HeaderText="ModifyUserID">
                                <HeaderStyle Width="0%" />
                           </dnn:DnnGridBoundColumn>
                           <dnn:DnnGridBoundColumn DataField="ModifyDateTime" Visible="False"
                                                        HeaderText="ModifyDateTime">
                                <HeaderStyle Width="0%" />
                           </dnn:DnnGridBoundColumn>
                           <dnn:DnnGridTemplateColumn Visible="True" HeaderText="#">
                                <HeaderStyle Width="10%" HorizontalAlign="Center" VerticalAlign="Middle" />
                                <ItemTemplate>
                                        <asp:LinkButton CommandArgument="<%#Eval(AbsentCalendarTable.ID).ToString() %>"
                                                        CommandName="<%#Eval(AbsentCalendarTable.UserID).ToString() %>"
                                                        CssClass="c-edit-link c-theme-color"
                                                        OnClick="DeleteAbsentItem"
                                                        runat="server">
                                            <img alt = "Delete" src="<%#DeleteIcon%>">
                                        </asp:LinkButton>
                               </ItemTemplate>
                           </dnn:DnnGridTemplateColumn>
                       </Columns>
                   </MasterTableView> 
                </control:Grid>
            </div>
            <div class="col-md-4"></div>
        </div>
    </div>
   <div ID="DivAddAbsentCalendar" runat="server">
       <div class="row">
           <div class="col-md-3"></div>
           <div class="col-md-6 text-center">
               <hr class="c-margin-t-40 separator" />
               <h4 class="c-font-uppercase">Thêm ngày nghỉ</h4>
           </div>
           <div class="col-md-3"></div>
       </div>
       <div class="form-group">
            <div class="col-md-2"></div>
            <div class="col-md-2 control-label">
                <dnn:Label ID="lblAbsentType" runat="server" />
            </div>
            <div class="col-md-4">
                <control:Combobox CssClass="form-control c-theme" 
                                          ID="ddlAbsentType"
                                          AutoPostBack="True"
                                          runat="server" />
            </div>
            <div class="col-md-4"></div>
        </div>
       <div class="form-group">
            <div class="row">
                <div class="col-md-3"></div>
                <div class="col-md-1 control-label">
                    <dnn:Label ID="lblFromDate" runat="server" />
                </div>
                <div class="col-md-2">
                    <dnn:DnnDatePicker Culture-DateTimeFormat-ShortDatePattern="dd/MM/yyyy"
                                           ID="dpFromDate"
                                           OnSelectedDateChanged="FromDateChange"
                                           runat="server" />
                </div>
                <div class="col-md-1 control-label">
                    <dnn:Label ID="lblToDate" runat="server" />
                </div>
                <div class="col-md-2">
                    <dnn:DnnDatePicker Culture-DateTimeFormat-ShortDatePattern="dd/MM/yyyy"
                                            OnSelectedDateChanged="ToDateChange"
                                           ID="dpToDate"
                                           runat="server" />
                </div>
                <div class="col-md-3"></div>
            </div>
        </div>
       <div class="form-group">
                <div class="col-sm-4"></div>
                <div class="col-sm-4" style="text-align: center;">
                    <asp:Button CssClass="btn btn-primary"
                                ID="btnAddAbsent"
                                OnClick="AddAbSentCalendar"
                                runat="server"
                                Text="Thêm ngày phép" />
                </div>
                <div class="col-sm-2"></div>
        </div>
   </div>
   <div id="DivUserProcessContent"
         runat="server">
        <div class="dnnPanels"
             id="DivUserList"
             runat="server">
            <br />
            <div class="form-group">
                <div class="col-lg-2"></div>
                <div class="col-sm-8 text-center">
                    <hr class="c-margin-t-40 separator" />
                    <h4 class="c-font-uppercase">Thông tin phân quyền thực hiện</h4>
                    <control:Grid AllowPaging="true"
                                 AutoGenerateColumns="false"
                                 CssClass="dnnGrid"
                                 EnableViewState="true"
                                 ID="gridData"
                                 OnPageIndexChanged="OnPageIndexChanging"
                                 OnPageSizeChanged="OnPageSizeChanging"
                                 PageSize="10"
                                 runat="server"
                                 Visible="false">
                        <MasterTableView>
                            <Columns>
                                <dnn:DnnGridBoundColumn DataField="UserID" Visible="False"
                                                        HeaderText="UserID">
                                <HeaderStyle Width="0%" />
                                </dnn:DnnGridBoundColumn>
                                <dnn:DnnGridTemplateColumn HeaderText="FullName" Visible="False">
                                    <HeaderStyle Width="0%"/>
                                    <ItemTemplate>
                                        <%#Eval(UserPhaseMappingTable.FullName).ToString() %> 
                                        < <%#Eval(UserPhaseMappingTable.Username).ToString() %> >
                                    </ItemTemplate>
                                </dnn:DnnGridTemplateColumn>
                                <dnn:DnnGridTemplateColumn HeaderText="Username" Visible="False">
                                    <HeaderStyle Width="0%" />
                                    <ItemTemplate>
                                        <%#Eval(UserPhaseMappingTable.Username).ToString() %>
                                    </ItemTemplate>
                                </dnn:DnnGridTemplateColumn>
                                <dnn:DnnGridBoundColumn DataField="ApplicationTypeCode" Visible="False"
                                                        HeaderText="ApplicationTypeCode">
                                    <HeaderStyle Width="0%" />
                                </dnn:DnnGridBoundColumn>
                                <dnn:DnnGridBoundColumn DataField="ApplicationTypeName"
                                                        HeaderText="ApplicationTypeName">
                                    <HeaderStyle Width="30%" HorizontalAlign="Center" VerticalAlign="Middle" />
                                </dnn:DnnGridBoundColumn>
                                <dnn:DnnGridBoundColumn DataField="PhaseCode" Visible="False"
                                                        HeaderText="PhaseCode">
                                    <HeaderStyle Width="0%" />
                                </dnn:DnnGridBoundColumn>
                                <dnn:DnnGridBoundColumn DataField="PhaseName" Visible="True"
                                                        HeaderText="PhaseName">
                                    <HeaderStyle Width="30%" HorizontalAlign="Center" VerticalAlign="Middle" />
                                </dnn:DnnGridBoundColumn>
                                <dnn:DnnGridBoundColumn DataField="UserPhaseMappingID" Visible="False"
                                                        HeaderText="UserPhaseMappingID">
                                    <HeaderStyle Width="0%" />
                                </dnn:DnnGridBoundColumn>
                                <dnn:DnnGridBoundColumn DataField="PolicyCode" Visible="True"
                                                        HeaderText="PolicyCode">
                                    <HeaderStyle Width="40%" HorizontalAlign="Center" VerticalAlign="Middle" />
                                </dnn:DnnGridBoundColumn>
                            </Columns>
                        </MasterTableView>
                    </control:Grid>
                </div>
                <div class="col-lg-2"></div>
            </div>
        </div>
        
        <div class="form-group">
            <div class="col-sm-2"></div>
            <div class="col-sm-8" style="text-align: center;">
                <hr class="c-margin-t-40 separator" />
                <asp:Button CssClass="btn btn-primary"
                            ID="btnConfigUserProcess"
                            OnClick="UserConfiguration"
                            runat="server"
                            Text="Cập nhập phân quyền" />
            </div>
            <div class="col-sm-2"></div>
        </div>
    </div>
</div>
</ContentTemplate>
</asp:UpdatePanel>
