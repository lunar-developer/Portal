<%@ Control Language="C#" AutoEventWireup="true" CodeFile="SectionReferenceInfo.ascx.cs" Inherits="DesktopModules.Modules.Application.Controls.SectionReferenceInfo" %>
<%@ Register TagPrefix="control" TagName="DoubleLabel" Src="~/controls/DoubleLabel.ascx" %>
<%@ Register TagPrefix="control" Namespace="Modules.Controls" Assembly="Modules.Controls" %>


<div class="col-sm-6">
    <div class="form-group">
        <div class="col-sm-4 control-label">
            <control:DoubleLabel
                runat="server"
                SubText="Marital Status (Main)"
                Text="Tình trạng hôn nhân" />
        </div>
        <div class="col-sm-8">
            <control:AutoComplete
                ID="ctrlMarialStatus"
                ClientIDMode="Static"
                runat="server"/>
        </div>
    </div>
    <div class="form-group">
        <div class="col-sm-4 control-label">
            <control:DoubleLabel
                runat="server"
                SubText="Contact Type (Spouse)"
                Text="Người hôn phối" />
        </div>
        <div class="col-sm-8">
            <control:AutoComplete
                ID="ctrlContactSpouseType"
                ClientIDMode="Static"
                runat="server">
                <Items>
                    <control:ComboBoxItem Value="Wife" Text="Vợ"/>
                    <control:ComboBoxItem Value="Husband" Text="Chồng"/>
                </Items>
            </control:AutoComplete>
        </div>
    </div>
    <div class="form-group">
        <div class="col-sm-4 control-label">
            <control:DoubleLabel 
                runat="server"
                SubText="Contact Name (Spouse)"
                Text="Họ và Tên" />
        </div>
        <div class="col-sm-8">
            <asp:TextBox
                CssClass="form-control c-theme"
                ID="ctrlContactSpouseName"
                runat="server" />
        </div>
    </div>
    <div class="form-group">
        <div class="col-sm-4 control-label">
            <control:DoubleLabel 
                runat="server"
                SubText="ID Doc No (Spouse)"
                Text="CMND" />
        </div>
        <div class="col-sm-8">
            <asp:TextBox
                CssClass="form-control c-theme"
                ID="ctrlContactSpouseID"
                runat="server" />
        </div>
    </div>
    <div class="form-group">
        <div class="col-sm-4 control-label">
            <control:DoubleLabel 
                runat="server"
                SubText="Mobile (Spouse)"
                Text="Điện thoại di động" />
        </div>
        <div class="col-sm-8">
            <asp:TextBox
                CssClass="form-control c-theme"
                ID="ctrlContactSpouseMobile"
                runat="server" />
        </div>
    </div>
    <div class="form-group">
        <div class="col-sm-4 control-label">
            <control:DoubleLabel 
                runat="server"
                SubText="Company Name (Spouse)"
                Text="Công ty" />
        </div>
        <div class="col-sm-8">
            <asp:TextBox
                CssClass="form-control c-theme"
                ID="ctrlContactSpouseCompanyName"
                runat="server" />
        </div>
    </div>
    <div class="form-group">
        <div class="col-sm-4 control-label">
            <control:DoubleLabel 
                runat="server"
                SubText="Remarks (Spouse)"
                Text="Địa chỉ" />
        </div>
        <div class="col-sm-8">
            <asp:TextBox
                CssClass="form-control c-theme"
                ID="ctrlContactSpouseRemark"
                runat="server" />
        </div>
    </div>
</div>


<div class="col-sm-6">
    <div class="form-group">
        <div class="col-sm-12 control-label" style="height: 35px"></div>
    </div>
    <div class="form-group">
        <div class="col-sm-4 control-label">
            <control:DoubleLabel
                runat="server"
                SubText="Contact Type (Contact 1)"
                Text="Người thân" />
        </div>
        <div class="col-sm-8">
            <control:AutoComplete
                ID="ctrlContact01Type"
                ClientIDMode="Static"
                runat="server">
                <Items>
                    <control:ComboBoxItem Value="Father" Text="Ba"/>
                    <control:ComboBoxItem Value="Mother" Text="Mẹ"/>
                </Items>
            </control:AutoComplete>
        </div>
    </div>
    <div class="form-group">
        <div class="col-sm-4 control-label">
            <control:DoubleLabel 
                runat="server"
                SubText="Contact Name (Contact 1)"
                Text="Họ và Tên" />
        </div>
        <div class="col-sm-8">
            <asp:TextBox
                CssClass="form-control c-theme"
                ID="ctrlContact01Name"
                runat="server" />
        </div>
    </div>
    <div class="form-group">
        <div class="col-sm-4 control-label">
            <control:DoubleLabel 
                runat="server"
                SubText="ID Doc No (Contact 1)"
                Text="CMND" />
        </div>
        <div class="col-sm-8">
            <asp:TextBox
                CssClass="form-control c-theme"
                ID="ctrlContact01ID"
                runat="server" />
        </div>
    </div>
    <div class="form-group">
        <div class="col-sm-4 control-label">
            <control:DoubleLabel 
                runat="server"
                SubText="Mobile (Contact 1)"
                Text="Điện thoại di động" />
        </div>
        <div class="col-sm-8">
            <asp:TextBox
                CssClass="form-control c-theme"
                ID="ctrlContact01Mobile"
                runat="server" />
        </div>
    </div>
    <div class="form-group">
        <div class="col-sm-4 control-label">
            <control:DoubleLabel 
                runat="server"
                SubText="Company Name (Contact 1)"
                Text="Công ty" />
        </div>
        <div class="col-sm-8">
            <asp:TextBox
                CssClass="form-control c-theme"
                ID="ctrlContact01CompanyName"
                runat="server" />
        </div>
    </div>
    <div class="form-group">
        <div class="col-sm-4 control-label">
            <control:DoubleLabel 
                runat="server"
                SubText="Remarks (Contact 1)"
                Text="Địa chỉ" />
        </div>
        <div class="col-sm-8">
            <asp:TextBox
                CssClass="form-control c-theme"
                ID="ctrlContact01Remark"
                runat="server" />
        </div>
    </div>
</div>