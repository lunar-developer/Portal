<%@ Control Language="C#" AutoEventWireup="true" CodeFile="SectionCustomerInfo.ascx.cs" Inherits="DesktopModules.Modules.Application.Controls.SectionCustomerInfo" %>
<%@ Register TagPrefix="control" TagName="DoubleLabel" Src="~/controls/DoubleLabel.ascx" %>
<%@ Register TagPrefix="control" Namespace="Modules.Controls" Assembly="Modules.Controls" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.Web.UI.WebControls" Assembly="DotNetNuke.Web.Deprecated" %>


<div class="col-sm-6">
    <div class="form-group">
        <div class="col-sm-4 control-label">
            <control:DoubleLabel
                IsRequire="True"
                runat="server"
                SubText="Cust ID/ID Doc No (main)"
                Text="Số CMND/CCCD/HC" />
        </div>
        <div class="col-sm-4">
            <asp:TextBox
                CssClass="c-theme form-control"
                ID="ctrlCustomerID"
                runat="server"
                placeholder="Số CMND/CCCD/HC"/>
        </div>
        <div class="col-sm-4">
            <asp:Button
                CssClass="btn btn-primary c-margin-0" 
                TabIndex="-1"
                ID="btnSearchIDNumber"
                OnClientClick="return alertOnConstruct()"
                runat="server"
                Text="Tìm" />
        </div>
    </div>
    <div class="form-group">
        <div class="col-sm-4 control-label">
            <control:DoubleLabel 
                IsRequire="True"
                runat="server"
                SubText="ID Doc Type (main)"
                Text="Loại chứng từ" />
        </div>
        <div class="col-sm-8">
            <control:AutoComplete ID="ctrlIdentityTypeCode" runat="server" ClientIDMode="Static" />
        </div>
    </div>
    <div class="form-group">
        <div class="col-sm-4 control-label">
            <control:DoubleLabel
                runat="server"
                SubText="CIF No"
                Text="Mã KH" />
        </div>
        <div class="col-sm-8">
            <asp:TextBox 
                CssClass="form-control c-theme"
                ID="ctrlCIFNo"
                runat="server" />
        </div>
    </div>
    <div class="form-group">
        <div class="col-sm-4 control-label">
            <control:DoubleLabel
                IsRequire="True"
                runat="server"
                SubText="Basic Supp Ind"
                Text="Thẻ chính/Phụ" />
        </div>
        <div class="col-sm-8">
            <control:AutoComplete
                ID="ctrlIsBasicCard"
                ClientIDMode="Static"
                runat="server"
                OnClientSelectedIndexChanged="processOnCardIndicatorChange">
                <Items>
                    <control:ComboBoxItem Value="1" Text="Chính"/>
                    <control:ComboBoxItem Value="0" Text="Phụ"/>
                </Items>
            </control:AutoComplete>
        </div>
    </div>
    <div class="form-group">
        <div class="col-sm-4 control-label">
            <control:DoubleLabel 
                runat="server"
                SubText="Basic Card Number"
                Text="Số thẻ chính" />
        </div>
        <div class="col-sm-8">
            <asp:TextBox
                CssClass="form-control c-theme"
                placeholder="Số thẻ chính"
                ID="ctrlBasicCardNumber"
                runat="server" />
        </div>
    </div>
    <div class="form-group">
        <div class="col-sm-4 control-label">
            <control:DoubleLabel
                IsRequire="True"
                runat="server"
                SubText="Customer Name (main)"
                Text="Họ và tên" />
        </div>
        <div class="col-sm-8">
            <asp:TextBox
                ID="ctrlFullName"
                runat="server"
                CssClass="form-control c-theme"
                placeholder="Họ và tên"
                MaxLength="50"
                onblur="processOnFullNameChange()"/>
        </div>
    </div>
    <div class="form-group">
        <div class="col-sm-4 control-label">
            <control:DoubleLabel
                IsRequire="True"
                runat="server"
                SubText="Emb Name"
                Text="Tên dập trên thẻ" />
        </div>
        <div class="col-sm-8">
            <asp:TextBox
                CssClass="form-control c-theme"
                ID="ctrlEmbossName"
                MaxLength="26"
                placeholder="Tên dập trên thẻ"
                runat="server" />
        </div>
    </div>
    <div class="form-group">
        <div class="col-sm-4 control-label">
            <control:DoubleLabel
                runat="server"
                SubText="Old IC Num"
                Text="Số CMND/ HC Khác" />
        </div>
        <div class="col-sm-8">
            <asp:TextBox 
                CssClass="form-control c-theme"
                ID="ctrlOldCustomerID"
                runat="server" />
        </div>
    </div>
    <div class="form-group">
        <div class="col-sm-4 control-label">
            <control:DoubleLabel
                runat="server"
                SubText="Medical/Social insurance No"
                Text="Số BHYT/ BHXH" />
        </div>
        <div class="col-sm-8">
            <asp:TextBox
                CssClass="form-control c-theme"
                ID="ctrlInsuranceNumber"
                runat="server" />
        </div>
    </div>
    <div class="form-group">
        <div class="col-sm-3 control-label">
            <control:DoubleLabel 
                runat="server"
                SubText="Sex (main)"
                Text="Giới tính" />
        </div>
        <div class="col-sm-3">
            <control:AutoComplete ID="ctrlGender" runat="server" ClientIDMode="Static">
                <Items>
                    <control:ComboBoxItem Value="M" Text="Nam"/>
                    <control:ComboBoxItem Value="F" Text="Nữ"/>
                </Items>
            </control:AutoComplete>
        </div>
        <div class="col-sm-3 control-label">
            <control:DoubleLabel
                runat="server"
                SubText="Title (main)"
                Text="Danh xưng" />
        </div>
        <div class="col-sm-3">
            <control:AutoComplete ID="ctrlTitleOfAddress" runat="server" ClientIDMode="Static">
                <Items>
                    <control:ComboBoxItem Value="MR" Text="Ông"/>
                    <control:ComboBoxItem Value="MRS" Text="Bà"/>
                    <control:ComboBoxItem Value="MS" Text="Cô"/>
                </Items>
            </control:AutoComplete>
        </div>
    </div>
</div>


<!-- SECOND COLUMN -->
<div class="col-sm-6">
    <div class="form-group">
        <div class="col-sm-4 control-label">
            <control:DoubleLabel 
                runat="server"
                SubText="Language (main)"
                Text="Ngôn ngữ" />
        </div>
        <div class="col-sm-8">
            <control:AutoComplete ID="ctrlLanguage" runat="server" ClientIDMode="Static" />
        </div>
    </div>
    <div class="form-group">
        <div class="col-sm-4 control-label">
            <control:DoubleLabel 
                IsRequire="True"
                runat="server"
                SubText="Nationality (main)"
                Text="Quốc tịch" />
        </div>
        <div class="col-sm-8">
            <control:AutoComplete ID="ctrlNationality" runat="server" ClientIDMode="Static" />
        </div>
    </div>
    <div class="form-group">
        <div class="col-sm-4 control-label">
            <control:DoubleLabel
                IsRequire="True"
                runat="server"
                SubText="Birth Date (main)"
                Text="Ngày sinh" />
        </div>
        <div class="col-sm-8">
            <dnn:DnnDatePicker 
                Culture-DateTimeFormat-ShortDatePattern="dd/MM/yyyy"
                ID="ctrlBirthDate"
                runat="server" />
        </div>
    </div>
    <div class="form-group">
        <div class="col-sm-4 control-label">
            <control:DoubleLabel 
                IsRequire="True"
                runat="server"
                SubText="Mobile (main)"
                Text="Điện thoại di động 1" />
        </div>
        <div class="col-sm-8">
            <asp:TextBox
                CssClass="form-control c-theme"
                placeholder="Điện thoại di động 1"
                ID="ctrlMobile01"
                runat="server" />
        </div>
    </div>
    <div class="form-group">
        <div class="col-sm-4 control-label">
            <control:DoubleLabel
                runat="server"
                SubText="2nd Mobile No (main)"
                Text="Điện thoại di động 2" />
        </div>
        <div class="col-sm-8">
            <asp:TextBox
                CssClass="form-control c-theme"
                ID="ctrlMobile02"
                runat="server" />
        </div>
    </div>
    <div class="form-group">
        <div class="col-sm-4 control-label">
            <control:DoubleLabel
                runat="server"
                SubText="Email (main)/ Stmt Email"
                Text="Thư điện tử 1" />
        </div>
        <div class="col-sm-8">
            <asp:TextBox
                CssClass="form-control c-theme"
                ID="ctrlEmail01"
                runat="server" />
        </div>
    </div>
    <div class="form-group">
        <div class="col-sm-4 control-label">
            <control:DoubleLabel
                runat="server"
                SubText="2nd Email (main)"
                Text="Thư điện tử 2" />
        </div>
        <div class="col-sm-8">
            <asp:TextBox 
                CssClass="form-control c-theme"
                ID="ctrlEmail02"
                runat="server" />
        </div>
    </div>
    <div class="form-group">
        <div class="col-sm-4 control-label">
            <control:DoubleLabel 
                IsRequire="True"
                runat="server"
                SubText="Corp/Indv"
                Text="Chủ thẻ được cấp TD" />
        </div>
        <div class="col-sm-8">
            <control:AutoComplete
                Enabled="False"
                ID="ctrlIsCorporateCard"
                ClientIDMode="Static"
                runat="server">
                <Items>
                    <control:ComboBoxItem Value="0" Text="Cá Nhân"/>
                </Items>
            </control:AutoComplete>
        </div>
    </div>
    <div class="form-group">
        <div class="col-sm-4 control-label">
            <control:DoubleLabel 
                IsRequire="True"
                runat="server"
                SubText="Cust Type"
                Text="Chủ thẻ sử dụng HM" />
        </div>
        <div class="col-sm-8">
            <control:AutoComplete
                Enabled="False"
                ID="ctrlCustomerType"
                ClientIDMode="Static"
                runat="server">
                <Items>
                    <control:ComboBoxItem Value="CONSUMER" Text="Khách hàng"/>
                </Items>
            </control:AutoComplete>
        </div>
    </div>
    <div class="form-group">
        <div class="col-sm-4 control-label">
            <control:DoubleLabel 
                runat="server"
                SubText="Customer Class"
                Text="Hạng khách hàng" />
        </div>
        <div class="col-sm-8">
            <control:AutoComplete ID="ctrlCustomerClass" runat="server" ClientIDMode="Static" />
        </div>
    </div>
</div>
