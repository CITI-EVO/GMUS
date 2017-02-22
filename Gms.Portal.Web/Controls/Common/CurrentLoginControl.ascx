<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CurrentLoginControl.ascx.cs" Inherits="Gms.Portal.Web.Controls.Common.CurrentLoginControl" %>

<div>
    <ul runat="server" id="pnlUser">
        <li class="pass">
            <asp:LinkButton ID="btnChangePass" ToolTip="პაროლის შეცვლა" runat="server" OnClick="btChangePassword_Click"> <i class="fa fa-key"></i> პაროლის შეცვლა   </asp:LinkButton>
        </li>
        <li class="log_out">
            <ce:ImageLinkButton runat="server" ID="btLogout" OnClick="btLogout_OnClick" ForeColor="white"></ce:ImageLinkButton>
        </li>
        <li class="user" style="text-align: right; padding: 35px 20px 0 15px;">
            <asp:Label runat="server" ID="lblUser">
            </asp:Label>
        </li>
    </ul>
</div>
