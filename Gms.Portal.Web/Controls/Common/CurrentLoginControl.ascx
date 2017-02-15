<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CurrentLoginControl.ascx.cs" Inherits="Gms.Portal.Web.Controls.Common.CurrentLoginControl" %>

<div>
    <ul>
        <asp:Panel runat="server" ID="pnlUser">
            <li class="pass">
                <asp:ImageButton ID="btnChangePass" ToolTip="პაროლის შეცვლა" ImageUrl="~/App_Themes/default/images/pass_icon.png" runat="server" OnClick="btChangePassword_Click" />
            </li>
            <li class="log_out">
                <ce:ImageLinkButton runat="server" ID="btLogout" DefaultImageUrl="~/App_Themes/default/images/log_out_icon.png" OnClick="btLogout_OnClick" ForeColor="white"></ce:ImageLinkButton>
            </li>
            <li class="user" style="text-align: right; padding: 35px 20px 0 15px;">
                <asp:Label runat="server" ID="lblUser">
                </asp:Label>
            </li>
        </asp:Panel>
    </ul>
</div>
<asp:Panel ID="pnlChangePassword" runat="server" Style="display: none;" CssClass="modalWindow"
    Width="333px" DefaultButton="btChangePasswordOK">
    <asp:UpdatePanel ID="upnlChangePassword" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Button ID="btChangePasswordPopup" runat="server" Style="display: none;" />
            <act:ModalPopupExtender ID="mpeChangePassword" TargetControlID="btChangePasswordPopup" PopupControlID="pnlChangePassword" runat="server" Enabled="True" BackgroundCssClass="modalBackground" />

            <asp:Label Text="პაროლის შეცვლა" runat="server" />

            <asp:Label ID="lblChangePasswordError" runat="server" ForeColor="Red"></asp:Label>

            <asp:Label Text="მიმდინარე პაროლი:" runat="server" />

            <asp:TextBox ID="tbCurrentPassword" TextMode="Password" runat="server" Width="150"></asp:TextBox>

            <asp:Label Text="ახალი პაროლი:" runat="server" />

            <asp:TextBox ID="tbNewPassword" TextMode="Password" runat="server" Width="150"></asp:TextBox>

            <asp:Label Text="გაიმეორეთ ახალი პაროლი:" runat="server" />

            <asp:TextBox ID="tbConfirmNewPassword" TextMode="Password" runat="server" Width="150"></asp:TextBox>

            <ce:ImageLinkButton ID="btChangePasswordOK" DefaultImageUrl="~/App_Themes/default/images/save_icon.png" runat="server" Text="შეცვლა" OnClick="btChangePasswordOK_Click" />
            <ce:ImageLinkButton ID="btChangePasswordCancel" DefaultImageUrl="~/App_Themes/default/images/close_icon.png" runat="server" Text="უარყოფა" OnClick="btChangePasswordCancel_Click" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Panel>
