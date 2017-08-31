<%@ Control Language="C#" AutoEventWireup="true" CodeFile="SectionFileInfo.ascx.cs" Inherits="DesktopModules.Modules.Application.Controls.SectionFileInfo" %>
<%@ Register TagPrefix="control" Namespace="Modules.Controls" Assembly="Modules.Controls" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI, Version=2013.2.717.40, Culture=neutral, PublicKeyToken=121fae78165ba3d4" %>

<telerik:RadImageEditor RenderMode="Lightweight" ID="RadImageEditor1" runat="server"
                        Width="100%" Height="100%">
    <Tools>
        <telerik:ImageEditorToolGroup>
            <telerik:ImageEditorTool CommandName="RotateRight"/>
            <telerik:ImageEditorTool CommandName="RotateLeft"/>
            <telerik:ImageEditorTool CommandName="FlipVertical"/>
            <telerik:ImageEditorTool CommandName="FlipHorizontal"/>
            <telerik:ImageEditorTool CommandName="ZoomIn"/>
            <telerik:ImageEditorTool CommandName="ZoomOut"/>
        </telerik:ImageEditorToolGroup>
    </Tools>
</telerik:RadImageEditor>

