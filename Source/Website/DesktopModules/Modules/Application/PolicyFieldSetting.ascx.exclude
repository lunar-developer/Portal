<%@ Control Language="C#" AutoEventWireup="false" CodeFile="PolicyFieldSetting.ascx.cs" Inherits="DesktopModules.Modules.Application.PolicyFieldSetting" %>
<%@ Register Src="~/controls/LabelControl.ascx" TagName="Label" TagPrefix="dnn" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<asp:UpdatePanel ID="updatePanel"
                 runat="server">
    <ContentTemplate>
        <div class="form-horizontal">
            <div class="form-group">
                <asp:PlaceHolder ID="phMessage"
                                 runat="server" />
            </div>

            <div class="form-group">
                <div class="col-sm-2">
                    <label>Chính sách</label>
                </div>
                <div class="col-sm-4">
                    <asp:DropDownList CssClass="form-control"
                                      ID="ddlPolicy"
                                      runat="server">
                    </asp:DropDownList>
                    <asp:HiddenField ID="hidPolicy"
                                     runat="server"
                                     Visible="False" />
                </div>
                <div class="col-sm-4">
                    <asp:Button CssClass="btn btn-primary c-margin-0"
                                ID="btnLoad"
                                OnClick="LoadData"
                                runat="server"
                                Text="Xem" />
                </div>
            </div>
            

            <div class="form-group">
                <div class="c-bg-blue c-line-center"></div>
            </div>

            <div class="form-group">
                <div class="col-sm-6 control-label">
                    <label>Chính sách</label>
                </div>
                <div class="col-sm-8">
                    <asp:Label ID="lbForPolicyField"
                               runat="server"
                               Text="">
                    </asp:Label>
                </div>
            </div>

            <div class="form-group">
                <div class="col-sm-6 control-label"
                     style="text-align: left !important;">
                    <asp:Label ID="lbTotalPolicy"
                               runat="server"
                               Text="">
                    </asp:Label>
                </div>
            </div>

            <div class="form-group">
                <div class="col-sm-2">
                    <input class="form-control"
                           id="search"
                           name=""
                           type="text"
                           value="">
                </div>
                <div class="col-sm-1">
                    <button class="btn btn-block btn-large btn-primary"
                            id="btnClearFiler"
                            style="margin: 0px;"
                            type="button">
                        Clear
                    </button>
                </div>
                <div class="col-sm-2 control-label">
                    <dnn:Label ID="Label1"
                               runat="server"
                               Text="Filter box left" />
                </div>
                <div class="col-sm-1">
                    <div class="c-checkbox has-info">
                        <asp:CheckBox ID="CheckBox1"
                                      runat="server" />
                        <label for="<%= CheckBox1.ClientID %>">
                        <span class="inc"></span><span class="check"></span><span class="box"></span>
                    </div>
                </div>
                <div class="col-sm-2 control-label">
                    <dnn:Label ID="Label2"
                               runat="server"
                               Text="Filter box right" />
                </div>
                <div class="col-sm-1">
                    <div class="c-checkbox has-info">
                        <asp:CheckBox ID="CheckBox2"
                                      runat="server" />
                        <label for="<%= CheckBox2.ClientID %>">
                        <span class="inc"></span><span class="check"></span><span class="box"></span>
                    </div>
                </div>
            </div>

            <div class="form-group">
                <div class="demo-container size-narrow"
                     id="DemoContainer1"
                     runat="server">

                    <div class="wrapper">

                        <telerik:RadListBox AllowTransfer="true"
                                            ButtonSettings-AreaWidth="10%"
                                            Height="200px"
                                            ID="RadListBoxSource"
                                            RenderMode="Lightweight"
                                            runat="server"
                                            SelectionMode="Multiple"
                                            TransferToID="RadListBoxDestination"
                                            Width="48%">
                        </telerik:RadListBox>
                        <telerik:RadListBox AllowDelete="True"
                                            AllowReorder="True"
                                            ButtonSettings-AreaWidth="35px"
                                            Height="200px"
                                            ID="RadListBoxDestination"
                                            RenderMode="Lightweight"
                                            runat="server"
                                            Width="50%">
                        </telerik:RadListBox>
                    </div>
                </div>
            </div>
            <div class="form-group">
                <asp:Button CssClass="btn btn-primary"
                            OnClick="Submit"
                            runat="server"
                            Text="Submit" />
            </div>
        </div>

    </ContentTemplate>
</asp:UpdatePanel>

<script type="text/javascript">
    addPageLoaded(function()
        {
            $('#search').keyup(function(event)
            {
                /* Act on the event */
                if (getControl("CheckBox1").checked && getControl("CheckBox2").checked)
                {
                    search($('.rlbList li'));
                }
                else if (getControl("CheckBox1").checked)
                {
                    search($('#' + getClientID('RadListBoxSource') + ' .rlbList li'));
                }
                else if (getControl("CheckBox2").checked)
                {
                    search($('#' + getClientID('RadListBoxDestination') + ' .rlbList li'));
                }
            });

            getJQueryControl("CheckBox1").click(function()
            {
                if ($(this).is(":checked"))
                {
                    search($('#' + getClientID('RadListBoxSource') + ' .rlbList li'));
                }
            });

            getJQueryControl("CheckBox2").click(function()
            {
                if ($(this).is(":checked"))
                {
                    search($('#' + getClientID('RadListBoxDestination') + ' .rlbList li'));
                }
            });

            function search(idFilter)
            {
                idFilter.each(function(index, el)
                {
                    var key = $('#search').val();
                    var valSearch = $(this).children('span').text();
                    if (valSearch.toLowerCase().indexOf(key) >= 0)
                    {
                        $(this).removeClass('hidden');
                    }
                    else
                    {
                        $(this).addClass('hidden');
                    }
                });
            }

            function clearFiler(idFilter)
            {
                idFilter.each(function(index, el)
                {
                    $(this).removeClass('hidden');
                });
            }

            $("#btnClearFiler").click(function()
            {
                $('#search').val('');
                clearFiler($('#' + getClientID('RadListBoxDestination') + ' .rlbList li'));
                clearFiler($('#' + getClientID('RadListBoxSource') + ' .rlbList li'));
            });
        },
        true);
</script>