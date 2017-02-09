<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Recovery.aspx.cs" Inherits="CITI.EVO.UserManagement.Web.Pages.User.Recovery" %>

<%@ Register TagPrefix="local" TagName="RecoveryControl" Src="~/Controls/RecoveryControl.ascx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <section id="login_page">
            <div class="main_content">
                <asp:Panel runat="server" ID="pnlCompleted" Visible="False">
                    <ce:Label runat="server" ID="lblMessage" ForeColor="white" Font-Size="17px">პაროლი შეცვლილია წარათებით</ce:Label>
                </asp:Panel>
                <asp:Panel runat="server" ID="pnlUserData">
                    <local:RecoveryControl runat="server" ID="recoveryControl" />
                    <div class="clear"></div>
                    <div style="margin-bottom: 30px;"></div>
                    <div class="box_body">
                        <ce:ImageLinkButton CssClass="login_button" runat="server" ID="btnOK" Text="შეცვლა" OnClick="btnOK_OnClick" />
                    </div>
                    <div class="wrapper"></div>
                    <asp:Label runat="server" ForeColor="white" ID="lblError"></asp:Label>
                </asp:Panel>
            </div>
        </section>
    </form>
</body>
</html>
