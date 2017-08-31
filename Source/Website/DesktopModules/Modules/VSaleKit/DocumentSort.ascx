<%@ Control Language="C#" AutoEventWireup="false" CodeFile="DocumentSort.ascx.cs" Inherits="DesktopModules.Modules.VSaleKit.DocumentSort" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.Web.UI.WebControls" Assembly="DotNetNuke.Web.Deprecated" %>

<style type="text/css">
    .rlbGroup {
        border: 0 !important;
    }

    .rlbList li img.rlbImage {
        max-height: 100px;
        max-width: 100px;
        margin-right: 10px !important;
    }
</style>

<asp:UpdatePanel ID="updatePanel"
    runat="server">
    <ContentTemplate>
        <div class="form-horizontal">
            <div class="form-group">
                <asp:PlaceHolder ID="phMessage"
                    runat="server" />
            </div>
            <div id="DivForm" runat="server" visible="False">
                <div class="col-sm-5">
                    <div class="form-group">
                        <div class="col-sm-4 control-label">
                            <label class="dnnLabel">Mã hồ sơ</label>
                        </div>
                        <div class="col-sm-8 control-value">
                            <asp:Label ID="lblUniqueID" runat="server" />
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-sm-4 control-label">
                            <label class="dnnLabel">Chính sách</label>
                        </div>
                        <div class="col-sm-8 control-value">
                            <asp:Label ID="lblPolicy" runat="server" />
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-sm-4 control-label">
                            <label class="dnnLabel">Loại chứng từ</label>
                        </div>
                        <div class="col-sm-8 control-value">
                            <asp:Label ID="lblDocumentType" runat="server" />
                        </div>
                    </div>
                </div>

                <div class="col-sm-7">
                    <div class="form-group">
                        <div class="col-sm-12">
                            <label>Danh sách chứng từ</label>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-sm-12">
                            <dnn:DnnListBox
                                AllowReorder="true"
                                EnableDragAndDrop="True"
                                ButtonSettings-AreaWidth="10%"
                                Height="400px"
                                ID="ListSource"
                                RenderMode="Lightweight"
                                runat="server"
                                SelectionMode="Multiple"
                                Width="100%" />
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-sm-12">
                            <asp:Button runat="server"
                                ID="btnSort"
                                OnClick="SortDocument"
                                CssClass="btn btn-primary"
                                Text="OK" />
                        </div>
                    </div>
                </div>
            </div>

            <asp:HiddenField ID="hidUniqueID"
                runat="server"
                Visible="False" />
            <asp:HiddenField ID="hidPolicyID"
                runat="server"
                Visible="False" />
            <asp:HiddenField ID="hidDocumentCode"
                runat="server"
                Visible="False" />
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
