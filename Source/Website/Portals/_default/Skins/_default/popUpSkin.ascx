<%@ Control AutoEventWireup="false" Language="C#" Inherits="DotNetNuke.UI.Skins.Skin" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.Web.Client.ClientResourceManagement" Assembly="DotNetNuke.Web.Client" %>

<%@ Import Namespace="Website.Library.Enum" %>
<dnn:DnnJsInclude runat="server" FilePath="Resources/Shared/Scripts/jquery/jquery.hoverIntent.min.js" ForceBundle="True" ForceVersion="True"/>
<dnn:DnnJsInclude runat="server" FilePath="Resources/Shared/Scripts/dnn.jquery.js" ForceBundle="True" ForceVersion="True"/>
<%# Styles.Render(FolderEnum.BaseStyleBundle) %>
<%# Scripts.Render(FolderEnum.BaseScriptBundle) %>


<div class="c-popup">
    <div class = "c-bg-white c-content-box c-padding-tb-10">
        <div class = "container">
            <div class = "row">
                <div class = "col-sm-12">
                    <div id = "ContentPane"
                         runat = "server">
                    </div>
                </div>
            </div>
        </div>
    </div>
</div>