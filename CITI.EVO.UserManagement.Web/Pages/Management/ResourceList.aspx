<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="ResourceList.aspx.cs" Inherits="CITI.EVO.UserManagement.Web.Pages.Management.ResourceList" %>

<%@ Register TagPrefix="local" TagName="ResourceControl" Src="~/Controls/ResourceControl.ascx" %>
<%@ Register TagPrefix="local" TagName="ResourcesControl" Src="~/Controls/ResourcesControl.ascx" %>
<%@ Register TagPrefix="local" TagName="ResourcesFilterControl" Src="~/Controls/Filters/ResourcesFilterControl.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphContent" runat="Server">
	<asp:UpdatePanel runat="server" ID="upnlMain" UpdateMode="Always">
		<ContentTemplate>
			<h1>
				<ce:Label runat="server">რესურსების მართვა</ce:Label>
			</h1>
			<div class="page_title_separator"></div>
			<div class="fieldset">
				<div class="box">
					<h3><ce:Label runat="server">პროექტების ჩამონათვალი</ce:Label>
					</h3>
					<div class="box_body_short">
                        <local:ResourcesFilterControl runat="server" ID="resourcesFilterControl" />
						<ce:ImageLinkButton ID="btnSearch" runat="server" ToolTip="დამატება" CssClass="icon" Text="ძებნა" OnClick="btSearch_OnClick" />
					</div>
				</div>
				<ce:ImageLinkButton ID="btNew" runat="server" ToolTip="დამატება" CssClass="icon" Text="რესურსის დამატება" OnClick="btnNew_OnClick" />
				<local:ResourcesControl runat="server" ID="resourcesControl" OnEdit="resourcesControl_OnEdit" />
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
