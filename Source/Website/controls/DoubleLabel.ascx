<%@ Control Language="C#" AutoEventWireup="false" Inherits="Modules.Controls.DoubleLabel" %>
<div class="dnnLabel double-label">    
    <label ID="label" runat="server" EnableViewState="false">
        <asp:Label ID="lblLabel" runat="server" EnableViewState="False" />   
    </label>
    <asp:LinkButton ID="cmdHelp" TabIndex="-1" runat="server" CausesValidation="False"
        EnableViewState="False" CssClass="dnnFormHelp">        
    </asp:LinkButton>
    <asp:Panel ID="pnlHelp" runat="server" CssClass="dnnTooltip">
        <div class="dnnFormHelpContent dnnClear">
            <asp:Label ID="lblHelp" runat="server" EnableViewState="False" class="dnnHelpText" />
            <a href="#" class="pinHelp"></a>
       </div>   
    </asp:Panel>
</div>

