<%@ Control Language="C#" AutoEventWireup="false" CodeFile="UserRequestEditor.ascx.cs" Inherits="DesktopModules.Modules.UserManagement.UserRequestEditor" %>
<%@ Register TagPrefix="control" Namespace="Modules.Controls" Assembly="Modules.Controls" %>
<%@ Register Src="~/controls/Label.ascx" TagName="Label" TagPrefix="control" %>

<asp:UpdatePanel ID="updatePanel"
                 runat="server">
<ContentTemplate>
<div class="form-group">
    <asp:PlaceHolder ID="phMessage"
                     runat="server" />
</div>
<div class="dnnPanels form-horizontal"
     id="DivForm"
     runat="server">

<div>
    <h2 class="dnnFormSectionHead">
        <a href="#">THÔNG TIN YÊU CẦU</a>
    </h2>
    <fieldset>
        <div class="form-group">
            <div class="col-sm-2 control-label">
                <control:Label IsRequire="true"
                               runat="server"
                               Text="Chi Nhánh" />
            </div>
            <div class="col-sm-4">
                <control:Combobox autocomplete="off"
                                  AutoPostBack="True"
                                  CssClass="c-theme form-control"
                                  ID="ddlBranch"
                                  OnSelectedIndexChanged="ProcessOnBranchChange"
                                  placeholder="Chi Nhánh"
                                  runat="server" />
            </div>
            <div class="col-sm-2 control-label">
                <control:Label runat="server"
                               Text="Quản lý trực tiếp" />
            </div>
            <div class="col-sm-4 control-value">
                <asp:Label ID="lblBranchManager"
                           runat="server">
                </asp:Label>
            </div>
        </div>

        <div class="form-group">
            <div class="col-sm-2 control-label">
                <control:Label IsRequire="True"
                               runat="server"
                               Text="Loại yêu cầu" />
            </div>
            <div class="col-sm-4">
                <control:Combobox autocomplete="off"
                                  AutoPostBack="True"
                                  CssClass="c-theme form-control"
                                  ID="ddlRequestType"
                                  OnSelectedIndexChanged="ProcessOnRequestChange"
                                  placeholder="Loại yêu cầu"
                                  runat="server" />
            </div>
            <div class="col-sm-2 control-label">
                <control:Label runat="server"
                               Text="Trạng thái" />
            </div>
            <div class="col-sm-4 control-value">
                <asp:Label CssClass="c-font-bold"
                           ID="lblRequestStatus"
                           runat="server" />
            </div>
        </div>

        <div class="form-group">
            <div class="col-sm-2 control-label">
                <control:Label IsRequire="True"
                               runat="server"
                               Text="Điều chỉnh User" />
            </div>
            <div class="col-sm-4">
                <control:Combobox autocomplete="off"
                                  AutoPostBack="True"
                                  CssClass="c-theme form-control"
                                  ID="ddlUser"
                                  OnSelectedIndexChanged="ProcessOnUserChange"
                                  placeholder="Thông tin User"
                                  runat="server" />
            </div>
            <div id="DivNewBranch"
                 runat="server">
                <div class="col-sm-2 control-label">
                    <control:Label IsRequire="True"
                                   runat="server"
                                   Text="Chi Nhánh mới" />
                </div>
                <div class="col-sm-4">
                    <control:Combobox CssClass="c-theme form-control"
                                      ID="ddlNewBranch"
                                      placeholder="Chi Nhánh mới"
                                      runat="server" />
                </div>
            </div>
        </div>

        <div class="form-group">
            <div class="col-sm-2 control-label">
                <control:Label runat="server"
                               Text="Người yêu cầu" />
            </div>
            <div class="col-sm-4 control-value">
                <asp:Label ID="lblCreateUserID"
                           runat="server" />
            </div>
            <div class="col-sm-2 control-label">
                <control:Label runat="server"
                               Text="Ngày yêu cầu" />
            </div>
            <div class="col-sm-4 control-value">
                <asp:Label ID="lblCreateDateTime"
                           runat="server" />
            </div>
        </div>

        <div class="form-group">
            <div class="col-sm-2 control-label">
                <control:Label IsRequire="True"
                               runat="server"
                               Text="Lý do" />
            </div>
            <div class="col-sm-10">
                <asp:TextBox CssClass="c-theme form-control"
                             Height="70px"
                             ID="txtRequestReason"
                             placeholder="Lý do"
                             runat="server"
                             TextMode="MultiLine" />
            </div>
        </div>

        <div id="DivRoleContainer" class="col-sm-12">
            <div class="c-padding-20"
                 id="DivRoles"
                 runat="server">
                <div class="c-content-title-1 clearfix c-title-md">
                    <h3 class="c-font-bold c-font-uppercase">Danh Sách Quyền</h3>
                </div>
                <div id="DivRoleTemplate"
                     runat="server">
                    <div class="form-group">
                        <div class="col-sm-2"></div>
                        <div class="col-sm-3 control-label">
                            <control:Label HelpText="Chọn Quyền mẫu đã thiết lập theo Chức danh"
                                           ID="lblTemplate"
                                           runat="server"
                                           Text="Chọn Quyền theo Chức danh" />
                        </div>
                        <div class="col-sm-4">
                            <control:Combobox autocomplete="off"
                                              CssClass="form-control c-theme"
                                              ID="ddlTemplate"
                                              placeholder="Quyền theo Chức danh"
                                              runat="server" />
                        </div>
                        <div class="col-sm-3"></div>
                    </div>
                    <div class="form-group">
                        <div class="col-sm-2"></div>
                        <div class="c-center col-sm-7">
                            <a class="btn btn-primary"
                               href="javascript:;"
                               id="btnApply">
                                <i class="fa fa-gear"></i>Apply
                            </a>
                            <a class="btn btn-primary"
                               href="javascript:;"
                               id="btnRestore">
                                <i class="fa fa-recycle"></i>Restore
                            </a>
                            <a class="btn btn-primary"
                               href="javascript:;"
                               id="btnCollapse"
                               onclick="collapseAllPanels('#DivRoleContainer')">
                                <i class="fa fa-compress"></i>Collapse
                            </a>
                        </div>
                        <div class="col-sm-3"></div>
                    </div>
                </div>
                <div class="form-group"
                     id="DivRoleContent"
                     runat="server">
                </div>
            </div>
        </div>
    </fieldset>
</div>

<div id="DivProcess"
     runat="server">
    <h2 class="dnnFormSectionHead">
        <a href="#">THÔNG TIN XỬ LÝ</a>
    </h2>
    <fieldset>
        <div class="form-group">
            <div class="col-sm-2 control-label">
                <control:Label runat="server"
                               Text="Người xử lý" />
            </div>
            <div class="col-sm-4 control-value">
                <asp:Label ID="lblProcessUserID"
                           runat="server" />
            </div>
            <div class="col-sm-2 control-label">
                <control:Label runat="server"
                               Text="Ngày xử lý" />
            </div>
            <div class="col-sm-4 control-value">
                <asp:Label ID="lblProcessDateTime"
                           runat="server" />
            </div>
        </div>

        <div class="form-group">
            <div class="col-sm-2 control-label">
                <control:Label runat="server"
                               Text="Ghi chú" />
            </div>
            <div class="col-sm-10">
                <asp:TextBox CssClass="c-theme form-control"
                             Height="70px"
                             ID="txtRemark"
                             runat="server"
                             TextMode="MultiLine" />
            </div>
        </div>
    </fieldset>
</div>


<div class="form-group">
    <div class="col-sm-2"></div>
    <div class="col-sm-10">
        <asp:Button CssClass="btn btn-primary"
                    ID="btnSave"
                    OnClick="UpdateRequest"
                    OnClientClick="return validate();"
                    runat="server"
                    Text="Cập Nhật" />
        <asp:Button CssClass="btn btn-primary"
                    ID="btnApprove"
                    OnClick="ProcessRequest"
                    runat="server"
                    Text="Phê Duyệt" />
        <asp:Button CssClass="btn btn-default"
                    ID="btnDecline"
                    OnClick="ProcessRequest"
                    runat="server"
                    Text="Từ Chối" />
        <asp:Button CssClass="btn btn-default"
                    ID="btnCancel"
                    OnClick="ProcessRequest"
                    runat="server"
                    Text="Huỷ" />
    </div>
</div>
</div>

<asp:HiddenField ID="hidUserRequestID"
                 runat="server"
                 Visible="False" />
<asp:HiddenField ID="hidRequestStatus"
                 runat="server"
                 Visible="False" />

<asp:HiddenField ID="hidListUserRoles"
                 runat="server" />
</ContentTemplate>
</asp:UpdatePanel>

<script type="text/javascript">
    addPageLoaded(function()
    {
        $(".dnnPanels").dnnPanels({ defaultState: "open" });

        confirmMessage("#btnApply", "Bạn muốn Apply Quyền theo Chức danh?", undefined, undefined, undefined,
            applyTemplate);
        confirmMessage("#btnRestore", "Bạn muốn Restore Quyền hiện tại của User?", undefined, undefined, undefined,
            resetToDefault);
    }, true);

    function toggleGroup(element, groupId)
    {
        $(element)
            .closest("table")
            .find("input[group='" + groupId + "']")
            .attr("checked", element.checked);
    }

    function applyTemplate()
    {
        var element = getControl("ddlTemplate");
        if (validateOption(element) === false)
        {
            return;
        }

        // Clean
        $("input[name='Roles']").attr("checked", false);

        // Apply Template
        var array = element.value.split(',');
        for (var i = 0; i < array.length; i++)
        {
            $("#Role" + array[i]).attr("checked", true);
        }
    }

    function resetToDefault()
    {
        // Clean
        $("input[name='Roles']").attr("checked", false);

        // Reset to history
        var array = getControl("hidListUserRoles").value.split(',');
        for (var i = 0; i < array.length; i++)
        {
            $("#Role" + array[i]).attr("checked", true);
        }
    }

    function validate()
    {
        var array = ["ddlBranch", "ddlRequestType", "ddlUser"];
        if (validateOptionArray(array) === false)
        {
            return false;
        }

        var element = getControl("ddlNewBranch");
        if (element !== null && validateOption(element) === false)
        {
            return false;
        }

        element = getControl("txtRequestReason");
        if (element.disabled === false && validateInput(element) === false)
        {
            return false;
        }

        return true;
    }
</script>