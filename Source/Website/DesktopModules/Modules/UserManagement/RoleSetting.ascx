<%@ Control Language="C#" AutoEventWireup="false" CodeFile="RoleSetting.ascx.cs" Inherits="DesktopModules.Modules.UserManagement.RoleSetting" %>
<%@ Import Namespace="Website.Library.Global" %>
<%@ Import Namespace="Modules.UserManagement.Database" %>
<%@ Import Namespace="Website.Library.Database" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="control" Namespace="Modules.Controls" Assembly="Modules.Controls" %>

<asp:UpdatePanel ID="updatePanel"
    runat="server">
    <ContentTemplate>
        <div class="form-horizontal">
            <div class="form-group">
                <asp:PlaceHolder ID="phMessage"
                    runat="server" />
            </div>
            <div class="form-group">
                <div class="col-sm-2"></div>
                <div class="col-sm-2 control-label">
                    <dnn:Label ID="lblRoleGroup"
                        Text="Nhóm quyền"
                        runat="server" />
                </div>
                <div class="col-sm-4">
                    <control:AutoComplete
                        ID="ddlRoleGroup"
                        runat="server" />
                </div>
                <div class="col-sm-4">
                    <asp:Button runat="server"
                        CssClass="btn btn-primary c-margin-t-0"
                        ID="btnSearch"
                        OnClick="SearchRoleSetting"
                        Text="Tìm kiếm" />
                    <asp:Button runat="server"
                        CssClass="btn btn-success c-margin-t-0"
                        ID="btnAdd"
                        OnClick="AddRoleSetting"
                        Text="Thêm Mới" />
                    <asp:Button runat="server"
                        CssClass="btn btn-default c-margin-t-0 invisible"
                        ID="btnRefresh"
                        OnClick="Refresh"
                        Text="Refresh" />
                </div>
            </div>
            <div class="form-group c-margin-t-30">
                <div class="col-sm-12">
                    <control:Grid AutoGenerateColumns="False"
                        ID="gridData"
                        OnItemCommand="ProcessOnGridItemCommand"
                        Width="100%"
                        runat="server">
                        <MasterTableView TableLayout="Auto" DataKeyNames="RoleID">
                            <Columns>
                                <telerik:GridBoundColumn DataField="RoleID"
                                    HeaderText="ID">
                                    <HeaderStyle Width="5%" />
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="RoleName"
                                    HeaderText="Tên Quyền">
                                    <HeaderStyle Width="15%" />
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="Description"
                                    HeaderText="Mô tả">
                                    <HeaderStyle Width="25%" />
                                </telerik:GridBoundColumn>
                                <telerik:GridTemplateColumn DataField="RoleScope"
                                    HeaderText="Phạm vi áp dụng">
                                    <HeaderStyle Width="15%" />
                                    <ItemTemplate>
                                        <%#FormatScope(Eval(RoleExtensionTable.RoleScope)?.ToString()) %>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn DataField="IsDisable"
                                    HeaderText="Đang sử dụng">
                                    <HeaderStyle Width="10%" />
                                    <ItemTemplate>
                                        <%#FunctionBase.FormatState(Eval(BaseTable.IsDisable)?.ToString(), false) %>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn DataField="ListExcludeRoleID"
                                    HeaderText="Các Quyền không tương thích">
                                    <HeaderStyle Width="20%" />
                                    <ItemTemplate>
                                        <div class="row">
                                            <div class="col-sm-12">
                                                <%#FormatListExcludeRole(Eval(RoleExtensionTable.ListExcludeRoleID)?.ToString()) %>
                                            </div>
                                        </div>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Hành Động" UniqueName="Action">
                                    <HeaderStyle Width="10%" />
                                    <ItemTemplate>
                                        <div class="row text-center">
                                            <div class="col-sm-2">
                                                <asp:LinkButton runat="server">
                                                    <i class="fa fa-pencil-square-o icon-primary"></i>
                                                </asp:LinkButton>
                                            </div>
                                            <div class="col-sm-2">
                                                <asp:LinkButton runat="server"
                                                    ToolTip="Xóa"
                                                    CssClass="btnDelete"
                                                    CommandName="DeleteCommand">
                                                    <i class="fa fa-trash-o icon-danger"></i>
                                                </asp:LinkButton>
                                            </div>
                                        </div>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                            </Columns>
                        </MasterTableView>
                    </control:Grid>
                </div>
            </div>
        </div>

        <asp:HiddenField runat="server" ID="hidRoleGroupID" Visible="False" />

    </ContentTemplate>
</asp:UpdatePanel>
<style type="text/css">
    .btnDelete {
        display: block;
    }
</style>
<script type="text/javascript">
    addPageLoaded(function ()
    {
        $(".btnDelete").each(function ()
        {
            var element = this;
            registerConfirm({
                jquery: element,
                isUseRuntimeMessage: true,
                getRunTimeMessage: function ()
                {
                    var roleName = getRoleName(element);
                    return "Bạn có chắc muốn xóa cấu hình phân Quyền <b>" + roleName + "</b> ?";
                }
            });
        });
    }, true);

    function getRoleName(element)
    {
        var columns = $(element).closest("tr").find("td");
        return columns[1].innerHTML;
    }

    function refresh()
    {
        getJQueryControl("btnRefresh").click();
    }
</script>
