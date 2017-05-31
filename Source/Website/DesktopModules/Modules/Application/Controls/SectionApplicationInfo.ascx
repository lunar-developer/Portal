<%@ Control Language="C#" AutoEventWireup="true" CodeFile="SectionApplicationInfo.ascx.cs" Inherits="DesktopModules.Modules.Application.Controls.SectionApplicationInfo" %>
<%@ Register TagPrefix="control" Namespace="Modules.Controls" Assembly="Modules.Controls" %>

<div class="col-sm-6">
    <div class="form-group">
        <div class="col-sm-4 control-label">
            <label class="dnnLabel">Mã hồ sơ VSaleKit</label>
        </div>
        <div class="col-sm-8 control-value">
            <asp:Label ID="valUniqueID"
                       runat="server" />
        </div>
    </div>
    <div class="form-group">
        <div class="col-sm-4 control-label">
            <label class="dnnLabel">Mã số thứ tự</label>
        </div>
        <div class="col-sm-8 control-value">
            <asp:Label ID="valApplicationID"
                       runat="server" />
        </div>
    </div>
    <div class="form-group">
        <div class="col-sm-4 control-label">
            <label class="dnnLabel">Loại hồ sơ</label>
        </div>
        <div class="col-sm-8">
            <control:PickList autocomplete="off"
                              CssClass="form-control c-theme"
                              ID="ctrlApplicationTypeID"
                              runat="server" />
        </div>
    </div>
    <div class="form-group">
        <div class="col-sm-4 control-label">
            <label class="dnnLabel">Ưu tiên</label>
        </div>
        <div class="col-sm-8">
            <asp:DropDownList autocomplete="off"
                              CssClass="form-control c-theme"
                              ID="ctrlPriority"
                              runat="server">
                <asp:ListItem Value="NORMAL">Thường</asp:ListItem>
                <asp:ListItem Value="FAST">Gấp</asp:ListItem>
                <asp:ListItem Value="VIP">VIP</asp:ListItem>
            </asp:DropDownList>
        </div>
    </div>
</div>

<!-- SECOND COLUMN -->
<div class="col-sm-6">
    <div class="form-group">
        <div class="col-sm-4 control-label">
            <label class="dnnLabel">Tình trạng hồ sơ</label>
        </div>
        <div class="col-sm-8 control-value">
            <asp:Label ID="valApplicationStatus"
                       runat="server" />
        </div>
    </div>
    <div class="form-group">
        <div class="col-sm-4 control-label">
            <label class="dnnLabel">Đề xuất</label>
        </div>
        <div class="col-sm-8 control-value">
            <asp:Label ID="valDecisionCode"
                       runat="server" />
        </div>
    </div>
    <div class="form-group">
        <div class="col-sm-4 control-label">
            <label class="dnnLabel">Người thao tác cuối</label>
        </div>
        <div class="col-sm-8 control-value">
            <asp:Label ID="valModifyUserID"
                       runat="server" />
        </div>
    </div>
    <div class="form-group">
        <div class="col-sm-4 control-label">
            <label class="dnnLabel">Ngày thao tác cuối</label>
        </div>
        <div class="col-sm-8 control-value">
            <asp:Label ID="valModifyDateTime"
                       runat="server" />
        </div>
    </div>
</div>

<!-- ONE COLUMN -->
<div class="clearfix"></div>
<div class="col-sm-2 control-label">
    <label class="dnnLabel">Ghi chú</label>
</div>
<div class="col-sm-10 control-value">
    <asp:Label ID="valApplicationRemark"
               runat="server" />
</div>