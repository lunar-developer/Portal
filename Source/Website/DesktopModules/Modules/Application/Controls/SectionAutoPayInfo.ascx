<%@ Control Language="C#" AutoEventWireup="true" CodeFile="SectionAutoPayInfo.ascx.cs" Inherits="DesktopModules.Modules.Application.Controls.SectionAutoPayInfo" %>
<%@ Register TagPrefix="control" TagName="DoubleLabel" Src="~/controls/DoubleLabel.ascx" %>
<%@ Register TagPrefix="control" Namespace="Modules.Controls" Assembly="Modules.Controls" %>


<div class="col-sm-6">
    <div class="form-group">
        <div class="col-sm-4 control-label">
            <control:DoubleLabel
                runat="server"
                SubText="Payment Method"
                Text="Hình thức thanh toán" />
        </div>
        <div class="col-sm-8">
            <control:AutoComplete
                ID="ctrlPaymentMethod"
                ClientIDMode="Static"
                Enabled="False"
                runat="server">
                <Items>
                    <control:ComboBoxItem Value="O" Text="Other - Khác" />
                </Items>
            </control:AutoComplete>
        </div>
    </div>
    <div class="form-group">
        <div class="col-sm-4 control-label">
            <control:DoubleLabel
                runat="server"
                SubText="Pay By"
                Text="Chủ thể thanh toán" />
        </div>
        <div class="col-sm-8">
            <control:AutoComplete
                ID="ctrlPaymentSource"
                ClientIDMode="Static"
                Enabled="False"
                runat="server">
                <Items>
                    <control:ComboBoxItem Value="ACCOUNT" Text="Account - Tài Khoản" />
                </Items>
            </control:AutoComplete>
        </div>
    </div>
    <div class="form-group">
        <div class="col-sm-4 control-label">
            <control:DoubleLabel
                runat="server"
                SubText="CIF No of Acc name"
                Text="Số CIF chủ tài khoản" />
        </div>
        <div class="col-sm-4">
            <asp:TextBox
                CssClass="c-theme form-control"
                ID="ctrlPaymentCIFNo"
                runat="server" />
        </div>
        <div class="col-sm-4">
            <asp:Button
                CssClass="btn btn-primary c-margin-0"
                TabIndex="-1"
                ID="btnSearchPaymentCIFNo"
                OnClientClick="return alertOnConstruct()"
                runat="server"
                Text="Tìm" />
        </div>
    </div>
    <div class="form-group">
        <div class="col-sm-4 control-label">
            <control:DoubleLabel
                runat="server"
                SubText="Payment Account Name"
                Text="Tên chủ tài khoản" />
        </div>
        <div class="col-sm-8">
            <asp:TextBox
                CssClass="form-control c-theme"
                ReadOnly="True"
                ID="ctrlPaymentAccountName"
                runat="server" />
        </div>
    </div>
</div>


<div class="col-sm-6">
    <div class="form-group">
        <div class="col-sm-4 control-label">
            <control:DoubleLabel
                IsRequire="True"
                runat="server"
                SubText="Payment Account No"
                Text="Số tài khoản" />
        </div>
        <div class="col-sm-8">
            <control:AutoComplete
                ID="ctrlPaymentAccountNo"
                ClientIDMode="Static"
                Enabled="False"
                runat="server" />
        </div>
    </div>
    <div class="form-group">
        <div class="col-sm-4 control-label">
            <control:DoubleLabel
                runat="server"
                SubText="Payment Bank"
                Text="Chi nhánh mở tài khoản" />
        </div>
        <div class="col-sm-8">
            <asp:TextBox
                CssClass="form-control c-theme"
                ReadOnly="True"
                ID="ctrlPaymentBankCode"
                runat="server" />
        </div>
    </div>
    <div class="form-group">
        <div class="col-sm-4 control-label">
            <control:DoubleLabel
                IsRequire="True"
                runat="server"
                SubText="Direct Debit In"
                Text="Tỷ lệ thanh toán" />
        </div>
        <div class="col-sm-8">
            <control:AutoComplete
                ID="ctrlAutoPayIndicator"
                ClientIDMode="Static"
                runat="server">
                <Items>
                    <control:ComboBoxItem Value="1" Text="1 - Thanh toán tối thiểu" />
                    <control:ComboBoxItem Value="2" Text="2 - Thanh toán toàn bộ dư nợ" />
                </Items>
            </control:AutoComplete>
        </div>
    </div>
</div>