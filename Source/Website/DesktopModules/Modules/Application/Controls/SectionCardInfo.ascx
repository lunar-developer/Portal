<%@ Control Language="C#" AutoEventWireup="true" CodeFile="SectionCardInfo.ascx.cs" Inherits="DesktopModules.Modules.Application.Controls.SectionCardInfo" %>
<%@ Register TagPrefix="control" TagName="DoubleLabel" Src="~/controls/DoubleLabel.ascx" %>
<%@ Register TagPrefix="control" Namespace="Modules.Controls" Assembly="Modules.Controls" %>


<div class="col-sm-6">
    <div class="form-group">
        <div class="col-sm-4 control-label">
            <control:DoubleLabel
                runat="server"
                SubText="Emboss Ind"
                Text="Dập thẻ vật lý" />
        </div>
        <div class="col-sm-2">
            <control:AutoComplete
                ID="ctrlEmbossIndicator"
                ClientIDMode="Static"
                runat="server">
                <Items>
                    <control:ComboBoxItem Value="Y" Text="Yes" />
                    <control:ComboBoxItem Value="N" Text="No" />
                </Items>
            </control:AutoComplete>
        </div>
        <div class="col-sm-4 control-label">
            <control:DoubleLabel
                runat="server"
                SubText="Instant Emboss Ind"
                Text="Dập thẻ trong ngày" />
        </div>
        <div class="col-sm-2">
            <control:AutoComplete
                ID="ctrlInstantEmbossIndicator"
                ClientIDMode="Static"
                runat="server">
                <Items>
                    <control:ComboBoxItem Value="N" Text="No" />
                    <control:ComboBoxItem Value="Y" Text="Yes" />
                </Items>
            </control:AutoComplete>
        </div>
    </div>



    <div class="form-group">
        <div class="col-sm-4 control-label">
            <control:DoubleLabel
                IsRequire="True"
                runat="server"
                SubText="Delivery Method"
                Text="Phương thức nhận thẻ" />
        </div>
        <div class="col-sm-8">
            <control:AutoComplete
                ID="ctrlDeliveryMethod"
                ClientIDMode="Static"
                runat="server"/>
        </div>
    </div>
    <div class="form-group">
        <div class="col-sm-4 control-label">
            <control:DoubleLabel
                runat="server"
                SubText="Despatch Branch"
                Text="Nơi nhận thẻ tại VietBank" />
        </div>
        <div class="col-sm-8">
            <control:AutoComplete
                ID="ctrlDespatchBranchCode"
                ClientIDMode="Static"
                runat="server"/>
        </div>
    </div>
    <div class="form-group">
        <div class="col-sm-4 control-label">
            <control:DoubleLabel
                runat="server"
                SubText="Delivery Address"
                Text="Nơi chuyển thẻ cho KH" />
        </div>
        <div class="col-sm-8">
            <control:AutoComplete
                ID="ctrlDeliveryAddress"
                ClientIDMode="Static"
                runat="server"/>
        </div>
    </div>
</div>


<div class="col-sm-6">
    <div class="form-group" style="height: 35px"></div>
    <div class="form-group">
        <div class="col-sm-4 control-label">
            <control:DoubleLabel
                IsRequire="True"
                runat="server"
                SubText="Statement Delivery To"
                Text="TBGD xuất theo chủ thể" />
        </div>
        <div class="col-sm-8">
            <control:AutoComplete
                ID="ctrlStatementDeliveryType"
                ClientIDMode="Static"
                runat="server"/>
        </div>
    </div>
    <div class="form-group">
        <div class="col-sm-4 control-label">
            <control:DoubleLabel
                runat="server"
                SubText="Statement Type"
                Text="Phương thức nhận TBGD" />
        </div>
        <div class="col-sm-8">
            <control:AutoComplete
                ID="ctrlStatementType"
                ClientIDMode="Static"
                runat="server"/>
        </div>
    </div>
    <div class="form-group">
        <div class="col-sm-4 control-label">
            <control:DoubleLabel
                runat="server"
                SubText="Statement Address"
                Text="Địa chỉ nhận TBGD" />
        </div>
        <div class="col-sm-8">
            <control:AutoComplete
                ID="ctrlStatementAddress"
                ClientIDMode="Static"
                runat="server"/>
        </div>
    </div>
</div>
