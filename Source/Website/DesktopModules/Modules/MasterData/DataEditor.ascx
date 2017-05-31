﻿<%@ Control Language="C#" AutoEventWireup="true" CodeFile="DataEditor.ascx.cs" Inherits="DesktopModules.Modules.MasterData.DataEditor" %>

<asp:UpdatePanel ID="UpdatePanel"
                 runat="server">
    <ContentTemplate>
        <div class="form-horizontal">
            <div class="form-group">
                <h1><asp:Label runat="server" ID="lblTitle"></asp:Label></h1>
            </div>
            <div class="form-group">
                <asp:PlaceHolder ID="phMessage"
                                 runat="server" />
            </div>
            <div id="DivForm"
                 runat="server">
            </div>
            <div class="form-group">
                <div class="col-sm-2"></div>
                <div class="col-sm-10">
                    <asp:Button CssClass="btn btn-primary"
                                ID="btnSave"
                                OnClick="InsertData"
                                OnClientClick="return validate()"
                                runat="server"
                                Text="Lưu" />
                    <asp:Button CssClass="btn btn-primary"
                                ID="btnUpdate"
                                OnClick="UpdateData"
                                OnClientClick="return validate()"
                                runat="server"
                                Text="Cập Nhật" />
                    <asp:Button CssClass="btn btn-primary"
                                ID="btnDelete"
                                OnClick="DeleteData"
                                runat="server"
                                Text="Xóa" />
                </div>
            </div>
        </div>

        <asp:HiddenField ID="hidTableID"
                         runat="server"
                         Visible="False" />
        <asp:HiddenField ID="hidPrimaryKey"
                         runat="server"
                         Visible="False" />

        <asp:HiddenField ID="hidConnectionName"
                         runat="server"
                         Visible="False" />
        <asp:HiddenField ID="hidDatabaseName"
                         runat="server"
                         Visible="False" />
        <asp:HiddenField ID="hidSchemaName"
                         runat="server"
                         Visible="False" />
        <asp:HiddenField ID="hidTableName"
                         runat="server"
                         Visible="False" />

        <asp:HiddenField ID="hidAssemblyName"
                         runat="server"
                         Visible="False" />
        <asp:HiddenField ID="hidCacheName"
                         runat="server"
                         Visible="False" />
        <asp:HiddenField ID="hidCacheID"
                         runat="server"
                         Visible="False" />

        <asp:HiddenField ID="hidFields"
                         runat="server"
                         Visible="False" />
        <asp:HiddenField ID="hidIdentityField"
                         runat="server"
                         Visible="False" />
    </ContentTemplate>
</asp:UpdatePanel>

<script type="text/javascript">
    addPageLoaded(function()
    {
        $(".dnnTooltip").dnnTooltip();
        $(".combobox").combobox();
    }, false);

    function validate()
    {
        var list = getJQueryControl("DivForm").find("input[is-require='True'], textarea[is-require='True'], select[is-require='True']");
        for (var i = 0; i < list.length; i++)
        {
            var element = list.get(i);
            switch (element.tagName.toLowerCase())
            {
                case "input":
                case "textarea":
                    if (validateInput(element) === false)
                    {
                        return false;
                    }
                    break;

                case "select":    
                    if (validateOption(element) === false)
                    {
                        return false;
                    }
                    break;
            }
        }
        return true;
    }
</script>