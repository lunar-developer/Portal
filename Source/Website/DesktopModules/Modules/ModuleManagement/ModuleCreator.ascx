<%@ Control Language="C#" AutoEventWireup="false" CodeFile="ModuleCreator.ascx.cs" Inherits="DesktopModules.Modules.ModuleManagement.ModuleCreator" %>
<%@ Register TagPrefix="control" Namespace="Modules.Controls" Assembly="Modules.Controls" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.Web.UI.WebControls" Assembly="DotNetNuke.Web.Deprecated" %>

<asp:UpdatePanel ID="updatePanel"
                 runat="server">
    <ContentTemplate>
        <div class="form-horizontal">
            <div class="form-group">
                <div class="col-sm-3 control-label">
                    <label>Tab</label>
                </div>
                <div class="col-sm-5">
                    <control:Combobox autocomplete="off"
                                      AutoPostBack="True"
                                      CssClass="c-theme form-control"
                                      ID="ddlTab"
                                      OnSelectedIndexChanged="LoadTabModule"
                                      runat="server" />
                </div>
                <div class="col-sm-2"></div>
            </div>
            <div class="form-group">
                <div class="col-sm-3 control-label">
                    <label>Module</label>
                </div>
                <div class="col-sm-5">
                    <control:Combobox AutoPostBack="True"
                                      CssClass="c-theme form-control"
                                      ID="ddlModule"
                                      OnSelectedIndexChanged="LoadModuleDefinition"
                                      runat="server" />
                </div>
                <div class="col-sm-2"></div>
            </div>
            <div class="form-group">
                <div class="col-sm-3 control-label">
                    <label>Definition</label>
                </div>
                <div class="col-sm-5">
                    <control:Combobox CssClass="c-theme form-control"
                                      ID="ddlDefinition"
                                      runat="server" />
                </div>
                <div class="col-sm-2"></div>
            </div>
            <div class="form-group">
                <div class="col-sm-3 control-label"></div>
                <div class="col-sm-9">
                    <asp:Button CssClass="btn btn-primary"
                                ID="btnInsert"
                                OnClick="Insert"
                                runat="server"
                                Text="Add" />
                </div>
            </div>
            <div class="form-group">
                <asp:PlaceHolder ID="phMessage"
                                 runat="server" />
            </div>
            <div class="form-group">
                <div class="col-sm-12">
                    <control:Grid AllowPaging="true"
                                  AutoGenerateColumns="false"
                                  CssClass="dnnGrid"
                                  EnableViewState="true"
                                  ID="gridData"
                                  PageSize="10"
                                  runat="server"
                                  Visible="false">
                        <MasterTableView>
                            <Columns>
                                <dnn:DnnGridBoundColumn DataField="ModuleOrder"
                                                        HeaderText="ModuleOrder">
                                    <HeaderStyle Width="10%" />
                                </dnn:DnnGridBoundColumn>
                                <dnn:DnnGridBoundColumn DataField="ModuleName"
                                                        HeaderText="ModuleName">
                                    <HeaderStyle Width="30%" />
                                </dnn:DnnGridBoundColumn>
                                <dnn:DnnGridBoundColumn DataField="Content"
                                                        HeaderText="Content">
                                    <HeaderStyle Width="30%" />
                                </dnn:DnnGridBoundColumn>
                                <dnn:DnnGridBoundColumn DataField="ControlSrc"
                                                        HeaderText="ControlSrc">
                                    <HeaderStyle Width="30%" />
                                </dnn:DnnGridBoundColumn>
                            </Columns>
                        </MasterTableView>
                    </control:Grid>
                </div>
            </div>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>