<%@ Control Language="C#" AutoEventWireup="true" CodeFile="BranchConfirmation.ascx.cs" Inherits="DesktopModules.Modules.UserManagement.BranchConfirmation" %>
<%@ Register TagPrefix="control" Namespace="Modules.Controls" Assembly="Modules.Controls" %>

<asp:UpdatePanel ID="updatePanel"
    runat="server">
    <ContentTemplate>
        <div class="form-group">
            <asp:PlaceHolder ID="phMessage"
                runat="server" />
        </div>
        <div class="form-horizontal">
            <div class="col-sm-12 c-font-18 c-font-bold c-margin-b-20 form-group">
                Vui lòng xác nhận thông tin Chi nhánh nơi mà bạn đang làm việc.
            </div>
            <div class="form-group">
                <div class="col-sm-6">
                    <div class="form-group col-sm-12">
                        <h3>THÔNG TIN CÁ NHÂN</h3>
                    </div>
                    <div class="form-group">
                        <div class="col-sm-3 control-label">
                            <label class="dnnLabel">Tên đăng nhập</label>
                        </div>
                        <div class="col-sm-9 control-value">
                            <asp:Label ID="lblUserName"
                                runat="server" />
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-sm-3 control-label">
                            <label class="dnnLabel">Họ và Tên</label>
                        </div>
                        <div class="col-sm-9 control-value">
                            <asp:Label ID="lblDisplayName"
                                runat="server" />
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-sm-3 control-label">
                            <label class="dnnLabel">Chi nhánh</label>
                        </div>
                        <div class="col-sm-9">
                            <control:AutoComplete
                                autocomplete="off"
                                AutoPostBack="true"
                                ID="ddlBranch"
                                OnSelectedIndexChanged="ProcessOnBranchChanged"
                                EmptyMessage="Chi nhánh"
                                runat="server" />
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-sm-3"></div>
                        <div class="col-sm-9">
                            <asp:Button CssClass="btn btn-primary"
                                ID="btnAccept"
                                OnClick="Confirm"
                                runat="server"
                                Text="Đồng ý" />
                        </div>
                    </div>
                </div>
                <div class="col-sm-1"></div>
                <div class="col-sm-5">
                    <div class="form-group col-sm-12">
                        <h3>THÔNG TIN USER QUẢN LÝ CHI NHÁNH</h3>
                    </div>
                    <div class="form-group">
                        <div class="col-sm-3 control-label">
                            <label class="dnnLabel">Họ và Tên</label>
                        </div>
                        <div class="col-sm-9 control-value">
                            <asp:Label ID="lblManagerName"
                                runat="server" />
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-sm-3 control-label">
                            <label class="dnnLabel">Email</label>
                        </div>
                        <div class="col-sm-9 control-value">
                            <asp:Label ID="lblManagerEmail"
                                runat="server" />
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-sm-3 control-label">
                            <label class="dnnLabel">Điện thoại</label>
                        </div>
                        <div class="col-sm-9 control-value">
                            <asp:Label ID="lblManagerMobile"
                                runat="server" />
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-sm-3 control-label">
                            <label class="dnnLabel">Số Extension</label>
                        </div>
                        <div class="col-sm-9 control-value">
                            <asp:Label ID="lblManagerPhoneExtension"
                                runat="server" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>

<script type="text/javascript">
    addPageLoaded(function ()
    {
        registerConfirm({
            jquery: "#" + getClientID("btnAccept"),
            onBeforeOpen: onUserSubmit,
            isUseRuntimeMessage: true,
            getRunTimeMessage: getBranchName
        });
    }, true);

    function onUserSubmit()
    {
        return validateRadOption("ddlBranch");
    }

    function getBranchName()
    {
        var element = getRadCombobox("ddlBranch");
        var text = element.get_inputDomElement().value;
        return "Bạn đang làm việc tại Chi Nhánh:<br><b>" + text + "</b>";
    }
</script>
