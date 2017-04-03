<%@ Control Language="C#" AutoEventWireup="true" CodeFile="FormsLinksControl.ascx.cs" Inherits="Gms.Portal.Web.Controls.User.FormsLinksControl" %>
<ul class="nav nav-second-level collapse" runat="server">
    <asp:Repeater runat="server" ID="rptForms">
        <ItemTemplate>
            <li runat="server">
                <asp:HyperLink NavigateUrl='<%# GetLinkUrl(Eval("ID")) %>' runat="server">
                    <ce:HtmlLabel runat="server" Text='<%# Eval("Name") %>'/>
                </asp:HyperLink>
            </li>
        </ItemTemplate>
    </asp:Repeater>
</ul>

