<%@ Control Language="C#" AutoEventWireup="false" CodeFile="EmployeeInquiry.ascx.cs" Inherits="DesktopModules.Modules.EmployeeManagement.EmployeeInquiry" %>
<%@ Import Namespace="Website.Library.Global" %>
<%@ Import Namespace="Modules.EmployeeManagement.Database" %>
<%@ Register Src="~/controls/LabelControl.ascx" TagName="Label" TagPrefix="dnn" %>
<%@ Register TagPrefix="control" Namespace="Modules.Controls" Assembly="Modules.Controls" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.Web.UI.WebControls" Assembly="DotNetNuke.Web.Deprecated" %>

<style type="text/css">
    .imageHeight
    {
        height: 250px;
        overflow: hidden;
    }
    .imageWidth
    {
        width: 100%;
        max-width: 150px;
        padding: 0;
        margin: 20px 0;
    }
    .divRecord > div:nth-child(odd)
    {
        background-color: #f9f9f9;
    }

    .divRecord > div
    {
        padding: 8px;
        line-height: 1.42857143;
        vertical-align: top;
        border-top: 1px solid #ddd;
    }
</style>

<asp:UpdatePanel ID="updatePanel"
                 runat="server">
    <ContentTemplate>
        <div class="form-horizontal">
            <div class="form-group">
                <div class="col-sm-2 control-label">
                    <dnn:Label ID="lblEmployeeID"
                               runat="server" />
                </div>
                <div class="col-sm-4">
                    <asp:TextBox CssClass="form-control c-theme"
                                 ID="txtEmployeeID"
                                 runat="server" />
                </div>
                <div class="col-sm-2 control-label">
                    <dnn:Label ID="lblFullName"
                               runat="server" />
                </div>
                <div class="col-sm-4">
                    <asp:TextBox CssClass="form-control c-theme"
                                 ID="txtFullName"
                                 runat="server" />
                </div>
            </div>
            <div class="form-group">
                <div class="col-sm-2 control-label">
                    <dnn:Label ID="lblEmail"
                               runat="server" />
                </div>
                <div class="col-sm-4">
                    <asp:TextBox CssClass="form-control c-theme"
                                 ID="txtEmail"
                                 runat="server" />
                </div>
                <div class="col-sm-2 control-label">
                    <dnn:Label ID="lblBranch"
                               runat="server" />
                </div>
                <div class="col-sm-4">
                    <asp:TextBox CssClass="form-control c-theme"
                                 ID="txtBranch"
                                 runat="server" />
                </div>
            </div>
            <div class="form-group">
                <div class="col-sm-2 control-label">
                    <dnn:Label ID="lblArea"
                               runat="server" />
                </div>
                <div class="col-sm-4">
                    <asp:TextBox CssClass="form-control c-theme"
                                 ID="txtArea"
                                 runat="server" />
                </div>
                <div class="col-sm-2 control-label"></div>
                <div class="col-sm-4"></div>
            </div>
            <div class="form-group">
                <div class="col-sm-2"></div>
                <div class="col-sm-10">
                    <asp:Button CssClass="btn btn-primary"
                                ID="btnSearch"
                                OnClick="SearchEmployee"
                                runat="server"
                                Text="Tìm Kiếm" />
                </div>
            </div>
            <br />
            <div class="form-group">
                <div class="col-sm-12">
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
                                <dnn:DnnGridBoundColumn DataField="EmployeeID"
                                                        HeaderText="EmployeeID">
                                    <HeaderStyle Width="5%" />
                                </dnn:DnnGridBoundColumn>
                                <dnn:DnnGridBoundColumn DataField="FullName"
                                                        HeaderText="FullName">
                                    <HeaderStyle Width="15%" />
                                </dnn:DnnGridBoundColumn>
                                <dnn:DnnGridTemplateColumn DataField="DateOfBirth"
                                                           HeaderText="DateOfBirth">
                                    <HeaderStyle Width="5%" />
                                    <ItemTemplate>
                                        <%#FunctionBase.FormatDate(Eval(EmployeeTable.DateOfBirth).ToString()) %>
                                    </ItemTemplate>
                                </dnn:DnnGridTemplateColumn>
                                <dnn:DnnGridBoundColumn DataField="Gender"
                                                        HeaderText="Gender">
                                    <HeaderStyle Width="5%" />
                                </dnn:DnnGridBoundColumn>
                                <dnn:DnnGridBoundColumn DataField="Role"
                                                        HeaderText="Role">
                                    <HeaderStyle Width="15%" />
                                </dnn:DnnGridBoundColumn>
                                <dnn:DnnGridBoundColumn DataField="Office"
                                                        HeaderText="Office">
                                    <HeaderStyle Width="20%" />
                                </dnn:DnnGridBoundColumn>
                                <dnn:DnnGridBoundColumn DataField="Area"
                                                        HeaderText="Area">
                                    <HeaderStyle Width="15%" />
                                </dnn:DnnGridBoundColumn>
                                <dnn:DnnGridBoundColumn DataField="PhoneNumber"
                                                        HeaderText="PhoneNumber">
                                    <HeaderStyle Width="5%" />
                                </dnn:DnnGridBoundColumn>
                                <dnn:DnnGridBoundColumn DataField="PhoneExtendNumber"
                                                        HeaderText="PhoneExtendNumber">
                                    <HeaderStyle Width="5%" />
                                </dnn:DnnGridBoundColumn>
                                <dnn:DnnGridBoundColumn DataField="Email"
                                                        HeaderText="Email">
                                    <HeaderStyle Width="10%" />
                                </dnn:DnnGridBoundColumn>
                            </Columns>
                        </MasterTableView>
                    </control:Grid>
                </div>
            </div>
        </div>
        
        <div id="divResultSearch" runat="server"></div>
        
        <asp:HiddenField ID="hidEmployeeID"
                         runat="server"
                         Visible="False" />
        <asp:HiddenField ID="hidFullName"
                         runat="server"
                         Visible="False" />
        <asp:HiddenField ID="hidEmail"
                         runat="server"
                         Visible="False" />
        <asp:HiddenField ID="hidBranch"
                         runat="server"
                         Visible="False" />
        <asp:HiddenField ID="hidArea"
                         runat="server"
                         Visible="False" />
    </ContentTemplate>
</asp:UpdatePanel>