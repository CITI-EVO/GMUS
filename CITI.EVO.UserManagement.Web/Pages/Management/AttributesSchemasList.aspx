<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="AttributesSchemasList.aspx.cs" Inherits="CITI.EVO.UserManagement.Web.Pages.Management.AttributesSchemasList" %>

<%@ Register TagPrefix="local" TagName="AttributeFieldControl" Src="~/Controls/AttributeFieldControl.ascx" %>
<%@ Register TagPrefix="local" TagName="AttributeSchemaControl" Src="~/Controls/AttributeSchemaControl.ascx" %>
<%@ Register TagPrefix="local" TagName="AttributesSchemasControl" Src="~/Controls/AttributesSchemasControl.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphContent" runat="Server">
    <asp:UpdatePanel runat="server" ID="upnlMain" UpdateMode="Always" RenderMode="Block">
        <ContentTemplate>
            <h1>
                <asp:Label runat="server">ატრიბუტების სქემები</asp:Label>
            </h1>
            <div class="page_title_separator"></div>

            <local:AttributesSchemasControl ID="attributesSchemasControl" runat="server" OnDelete="attributesSchemasControl_OnDelete" OnEdit="attributesSchemasControl_OnEdit" OnNew="attributesSchemasControl_OnNew" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:Panel ID="pnlAttributeSchema" runat="server" Style="display: none;" CssClass="modalWindow"
        Width="333px" DefaultButton="btAttributeSchemaOK">
        <asp:UpdatePanel ID="upnlAttributeSchema" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:Button ID="btAttributeSchemaPopup" runat="server" Style="display: none;" />
                <act:ModalPopupExtender ID="mpeAttributeSchema" TargetControlID="btAttributeSchemaPopup" PopupControlID="pnlAttributeSchema" runat="server" Enabled="True" BackgroundCssClass="modalBackground" />

                <div class="popup">
                    <div class="popup_fieldset">
                        <h2></h2>
                        <asp:Label ID="lblAttributeSchemaError" runat="server" ForeColor="Red"></asp:Label>
                        <div class="title_separator"></div>
                        <local:AttributeSchemaControl ID="attributeSchemaControl" runat="server" />

                        <div class="box">
                            <h3></h3>
                            <div class="box_body">
                            </div>
                        </div>
                    </div>
                    <div class="fieldsetforicons">
                        <div class="left">
                            <asp:LinkButton CssClass="icon" ID="btAttributeSchemaOK" runat="server" ToolTip="შენახვა" Text="შენახვა" OnClick="btAttributeSchemaOK_Click" />
                        </div>
                        <div class="right">
                            <asp:LinkButton CssClass="icon" ID="btAttributeSchemaCancel" runat="server" ToolTip="დახურვა" Text="დახურვა" />
                        </div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </asp:Panel>
    <asp:Panel ID="pnlAttributeSchemaNode" runat="server" Style="display: none;" CssClass="modalWindow"
        Width="333px" DefaultButton="btAttributeSchemaNodeOK">
        <asp:UpdatePanel ID="upnlAttributeSchemaNode" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:Button ID="btAttributeSchemaNodePopup" runat="server" Style="display: none;" />
                <act:ModalPopupExtender ID="mpeAttributeSchemaNode" TargetControlID="btAttributeSchemaNodePopup" PopupControlID="pnlAttributeSchemaNode" runat="server" Enabled="True" BackgroundCssClass="modalBackground" />

                <div class="popup">
                    <div class="popup_fieldset">
                        <h2></h2>
                        <asp:Label ID="Label1" runat="server" ForeColor="Red"></asp:Label>
                        <div class="title_separator"></div>
                        <local:AttributeFieldControl ID="attributeFieldControl" runat="server" />

                        <div class="box">
                            <h3></h3>
                            <div class="box_body">
                            </div>
                        </div>
                    </div>
                    <div class="fieldsetforicons">
                        <div class="left">
                            <asp:LinkButton CssClass="icon" ID="btAttributeSchemaNodeOK" runat="server" ToolTip="შენახვა" Text="შენახვა" OnClick="btAttributeSchemaNodeOK_Click" />
                        </div>
                        <div class="right">
                            <asp:LinkButton CssClass="icon" ID="btAttributeSchemaNodeCancel" runat="server" ToolTip="დახურვა" Text="დახურვა" />
                        </div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </asp:Panel>
</asp:Content>
