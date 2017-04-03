<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CategoriesLinksControl.ascx.cs" Inherits="Gms.Portal.Web.Controls.User.CategoriesLinksControl" %>

<%@ Register Src="~/Controls/User/FormsLinksControl.ascx" TagPrefix="local" TagName="FormsLinksControl" %>

<asp:Repeater runat="server" ID="rptForms">
    <ItemTemplate>
        <li class="landing_link" runat="server">
            <asp:HyperLink NavigateUrl="#" runat="server" ><i class="fa fa-th-large"></i>
                <ce:Label runat="server" CssClass="nav-label" Text='<%# Eval("Name") %>' />
            </asp:HyperLink>
            <local:FormsLinksControl runat="server" Model='<%# Eval("Children") %>' />
        </li>
    </ItemTemplate>
</asp:Repeater>


    