<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="ProjectsList.aspx.cs" Inherits="CITI.EVO.UserManagement.Web.Pages.Management.ProjectsList" %>

<%@ Register TagPrefix="local" TagName="ProjectControl" Src="~/Controls/ProjectControl.ascx" %>
<%@ Register TagPrefix="local" TagName="ProjectsControl" Src="~/Controls/ProjectsControl.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphContent" runat="Server">
    <h1>მოდულები
    </h1>
    <div class="page_title_separator"></div>
    <div class="fieldset">
        <ce:ImageLinkButton runat="server" CssClass="icon" ID="btNewProject" ToolTip="მოდულის დამატება"
            Text="მოდულის დამატება"
            OnClick="btNewProject_Click" />
        <asp:UpdatePanel runat="server" ID="upnlMain" UpdateMode="Always">
            <ContentTemplate>
                <local:ProjectsControl runat="server" ID="projectsControl" OnEdit="projectsControl_OnEdit" OnDelete="projectsControl_OnDelete" />
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>

    <asp:Panel ID="pnlProject" runat="server" Style="display: none;" CssClass="modalWindow" DefaultButton="btnOK">
        <asp:UpdatePanel ID="upnlProject" runat="server">
            <ContentTemplate>
                <asp:Button ID="btnProjectFake" runat="server" Style="display: none;" />
                <act:ModalPopupExtender ID="mpeProject" TargetControlID="btnProjectFake" PopupControlID="pnlProject" CancelControlID="btnCancel" runat="server" Enabled="True" BackgroundCssClass="modalBackground" />
                <div class="popup">
                    <div class="popup_fieldset">
                        <ce:Label ID="lblErrorMessage" runat="server" ForeColor="Red"></ce:Label>
                        <h2>
                            <ce:Label runat="server">მოდულის დამატება</ce:Label>
                        </h2>
                        <div class="clear"></div>
                        <div class="title_separator"></div>
                        <local:ProjectControl runat="server" ID="projectControl" />
                    </div>
                    <div class="fieldsetforicons">
                        <div class="left">
                            <ce:ImageLinkButton ID="btnOK" CssClass="icon" runat="server" Text="შენახვა" ToolTip="შენახვა" OnClick="btnOK_OnClick" />
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
