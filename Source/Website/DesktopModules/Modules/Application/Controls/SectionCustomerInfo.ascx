<%@ Control Language="C#" AutoEventWireup="true" CodeFile="SectionCustomerInfo.ascx.cs" Inherits="DesktopModules.Modules.Application.Controls.SectionCustomerInfo" %>
<%@ Register TagPrefix="control" TagName="DoubleLabel" Src="~/controls/DoubleLabel.ascx" %>
<%@ Register TagPrefix="control" Namespace="Modules.Controls" Assembly="Modules.Controls" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.Web.UI.WebControls" Assembly="DotNetNuke.Web.Deprecated" %>


<div class="col-sm-6">
    <div class="form-group">
        <div class="col-sm-4 control-label">
            <control:DoubleLabel IsRequire="True"
                                 runat="server"
                                 SubText="Cust ID/ID Doc No (main)"
                                 Text="Số CMND/CCCD/HC" />
        </div>
        <div class="col-sm-4">
            <asp:TextBox CssClass="c-theme form-control"
                         ID="ctrlCustomerID"
                         runat="server" />
        </div>
        <div class="col-sm-4">
            <asp:Button CssClass="btn btn-primary c-margin-0"
                        ID="btnSearchIDNumber"
                        runat="server"
                        Text="Tìm" />
        </div>
    </div>
    <div class="form-group">
        <div class="col-sm-4 control-label">
            <control:DoubleLabel IsRequire="True"
                                 runat="server"
                                 SubText="ID Doc Type (main)"
                                 Text="Loại chứng từ" />
        </div>
        <div class="col-sm-8">
            <control:PickList autocomplete="off"
                              CssClass="form-control c-theme"
                              ID="ctrlIdentityTypeCode"
                              runat="server" />
        </div>
    </div>
    <div class="form-group">
        <div class="col-sm-4 control-label">
            <control:DoubleLabel runat="server"
                                 SubText="CIF No"
                                 Text="Mã KH" />
        </div>
        <div class="col-sm-8">
            <asp:TextBox CssClass="form-control c-theme"
                         ID="ctrlCIFNo"
                         runat="server" />
        </div>
    </div>
    <div class="form-group">
        <div class="col-sm-4 control-label">
            <control:DoubleLabel IsRequire="True"
                                 runat="server"
                                 SubText="Basic Supp Ind"
                                 Text="Thẻ chính/Phụ" />
        </div>
        <div class="col-sm-8">
            <asp:DropDownList autocomplete="off"
                              CssClass="form-control c-theme"
                              ID="ctrlIsBasicCard"
                              runat="server">
                <asp:ListItem Value="1">Chính</asp:ListItem>
                <asp:ListItem Value="0">Phụ</asp:ListItem>
            </asp:DropDownList>
        </div>
    </div>
    <div class="form-group">
        <div class="col-sm-4 control-label">
            <control:DoubleLabel runat="server"
                                 SubText="Basic Card Number"
                                 Text="Số thẻ chính" />
        </div>
        <div class="col-sm-8">
            <asp:TextBox CssClass="form-control c-theme"
                         ID="ctrlBasicCardNumber"
                         runat="server" />
        </div>
    </div>
    <div class="form-group">
        <div class="col-sm-4 control-label">
            <control:DoubleLabel IsRequire="True"
                                 runat="server"
                                 SubText="Customer Name (main)"
                                 Text="Họ và tên" />
        </div>
        <div class="col-sm-8">
            <asp:TextBox CssClass="form-control c-theme"
                         ID="ctrlFullName"
                         MaxLength="50"
                         runat="server" />
        </div>
    </div>
    <div class="form-group">
        <div class="col-sm-4 control-label">
            <control:DoubleLabel IsRequire="True"
                                 runat="server"
                                 SubText="Emb Name"
                                 Text="Tên dập trên thẻ" />
        </div>
        <div class="col-sm-8">
            <asp:TextBox CssClass="form-control c-theme"
                         ID="ctrlEmbossName"
                         MaxLength="26"
                         runat="server" />
        </div>
    </div>
    <div class="form-group">
        <div class="col-sm-4 control-label">
            <control:DoubleLabel runat="server"
                                 SubText="Old IC Num"
                                 Text="Số CMND/ HC Khác" />
        </div>
        <div class="col-sm-8">
            <asp:TextBox CssClass="form-control c-theme"
                         ID="ctrlOldCustomerID"
                         runat="server" />
        </div>
    </div>
    <div class="form-group">
        <div class="col-sm-4 control-label">
            <control:DoubleLabel runat="server"
                                 SubText="Medical/Social insurance No"
                                 Text="Số BHYT/ BHXH" />
        </div>
        <div class="col-sm-8">
            <asp:TextBox CssClass="form-control c-theme"
                         ID="ctrlInsuranceNumber"
                         runat="server" />
        </div>
    </div>
    <div class="form-group">
        <div class="col-sm-3 control-label">
            <control:DoubleLabel runat="server"
                                 SubText="Sex (main)"
                                 Text="Giới tính" />
        </div>
        <div class="col-sm-3">
            <asp:DropDownList autocomplete="off"
                              CssClass="form-control c-theme"
                              ID="ctrlGender"
                              runat="server">
                <asp:ListItem Value="M">Nam</asp:ListItem>
                <asp:ListItem Value="F">Nữ</asp:ListItem>
            </asp:DropDownList>
        </div>
        <div class="col-sm-3 control-label">
            <control:DoubleLabel runat="server"
                                 SubText="Title (main)"
                                 Text="Danh xưng" />
        </div>
        <div class="col-sm-3">
            <asp:DropDownList autocomplete="off"
                              CssClass="form-control c-theme"
                              ID="ctrlTitleOfAddress"
                              runat="server">
                <asp:ListItem Value="MR">Ông</asp:ListItem>
                <asp:ListItem Value="MRS">Bà</asp:ListItem>
                <asp:ListItem Value="MS">Cô</asp:ListItem>
            </asp:DropDownList>
        </div>
    </div>
</div>

<!-- SECOND COLUMN -->
<div class="col-sm-6">
    <div class="form-group">
        <div class="col-sm-4 control-label">
            <control:DoubleLabel runat="server"
                                 SubText="Language (main)"
                                 Text="Ngôn ngữ" />
        </div>
        <div class="col-sm-8">
            <control:Combobox autocomplete="off"
                              CssClass="form-control c-theme"
                              ID="ctrlLanguage"
                              runat="server" />
        </div>
    </div>
    <div class="form-group">
        <div class="col-sm-4 control-label">
            <control:DoubleLabel IsRequire="True"
                                 runat="server"
                                 SubText="Nationality (main)"
                                 Text="Quốc tịch" />
        </div>
        <div class="col-sm-8">
            <control:Combobox autocomplete="off"
                              CssClass="form-control c-theme"
                              ID="ctrlNationality"
                              runat="server" />
        </div>
    </div>
    <div class="form-group">
        <div class="col-sm-4 control-label">
            <control:DoubleLabel IsRequire="True"
                                 runat="server"
                                 SubText="Birth Date (main)"
                                 Text="Ngày sinh" />
        </div>
        <div class="col-sm-8">
            <dnn:DnnDatePicker Culture-DateTimeFormat-ShortDatePattern="dd/MM/yyyy"
                               ID="ctrlBirthDate"
                               runat="server" />
        </div>
    </div>
    <div class="form-group">
        <div class="col-sm-4 control-label">
            <control:DoubleLabel IsRequire="True"
                                 runat="server"
                                 SubText="Mobile (main)"
                                 Text="Điện thoại di động 1" />
        </div>
        <div class="col-sm-8">
            <asp:TextBox CssClass="form-control c-theme"
                         ID="ctrlMobile01"
                         runat="server" />
        </div>
    </div>
    <div class="form-group">
        <div class="col-sm-4 control-label">
            <control:DoubleLabel runat="server"
                                 SubText="2nd Mobile No (main)"
                                 Text="Điện thoại di động 2" />
        </div>
        <div class="col-sm-8">
            <asp:TextBox CssClass="form-control c-theme"
                         ID="ctrlMobile02"
                         runat="server" />
        </div>
    </div>
    <div class="form-group">
        <div class="col-sm-4 control-label">
            <control:DoubleLabel runat="server"
                                 SubText="Email (main)/ Stmt Email"
                                 Text="Thư điện tử 1" />
        </div>
        <div class="col-sm-8">
            <asp:TextBox CssClass="form-control c-theme"
                         ID="ctrlEmail01"
                         runat="server" />
        </div>
    </div>
    <div class="form-group">
        <div class="col-sm-4 control-label">
            <control:DoubleLabel runat="server"
                                 SubText="2nd Email (main)"
                                 Text="Thư điện tử 2" />
        </div>
        <div class="col-sm-8">
            <asp:TextBox CssClass="form-control c-theme"
                         ID="ctrlEmail02"
                         runat="server" />
        </div>
    </div>
    <div class="form-group">
        <div class="col-sm-4 control-label">
            <control:DoubleLabel IsRequire="True"
                                 runat="server"
                                 SubText="Corp/Indv"
                                 Text="Chủ thẻ được cấp TD" />
        </div>
        <div class="col-sm-8">
            <asp:DropDownList CssClass="form-control c-theme"
                              Enabled="False"
                              ID="ctrlIsCorporateCard"
                              runat="server">
                <asp:ListItem Value="0">Cá Nhân</asp:ListItem>
            </asp:DropDownList>
        </div>
    </div>
    <div class="form-group">
        <div class="col-sm-4 control-label">
            <control:DoubleLabel IsRequire="True"
                                 runat="server"
                                 SubText="Cust Type"
                                 Text="Chủ thẻ sử dụng HM" />
        </div>
        <div class="col-sm-8">
            <asp:DropDownList CssClass="form-control c-theme"
                              Enabled="False"
                              ID="ctrlCustomerType"
                              runat="server">
                <asp:ListItem Value="CONSUMER">Khách hàng</asp:ListItem>
            </asp:DropDownList>
        </div>
    </div>
    <div class="form-group">
        <div class="col-sm-4 control-label">
            <control:DoubleLabel runat="server"
                                 SubText="Customer Class"
                                 Text="Hạng khách hàng" />
        </div>
        <div class="col-sm-8">
            <control:PickList CssClass="form-control c-theme"
                              ID="ctrlCustomerClass"
                              runat="server" />
        </div>
    </div>
</div>