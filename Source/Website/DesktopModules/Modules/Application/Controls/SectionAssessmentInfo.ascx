<%@ Control Language="C#" AutoEventWireup="true" CodeFile="SectionAssessmentInfo.ascx.cs" Inherits="DesktopModules.Modules.Application.Controls.SectionAssessmentInfo" %>
<%@ Register TagPrefix="control" TagName="DoubleLabel" Src="~/controls/DoubleLabel.ascx" %>
<%@ Register TagPrefix="control" Namespace="Modules.Controls" Assembly="Modules.Controls" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.Web.UI.WebControls" Assembly="DotNetNuke.Web.Deprecated" %>


<div class="col-sm-6">
    <div class="form-group">
        <div class="col-sm-4 control-label">
            <control:DoubleLabel
                runat="server"
                SubText="Propose"
                Text="Đề xuất" />
        </div>
        <div class="col-sm-8">
            <control:AutoComplete
                ID="ctrlDecisionCode"
                ClientIDMode="Static"
                AutoPostBack="true"
                OnSelectedIndexChanged="ProcessOnSelectDecisionCode"
                runat="server">
                <Items>
                    <control:ComboBoxItem Value="" Text="CHƯA CHỌN" />
                    <control:ComboBoxItem Value="A" Text="A - Approve" />
                    <control:ComboBoxItem Value="D" Text="D - Decline" />
                    <control:ComboBoxItem Value="C" Text="C - Cancel" />
                </Items>
            </control:AutoComplete>
        </div>
    </div>
    <div class="form-group">
        <div class="col-sm-4 control-label">
            <control:DoubleLabel
                runat="server"
                SubText="Propose Reason"
                Text="Mã đề xuất" />
        </div>
        <div class="col-sm-8">
            <control:AutoComplete
                ID="ctrlDecisionReason"
                ClientIDMode="Static"
                AutoPostBack="true"
                CheckBoxes="true"
                AllowCustomText="true"
                OnSelectedIndexChanged="ProcessOnSelectDecisionReason"
                EmptyMessage="CHƯA CHỌN"
                runat="server" />
        </div>
    </div>
    <div class="form-group">
        <div class="col-sm-4 control-label">
            <control:DoubleLabel
                runat="server"
                SubText="Assessment Content"
                Text="Nôị dung đánh giá" />
        </div>
        <div class="col-sm-8">
            <asp:TextBox
                CssClass="c-theme form-control"
                TextMode="MultiLine"
                Height="100"
                ID="ctrlAssessmentContent"
                runat="server" />
        </div>
    </div>
    <div class="form-group">
        <div class="col-sm-4 control-label">
            <control:DoubleLabel
                runat="server"
                SubText="Assessment Display Content"
                Text="Nội dung ĐVKD" />
        </div>
        <div class="col-sm-8">
            <asp:TextBox
                CssClass="form-control c-theme"
                TextMode="MultiLine"
                Height="70"
                ID="ctrlAssessmentDisplayContent"
                runat="server" />
        </div>
    </div>
    <div class="form-group">
        <div class="col-sm-4 control-label">
            <control:DoubleLabel
                runat="server"
                SubText="Credit Limit"
                Text="Hạn mức" />
        </div>
        <div class="col-sm-4">
            <asp:TextBox
                CssClass="form-control c-theme"
                ID="ctrlProposeLimit"
                runat="server" />
        </div>
        <div class="col-sm-4">
            <asp:TextBox
                CssClass="form-control c-theme"
                ID="ctrlCreditLimit"
                runat="server" />
        </div>
    </div>
    <div class="form-group">
        <div class="col-sm-4 control-label">
            <control:DoubleLabel
                runat="server"
                SubText="Installment Limit"
                Text="Hạn mức trả góp" />
        </div>
        <div class="col-sm-4">
            <asp:TextBox
                CssClass="form-control c-theme"
                ID="ctrlProposeInstallmentLimit"
                runat="server" />
        </div>
        <div class="col-sm-4">
            <asp:TextBox
                CssClass="form-control c-theme"
                ID="ctrlInstallmentLimit"
                runat="server" />
        </div>
    </div>
</div>

<div class="col-sm-6">
    <div class="form-group">
        <div class="col-sm-4 control-label">
            <control:DoubleLabel
                runat="server"
                SubText="Assessment Branch Code"
                Text="Đơn vị thẩm định" />
        </div>
        <div class="col-sm-8">
            <control:AutoComplete
                ID="ctrlAssessmentBranchCode"
                ClientIDMode="Static"
                runat="server" />
        </div>
    </div>
    <div class="form-group">
        <div class="col-sm-4 control-label">
            <control:DoubleLabel
                runat="server"
                SubText="Re-assessment Date"
                Text="Ngày tái đánh giá" />
        </div>
        <div class="col-sm-8">
            <dnn:DnnDatePicker
                Culture-DateTimeFormat-ShortDatePattern="dd/MM/yyyy"
                ID="ctrlReAssessmentDate"
                runat="server" />
        </div>
    </div>
    <div class="form-group">
        <div class="col-sm-4 control-label">
            <control:DoubleLabel
                runat="server"
                SubText="Reason of Re-assessment"
                Text="Lý do tái đánh giá" />
        </div>
        <div class="col-sm-8">
            <asp:TextBox
                CssClass="form-control c-theme"
                TextMode="MultiLine"
                Height="70"
                ID="ctrlReAssessmentReason"
                runat="server" />
        </div>
    </div>
    <div class="form-group">
        <div class="col-sm-4 control-label">
            <control:DoubleLabel
                runat="server"
                SubText="Extension Info"
                Text="Thông tin mở rộng" />
        </div>
        <div class="col-sm-8">
        </div>
    </div>
</div>
