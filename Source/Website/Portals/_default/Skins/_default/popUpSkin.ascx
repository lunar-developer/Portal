<%@ Control AutoEventWireup="false" Language="C#" Inherits="DotNetNuke.UI.Skins.Skin" %>
<%@ Import Namespace="Website.Library.Enum" %>
<%# Styles.Render(FolderEnum.BaseStyleBundle) %>
<%# Scripts.Render(FolderEnum.BaseScriptBundle) %>


<div class="c-popup">
    <div class = "c-bg-white c-content-box c-padding-tb-10">
        <div class = "container-fluid">
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