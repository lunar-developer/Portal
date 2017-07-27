<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ApplicationInquiry.ascx.cs" Inherits="DesktopModules.Modules.Application.ApplicationInquiry" %>
<%@ Import Namespace="Modules.Application.Database" %>
<%@ Register TagPrefix="dnn" Assembly="DotNetNuke.Web.Deprecated" Namespace="DotNetNuke.Web.UI.WebControls" %>
<%@ Register TagPrefix="control" Namespace="Modules.Controls" Assembly="Modules.Controls" %>

<asp:UpdatePanel runat="server"
                 UpdateMode="Conditional">
    <ContentTemplate>
        <div class="form-horizontal">
            <div class="col-sm-6">
                <div class="form-group">
                    <div class="col-sm-4">
                        <control:AutoComplete ID="ddlInputName01" runat="server" />
                    </div>
                    <div class="col-sm-7">
                        <asp:TextBox CssClass="c-theme form-control"
                                     ID="txtInputValue01"
                                     placeholder="Mỗi giá trị phân cách bằng ';'"
                                     runat="server" />
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-sm-4">
                        <control:AutoComplete ID="ddlInputName02" runat="server" />
                    </div>
                    <div class="col-sm-7">
                        <asp:TextBox CssClass="c-theme form-control"
                                     ID="txtInputValue02"
                                     placeholder="Mỗi giá trị phân cách bằng ';'"
                                     runat="server" />
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-sm-4">
                        <control:AutoComplete ID="ddlDateField" runat="server" />
                    </div>
                    <div class="col-sm-3">
                        <dnn:DnnDatePicker Culture-DateTimeFormat-ShortDatePattern="dd/MM/yyyy"
                                           EnableTyping="False"
                                           ID="calFromDate"
                                           runat="server"
                                           Width="120px" />
                    </div>
                    <div class="col-sm-3">
                        <dnn:DnnDatePicker Culture-DateTimeFormat-ShortDatePattern="dd/MM/yyyy"
                                           EnableTyping="False"
                                           ID="calToDate"
                                           runat="server"
                                           Width="120px" />
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-sm-4"></div>
                    <div class="col-sm-8">
                        <asp:Button CssClass="btn btn-primary"
                                    ID="btnSearch"
                                    OnClick="Search"
                                    runat="server"
                                    Text="Tìm kiếm" />
                    </div>
                </div>
            </div>

            <div class="col-sm-6">
                <div class="form-group">
                    <div class="col-sm-4">
                        <control:AutoComplete
                            AutoPostBack="True"
                            ID="ddlSelectName01"
                            OnSelectedIndexChanged="LoadData"
                            runat="server" />
                    </div>
                    <div class="col-sm-8">
                        <control:AutoComplete ID="ddlSelectValue01" runat="server" />
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-sm-4">
                        <control:AutoComplete 
                            AutoPostBack="True"
                            ID="ddlSelectName02"
                            OnSelectedIndexChanged="LoadData"
                            runat="server" />
                    </div>
                    <div class="col-sm-8">
                        <control:AutoComplete ID="ddlSelectValue02" runat="server" />
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-sm-4">
                        <control:AutoComplete
                            AutoPostBack="True"
                            ID="ddlSelectName03"
                            OnSelectedIndexChanged="LoadData"
                            runat="server" />
                    </div>
                    <div class="col-sm-8">
                        <control:AutoComplete
                            ID="ddlSelectValue03"
                            runat="server" />
                    </div>
                </div>
            </div>

            <div class="clearfix"></div>
            <div class="form-group">
                <div style="width: 100%; overflow: auto;">
                    <control:Grid
                        AutoGenerateColumns="True"
                        ID="gridData"
                        runat="server"
                        Visible="false">
                        <MasterTableView>
                            <Columns>
                                <dnn:DnnGridTemplateColumn HeaderText="#">
                                    <HeaderStyle Width="35px"></HeaderStyle>
                                    <ItemTemplate>
                                        <a href="<%#GetEditUrl(Eval(ApplicationTable.ApplicationID).ToString()) %>" target="_blank">
                                            <i class="fa fa-edit"></i>
                                        </a>
                                    </ItemTemplate>
                                </dnn:DnnGridTemplateColumn>
                            </Columns>
                        </MasterTableView>
                    </control:Grid>
                </div>
            </div>


            <asp:HiddenField ID="hidInputName01"
                             runat="server"
                             Visible="False" />
            <asp:HiddenField ID="hidInputName02"
                             runat="server"
                             Visible="False" />
            <asp:HiddenField ID="hidDateField"
                             runat="server"
                             Visible="False" />
            <asp:HiddenField ID="hidSelectName01"
                             runat="server"
                             Visible="False" />
            <asp:HiddenField ID="hidSelectName02"
                             runat="server"
                             Visible="False" />
            <asp:HiddenField ID="hidSelectName03"
                             runat="server"
                             Visible="False" />
            <asp:HiddenField ID="hidInputValue01"
                             runat="server"
                             Visible="False" />
            <asp:HiddenField ID="hidInputValue02"
                             runat="server"
                             Visible="False" />
            <asp:HiddenField ID="hidSelectValue01"
                             runat="server"
                             Visible="False" />
            <asp:HiddenField ID="hidSelectValue02"
                             runat="server"
                             Visible="False" />
            <asp:HiddenField ID="hidSelectValue03"
                             runat="server"
                             Visible="False" />
            <asp:HiddenField ID="hidFromDate"
                             runat="server"
                             Visible="False" />
            <asp:HiddenField ID="hidToDate"
                             runat="server"
                             Visible="False" />
        </div>
    </ContentTemplate>
</asp:UpdatePanel>