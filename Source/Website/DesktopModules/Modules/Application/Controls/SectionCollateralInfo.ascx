<%@ Control Language="C#" AutoEventWireup="true" CodeFile="SectionCollateralInfo.ascx.cs" Inherits="DesktopModules.Modules.Application.Controls.SectionCollateralInfo" %>
<%@ Register TagPrefix="control" TagName="DoubleLabel" Src="~/controls/DoubleLabel.ascx" %>
<%@ Register TagPrefix="control" Namespace="Modules.Controls" Assembly="Modules.Controls" %>


<div class="col-sm-6">
    <div class="form-group">
        <div class="col-sm-4 control-label">
            <control:DoubleLabel
                runat="server"
                SubText="Collateral ID"
                Text="Mã tài sản đảm bảo" />
        </div>
        <div class="col-sm-4">
            <asp:TextBox
                CssClass="c-theme form-control"
                ID="ctrlCollateralID"
                placeholder="Mã tài sản đảm bảo"
                runat="server" />
        </div>
        <div class="col-sm-4">
            <asp:Button
                CssClass="btn btn-primary c-margin-0"
                TabIndex="-1"
                ID="ctrlQueryCollateral"
                OnClick="QueryCollateral"
                OnClientClick="return onBeforeQueryCollateral();"
                runat="server"
                Text="Tìm" />
        </div>
    </div>
    <div class="form-group">
        <div class="col-sm-4 control-label">
            <control:DoubleLabel
                runat="server"
                SubText="Collateral Value"
                Text="Giá trị tài sản đảm bảo" />
        </div>
        <div class="col-sm-8">
            <asp:TextBox
                CssClass="c-theme form-control"
                ID="ctrlCollateralValue"
                ReadOnly="True"
                runat="server" />
        </div>
    </div>
    <div class="form-group">
        <div class="col-sm-4 control-label">
            <control:DoubleLabel
                runat="server"
                SubText="Credit limit to Collateral value"
                Text="Tỉ lệ cho vay" />
        </div>
        <div class="col-sm-8">
            <asp:TextBox
                CssClass="c-theme form-control"
                ID="ctrlCollateralCreditLimit"
                Enabled="False"
                runat="server" />
        </div>
    </div>
</div>


<div class="col-sm-6">
    <div class="form-group">
        <div class="col-sm-4 control-label">
            <control:DoubleLabel
                runat="server"
                SubText="Collateral Purpose"
                Text="Lý do đảm bảo" />
        </div>
        <div class="col-sm-8">
            <asp:TextBox
                CssClass="c-theme form-control"
                ID="ctrlCollateralPurpose"
                Enabled="False"
                runat="server" />
        </div>
    </div>
    <div class="form-group">
        <div class="col-sm-4 control-label">
            <control:DoubleLabel
                runat="server"
                SubText="Type of Credit"
                Text="Hình thức cho vay thẻ" />
        </div>
        <div class="col-sm-8">
            <control:AutoComplete
                ID="ctrlCollateralType"
                ClientIDMode="Static"
                Enabled="False"
                runat="server" />
        </div>
    </div>
    <div class="form-group">
        <div class="col-sm-4 control-label">
            <control:DoubleLabel
                runat="server"
                SubText="Collateral Description"
                Text="Mô tả tài sản đảm bảo" />
        </div>
        <div class="col-sm-8">
            <asp:TextBox
                CssClass="c-theme form-control"
                TextMode="MultiLine"
                Height="100"
                ID="ctrlCollateralDescription"
                Enabled="False"
                runat="server" />
        </div>
    </div>
</div>
