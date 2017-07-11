<%@ Control Language="C#" AutoEventWireup="true" CodeFile="SectionPolicyInfo.ascx.cs" Inherits="DesktopModules.Modules.Application.Controls.SectionPolicyInfo" %>
<%@ Register TagPrefix="control" TagName="DoubleLabel" Src="~/controls/DoubleLabel.ascx" %>
<%@ Register TagPrefix="control" Namespace="Modules.Controls" Assembly="Modules.Controls" %>


<div class="col-sm-6">
    <div class="form-group">
        <div class="col-sm-4 control-label">
            <control:DoubleLabel
                runat="server"
                SubText="Credit Policy"
                Text="Điều kiện cấp tín dụng" />
        </div>
        <div class="col-sm-8">
            <control:AutoComplete
                ID="ctrlPolicyCode"
                ClientIDMode="Static"
                runat="server"/>
        </div>
    </div>
    <div class="form-group">
        <div class="col-sm-4 control-label">
            <control:DoubleLabel
                runat="server"
                SubText="Num of Document"
                Text="Số tờ trình" />
        </div>
        <div class="col-sm-8">
            <asp:TextBox
                CssClass="c-theme form-control"
                ID="ctrlNumOfDocument"
                runat="server" />
        </div>
    </div>
    <div class="form-group">
        <div class="col-sm-4 control-label">
            <control:DoubleLabel
                runat="server"
                SubText="Membership ID"
                Text="Mã thành viên" />
        </div>
        <div class="col-sm-8">
            <asp:TextBox
                CssClass="c-theme form-control"
                ID="ctrlMembershipID"
                runat="server" />
        </div>
    </div>
    <div class="form-group">
        <div class="col-sm-4 control-label">
            <control:DoubleLabel
                runat="server"
                SubText="Guarantor Name"
                Text="Người bảo lãnh" />
        </div>
        <div class="col-sm-8">
            <asp:TextBox
                CssClass="form-control c-theme"
                ID="ctrlGuarantorName"
                runat="server" />
        </div>
    </div>
</div>

<div class="col-sm-6">
    <div class="form-group">
        <div class="col-sm-4 control-label">
        </div>
        <div class="col-sm-8">
        </div>
    </div>
</div>