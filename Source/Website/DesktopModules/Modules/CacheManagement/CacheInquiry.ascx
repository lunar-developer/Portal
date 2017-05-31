<%@ Control Language="C#" AutoEventWireup="false" CodeFile="CacheInquiry.ascx.cs" Inherits="DesktopModules.Modules.CacheManagement.CacheInquiry" %>
<%@ Register TagPrefix="control" Namespace="Modules.Controls" Assembly="Modules.Controls" %>

<asp:UpdatePanel ID="updatePanel"
                 runat="server">
    <ContentTemplate>
        <div class="form-horizontal">
            <div class="form-group">
                <div class="col-sm-5">
                    <control:Combobox autocomplete="off"
                                      AutoPostBack="True"
                                      CssClass="form-control c-theme"
                                      ID="ddlCacheType"
                                      onselectedindexchanged="ProcessOnSelectionChange"
                                      runat="server">
                    </control:Combobox>
                </div>
                <div class="col-sm-3">
                    <h4>
                        Total Items: <asp:Label ID="lblTotal"
                                                runat="server"
                                                Text="0" />
                    </h4>
                </div>
                <div class="col-sm-4">
                    <asp:Button CssClass="btn btn-primary c-margin-t-0"
                                ID="btnReload"
                                OnClick="Reload"
                                runat="server"
                                Text="Reload" />
                </div>
            </div>

            <div class="form-group"
                 id="DivControl"
                 runat="server">
                <div class="col-sm-5">
                    <control:Combobox autocomplete="off"
                                      CssClass="form-control c-theme"
                                      ID="ddlField"
                                      runat="server" />
                </div>
                <div class="col-sm-3">
                    <asp:TextBox CssClass="form-control"
                                 ID="tbKeyword"
                                 placeholder="Keyword"
                                 runat="server" />
                </div>
                <div class="col-sm-4">
                    <asp:Button CssClass="btn btn-primary c-margin-t-0"
                                ID="btnSearch"
                                OnClick="Filter"
                                runat="server"
                                Text="Search" />
                    <asp:Button CssClass="btn btn-primary c-margin-t-0"
                                ID="btnClear"
                                OnClick="ClearFilter"
                                runat="server"
                                Text="Clear" />
                </div>
            </div>

            <div class="form-group">
                <div class="col-sm-12">
                    <control:Grid AllowPaging="true"
                                  AutoGenerateColumns="true"
                                  CssClass="dnnGrid"
                                  EnableViewState="true"
                                  ID="gridView"
                                  OnPageIndexChanged="OnPageIndexChanging"
                                  OnPageSizeChanged="OnPageSizeChanging"
                                  PageSize="10"
                                  runat="server"
                                  Visible="True" />
                </div>
            </div>
        </div>
        
        <asp:HiddenField runat="server" ID="hidFieldName"/>
        <asp:HiddenField runat="server" ID="hidFieldValue"/>
    </ContentTemplate>
</asp:UpdatePanel>