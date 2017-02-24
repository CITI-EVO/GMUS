<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="ResourceList.aspx.cs" Inherits="CITI.EVO.UserManagement.Web.Pages.Management.ResourceList" %>

<%@ Register TagPrefix="local" TagName="ResourceControl" Src="~/Controls/ResourceControl.ascx" %>
<%@ Register TagPrefix="local" TagName="ResourcesControl" Src="~/Controls/ResourcesControl.ascx" %>
<%@ Register TagPrefix="local" TagName="ResourcesFilterControl" Src="~/Controls/Filters/ResourcesFilterControl.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphContent" runat="Server">
    <asp:UpdatePanel runat="server" ID="upnlMain" UpdateMode="Always">
        <ContentTemplate>

            <div class="col-lg-12">
                <div class="ibox float-e-margins">
                    <div class="ibox-title">
                        <h5>პროექტების ჩამონათვალი</h5>
                    </div>
                    <div class="ibox-content">
                        <div class="row">
                            <div class="form-group">
                                <local:ResourcesFilterControl runat="server" ID="resourcesFilterControl" />
                            </div>
                            <div class="form-group">
                                <asp:LinkButton ID="btnSearch" runat="server" ToolTip="ძებნა" CssClass="btn btn-primary fa fa-search" OnClick="btSearch_OnClick" />
                                <asp:LinkButton ID="btNew" runat="server" ToolTip="რესურსის დამატება" CssClass="btn btn-primary fa fa-plus" OnClick="btnNew_OnClick" />
                            </div>
                        </div>
                        <div class="row">
                            <local:ResourcesControl runat="server" ID="resourcesControl" OnEdit="resourcesControl_OnEdit" />

                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:Panel ID="pnlResource" runat="server" Style="display: none;" CssClass="modalWindow" DefaultButton="btnOK">
        <asp:UpdatePanel ID="upnlResource" runat="server">
            <ContentTemplate>
                <asp:Button ID="btResourceFake" runat="server" Style="display: none;" />
                <act:ModalPopupExtender ID="mpeResource" runat="server" PopupControlID="pnlResource" BackgroundCssClass="modalBackground" TargetControlID="btResourceFake" />
                <div class="popup">
                    <div class="popup_fieldset">
                        <h2>
                            <asp:Label runat="server">რესურსი</asp:Label>
                        </h2>
                        <asp:Label ID="lblErrorMessage" runat="server" ForeColor="Red"></asp:Label>
                        <div class="title_separator"></div>

                        <local:ResourceControl runat="server" ID="resourceControl" />
                    </div>
                    <div class="fieldsetforicons">
                        <div class="left">
                            <ce:ImageLinkButton ID="btnOK" CssClass="icon" runat="server" Text="შენახვა" ToolTip="შენახვა" OnClick="btnOK_Click" />
                        </div>
                        <div class="right">
                            <ce:ImageLinkButton ID="btnCancel" CssClass="icon" runat="server" Text="დახურვა" ToolTip="დახურვა" />
                        </div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </asp:Panel>
</asp:Content>
