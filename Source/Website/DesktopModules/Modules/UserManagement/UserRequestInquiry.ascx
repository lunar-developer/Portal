<%@ Control Language="C#" AutoEventWireup="false" CodeFile="UserRequestInquiry.ascx.cs" Inherits="DesktopModules.Modules.UserManagement.UserRequestInquiry" %>
<%@ Import Namespace="Website.Library.Global" %>
<%@ Import Namespace="Modules.UserManagement.Database" %>
<%@ Import Namespace="Modules.UserManagement.Enum" %>
<%@ Register TagPrefix="control" Namespace="Modules.Controls" Assembly="Modules.Controls" %>
<%@ Register Src="~/controls/Label.ascx" TagName="Label" TagPrefix="control" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<asp:UpdatePanel ID="updatePanel"
                 runat="server">
    <ContentTemplate>
        <div class="form-horizontal">
            <div class="form-group">
                <div class="col-sm-2 control-label">
                    <control:Label HelpText="Email"
                                   runat="server"
                                   Text="Tên đăng nhập" />
                </div>
                <div class="col-sm-4">
                    <asp:TextBox CssClass="form-control c-theme"
                                 ID="txtUserName"
                                 placeholder="Tên đăng nhập"
                                 runat="server" />
                </div>
                <div class="col-sm-2 control-label">
                    <control:Label HelpText="Chi Nhánh"
                                   runat="server"
                                   Text="Chi Nhánh" />
                </div>
                <div class="col-sm-4">
                    <control:Combobox autocomplete="off"
                                      CssClass="form-control c-theme"
                                      ID="ddlBranch"
                                      runat="server" />
                </div>
            </div>
            <div class="form-group">
                <div class="col-sm-2 control-label">
                    <control:Label HelpText="Loại yêu cầu"
                                   runat="server"
                                   Text="Loại yêu cầu" />
                </div>
                <div class="col-sm-4">
                    <asp:DropDownList CssClass="form-control c-theme"
                                      ID="ddlRequestTypeID"
                                      runat="server" />
                </div>
                <div class="col-sm-2 control-label">
                    <control:Label HelpText="Trạng thái xử lý"
                                   runat="server"
                                   Text="Trạng thái" />
                </div>
                <div class="col-sm-4">
                    <asp:DropDownList CssClass="form-control c-theme"
                                      ID="ddlRequestStatus"
                                      runat="server" />
                </div>
            </div>
            <div class="form-group">
                <div class="col-sm-2"></div>
                <div class="col-sm-10">
                    <asp:Button CssClass="btn btn-primary"
                                ID="btnSearch"
                                OnClick="SearchRequest"
                                runat="server"
                                Text="Tìm Kiếm" />
                    <a class="btn btn-default"
                       href="<%= GetEditUrl() %>"
                       target="_blank">
                        Thêm mới
                    </a>
                </div>
            </div>

            <br />
            <div class="form-group">
                <div class="col-sm-12 table-responsive">
                    <control:Grid AllowPaging="true"
                                  AutoGenerateColumns="false"
                                  CssClass="dnnGrid"
                                  EnableViewState="true"
                                  ID="gridData"
                                  OnPageIndexChanged="OnPageIndexChanging"
                                  OnPageSizeChanged="OnPageSizeChanging"
                                  runat="server"
                                  Visible="false">
                        <MasterTableView TableLayout="Fixed">
                            <Columns>
                                <telerik:GridTemplateColumn HeaderText="Họ và Tên">
                                    <HeaderStyle Width="250px" />
                                    <ItemTemplate>
                                        <a class="c-edit-link c-theme-color"
                                           href="<%#GetEditUrl(Eval(UserRequestTable.UserRequestID).ToString()) %>"
                                           target="_blank">
                                            <%#FunctionBase.FormatUserID(Eval(UserRequestTable.UserID).ToString()) %>
                                        </a>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Loại yêu cầu">
                                    <HeaderStyle Width="200px" />
                                    <ItemTemplate>
                                        <%#RequestTypeEnum.GetDescription(Eval(UserRequestTable.RequestTypeID).ToString()) %>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Trạng thái">
                                    <HeaderStyle Width="150px" />
                                    <ItemTemplate>
                                        <%#RequestStatusEnum.GetDescription(Eval(UserRequestTable.RequestStatus).ToString()) %>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridBoundColumn DataField="RequestReason"
                                                         HeaderText="Lý do">
                                    <HeaderStyle Width="300px" />
                                </telerik:GridBoundColumn>
                                <telerik:GridTemplateColumn HeaderText="Ngày tạo">
                                    <HeaderStyle Width="200px" />
                                    <ItemTemplate>
                                        <%#FunctionBase.FormatDate(Eval(UserRequestTable.CreateDateTime).ToString()) %>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                            </Columns>
                        </MasterTableView>
                    </control:Grid>
                </div>
            </div>
        </div>

        <asp:HiddenField ID="hidUserName"
                         runat="server"
                         Visible="False" />
        <asp:HiddenField ID="hidBranchID"
                         runat="server"
                         Visible="False" />
        <asp:HiddenField ID="hidRequestTypeID"
                         runat="server"
                         Visible="False" />
        <asp:HiddenField ID="hidRequestStatus"
                         runat="server"
                         Visible="False" />
    </ContentTemplate>
</asp:UpdatePanel>