<%@ Control Language="C#" AutoEventWireup="true" CodeFile="RoleTemplateInquiry.ascx.cs" Inherits="DesktopModules.Modules.UserManagement.RoleTemplateInquiry" %>
<%@ Import Namespace="Website.Library.Database" %>
<%@ Import Namespace="Modules.UserManagement.Database" %>
<%@ Import Namespace="Website.Library.Global" %>
<%@ Register TagPrefix="control" Namespace="Modules.Controls" Assembly="Modules.Controls" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<asp:UpdatePanel ID="updatePanel"
                 runat="server">
    <ContentTemplate>
        <div class="form-horizontal">
            <div class="form-group">
                <div class="col-md-2 col-sm-0"></div>
                <div class="col-md-2 control-label">
                    <label>Chi Nhánh</label>
                </div>
                <div class="col-md-4">
                    <control:Combobox CssClass="form-control c-theme"
                                      ID="ddlBranch"
                                      runat="server" />
                </div>
                <div class="col-md-4 col-sm-0"></div>
            </div>
            <div class="form-group">
                <div class="col-md-4 col-sm-0"></div>
                <div class="col-md-8">
                    <asp:Button CssClass="btn btn-primary"
                                ID="btnSearch"
                                OnClick="Search"
                                runat="server"
                                Text="Xem" />
                    <a class="btn btn-primary"
                       href="<%= GetUrl() %>"
                       target="_blank">
                        Thêm Mới
                    </a>
                </div>
            </div>
            <div class="c-margin-t-30 form-group">
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
                            <telerik:GridTemplateColumn HeaderText="Chức danh">
                                <HeaderStyle Width="30%" />
                                <ItemTemplate>
                                    <%#GetEditLink(
                                           Eval(RoleTemplateTable.TemplateID).ToString(),
                                           Eval(RoleTemplateTable.TemplateName).ToString()) %>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridBoundColumn DataField="Remark"
                                                     HeaderText="Ghi chú">
                                <HeaderStyle Width="30%" />
                            </telerik:GridBoundColumn>
                            <telerik:GridTemplateColumn HeaderText="Ngày cập nhật">
                                <HeaderStyle Width="20%" />
                                <ItemTemplate>
                                    <%#FunctionBase.FormatDate(Eval(BaseTable.ModifyDateTime).ToString()) %>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Disable">
                                <HeaderStyle Width="20%" />
                                <ItemTemplate>
                                    <%#FormatState(Eval(BaseTable.IsDisable).ToString()) %>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                        </Columns>
                    </MasterTableView>
                </control:Grid>
            </div>

            <asp:HiddenField ID="hidBranchID"
                             runat="server"
                             Visible="False" />
        </div>
    </ContentTemplate>
</asp:UpdatePanel>